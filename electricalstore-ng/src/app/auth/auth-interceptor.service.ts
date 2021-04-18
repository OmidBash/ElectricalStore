import { HttpHandler, HttpHeaders, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { exhaustMap, take } from 'rxjs/operators';
import { AuthService } from './auth.service';

@Injectable()
export class AuthInterceptorService implements HttpInterceptor {
    constructor(private authService: AuthService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler) {
        return this.authService.userSubject.pipe(
            take(1),
            exhaustMap(user => {
                if (!user || req.method === 'GET') {
                    return next.handle(req);
                }
                const modifiedReq = req.clone({
                    headers: new HttpHeaders().set('authorization', `Bearer ${user.token}`)
                });
                return next.handle(modifiedReq);
            })
        );
    }
}
