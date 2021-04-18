import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { FeatureType } from '../feature-type.model';
import { FeatureTypeService } from '../feature-types.service';

@Injectable({ providedIn: 'root' })
export class FeatureEditResolverService implements Resolve<FeatureType> {
    constructor(private featureTypeService: FeatureTypeService) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot):
        Observable<FeatureType> | Promise<FeatureType> | FeatureType {
        if (route.params['id']) {
            const id = String(route.params['id']);
            return this.featureTypeService.getFeatureTypeDetailById(id);
        } else {
            return new FeatureType();
        }
    }
}
