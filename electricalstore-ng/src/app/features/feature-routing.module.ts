import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../auth/auth.guard';
import { FeatureEditResolverService } from './feature-edit/feature-edit-resolver.service';
import { FeatureEditComponent } from './feature-edit/feature-edit.component';
import { FeatureListResolverService } from './feature-list/features-list-resolver.service';
import { FeaturesComponent } from './features.component';

const featureRoutes: Routes = [
    {
        path: '', component: FeaturesComponent,
        canActivate: [AuthGuard],
        resolve: [FeatureListResolverService],
        children:
            [
                {
                    path: 'new',
                    component: FeatureEditComponent,
                    resolve: [FeatureEditResolverService],
                },
                {
                    path: ':id',
                    component: FeatureEditComponent,
                    resolve: [FeatureEditResolverService],
                },
            ]
    },
];

@NgModule({
    imports: [RouterModule.forChild(featureRoutes)]
})
export class FeatureRoutingModule { }
