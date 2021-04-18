import { NgModule } from '@angular/core';
import { FeatureListComponent } from './feature-list/feature-list.component';
import { FeatureTypeItemComponent } from './feature-list/feature-type-item/feature-type-item.component';
import { FeatureEditComponent } from './feature-edit/feature-edit.component';
import { SharedModule } from '../shared/shared.module';
import { FeaturesComponent } from './features.component';
import { FeatureRoutingModule } from './feature-routing.module';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations:
  [
    FeatureListComponent,
    FeatureTypeItemComponent,
    FeatureEditComponent,
    FeaturesComponent,
  ],
  imports:
  [
    SharedModule,
    FeatureRoutingModule,
    ReactiveFormsModule
  ]
})
export class FeaturesModule { }
