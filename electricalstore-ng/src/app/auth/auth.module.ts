import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AlertMessageComponent } from '../shared/alert-message/alert-message.component';
import { SharedModule } from '../shared/shared.module';
import { AuthComponent } from './auth.component';

@NgModule({
    declarations: [
        AuthComponent
    ],
    imports: [
        FormsModule,
        SharedModule,
        RouterModule.forChild([{ path: '', component: AuthComponent }])
    ],
    exports: [
        AuthComponent
    ],
    entryComponents: [
        AlertMessageComponent,
    ],
})
export class AuthModule { }
