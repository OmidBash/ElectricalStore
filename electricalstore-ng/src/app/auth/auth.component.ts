import { Component, ComponentFactoryResolver, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Observable, Subscription } from 'rxjs';

import { NgForm } from '@angular/forms';
import { AuthResponseData, AuthService } from './auth.service';
import { Router } from '@angular/router';
import { PlaceholderDirective } from '../shared/placeholder.directive';
import { AlertMessageComponent } from '../shared/alert-message/alert-message.component';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent implements OnInit, OnDestroy {
  isLoginMode = false;
  isLoading = false;
  error!: string;
  @ViewChild(PlaceholderDirective, { static: false }) alertMessageHost: PlaceholderDirective;
  alertMessageCloseSub: Subscription;

  constructor(
    private authService: AuthService,
    private router: Router,
    private ComponentFactoryResolver: ComponentFactoryResolver
  ) { }

  ngOnInit(): void {
    this.authService.autoLogin();
  }

  ngOnDestroy() {
    if (this.alertMessageCloseSub) {
      this.alertMessageCloseSub.unsubscribe();
    }
  }

  onSwitchMode() {
    this.isLoginMode = !this.isLoginMode;
  }

  onSubmit(authForm: NgForm) {
    if (!authForm.valid) {
      return;
    }

    const email = authForm.value.email;
    const username = authForm.value.username;
    const password = authForm.value.password;

    this.isLoading = true;

    let authObservable: Observable<AuthResponseData>;

    if (this.isLoginMode) {
      authObservable = this.authService.login(email, password);
    } else {
      authObservable = this.authService.signUp(email, username, password);
    }

    authObservable.subscribe(resData => {
      this.isLoading = false;
      this.router.navigate(['']);
    }, errorMessage => {
      this.error = errorMessage as string;
      this.showError(this.error);
      this.isLoading = false;
    });

    authForm.reset();
  }

  showError(message: string) {
    const alertMessageCmpFactory = this.ComponentFactoryResolver.resolveComponentFactory(AlertMessageComponent);
    this.alertMessageHost.viewContainerRef.clear();
    const componentRef = this.alertMessageHost.viewContainerRef.createComponent(alertMessageCmpFactory);

    componentRef.instance.message = message;
    this.alertMessageCloseSub = componentRef.instance.close.subscribe(() => {
      this.alertMessageCloseSub.unsubscribe();
      this.alertMessageHost.viewContainerRef.clear();
    });
  }
}
