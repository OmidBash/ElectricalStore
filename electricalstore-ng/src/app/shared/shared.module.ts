import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { AlertMessageComponent } from './alert-message/alert-message.component';
import { LoadingSpinnerComponent } from './loading-spinner/loading-spinner.component';
import { PlaceholderDirective } from './placeholder.directive';
import { MaterialModule } from './material.module';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

@NgModule({
    declarations: [
        LoadingSpinnerComponent,
        PlaceholderDirective,
        AlertMessageComponent,
    ],
    imports: [
        CommonModule,
        FormsModule,
        RouterModule,
        MaterialModule,
    ],
    exports: [
        CommonModule,
        FormsModule,
        RouterModule,
        MaterialModule,
        LoadingSpinnerComponent,
        PlaceholderDirective,
        AlertMessageComponent,
    ]
})
export class SharedModule {

}
