import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, tap } from 'rxjs/operators';
import { BehaviorSubject, throwError } from 'rxjs';
import { User } from './user.model';
import { Router } from '@angular/router';
import { AppConfig } from '../config/config';
import { JwtHelperService } from '@auth0/angular-jwt';

export interface AuthResponseData {
  token: string;
  email: string;
  refreshToken: string;
  expiresIn: string;
  userId: string;
  registered?: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private pathAPI = this.config.setting['PathAPI'];

  userSubject = new BehaviorSubject<User>(null);
  tokenExpTimer: any;

  constructor(private http: HttpClient, private router: Router, private config: AppConfig, private jwtHelper: JwtHelperService) { }

  signUp(email: string, username: string, password: string) {
    return this.http
      .post<AuthResponseData>
      (
        this.pathAPI + 'identity/registration',
        {
          email,
          username,
          password
        }
      )
      .pipe(catchError(this.errorHandling),
        tap(resData => {
          this.handleAuthentication(resData.token);
        }));
  }

  autoLogin() {
    const token = localStorage.getItem('_token');

    if (!token) {
      return;
    }

    if (!this.jwtHelper.isTokenExpired(token)) {
      this.handleAuthentication(token);
      this.router.navigate(['category']);
    }
  }

  login(email: string, password: string) {
    return this.http
      .post<AuthResponseData>
      (
        this.pathAPI + 'identity/login',
        {
          email,
          password
        }
      )
      .pipe(
        catchError(this.errorHandling),
        tap(resData => {
          this.handleAuthentication(resData.token);
        })
      );
  }

  autoLogout(expirationDuration: number) {
    this.tokenExpTimer = setTimeout(() => this.logout(), expirationDuration);
  }

  logout() {
    this.userSubject.next(null);
    this.router.navigate(['/auth']);

    if (this.tokenExpTimer) {
      clearTimeout(this.tokenExpTimer);
    }

    this.tokenExpTimer = null;

    localStorage.removeItem('_token');
  }

  private handleAuthentication(token: string) {
    if (this.jwtHelper.isTokenExpired(token)) {
      return;
    }
    localStorage.setItem('_token', token);
    const user = this.getUserData(token);
    this.userSubject.next(user);

    console.log(user.ExpirationDuration);
    this.autoLogout(user.ExpirationDuration);
  }

  getUserData(token: string): User | null {
    const tokenData = this.jwtHelper.decodeToken(token);
    const expirationDate = this.jwtHelper.getTokenExpirationDate(token);
    return new User(tokenData.email, tokenData.id, token, expirationDate);
  }

  private errorHandling(errorRes: HttpErrorResponse) {
    let errorMessage = 'Unknown error!!';

    if (errorRes.error && errorRes.error.errors) {
      errorMessage = '';
      errorRes.error.errors.forEach(error => {
        errorMessage += error + ' ';
      });
      return throwError(errorMessage);
    }
  }
}
