import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { forkJoin, Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { PaginationResponse } from 'src/app/shared/Response/pagination-response.model';
import { AppConfig } from '../../config/config';
import { PaginationRequest } from '../../shared/RequestModels/pagination-request.model';
import { FeatureTypeService } from '../feature-types.service';

@Injectable({ providedIn: 'root' })
export class FeatureListResolverService implements Resolve<PaginationResponse> {
    constructor(
        private featureTypeService: FeatureTypeService,
        private appConfig: AppConfig
    ) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot):
        Observable<PaginationResponse> | Promise<PaginationResponse> | PaginationResponse {
        const pageReq = new PaginationRequest("1", this.appConfig.setting['DefaultPageSize']);
        return forkJoin([
            this.featureTypeService.getFeatureTypes(pageReq),
            this.featureTypeService.getFeatureTypesCount()
        ])
            .pipe(
                map(result => {
                    let paginationResponse = new PaginationResponse();
                    paginationResponse = result[0];
                    paginationResponse.length = result[1];
                    return paginationResponse;
                }),
                catchError((error, pageData) => {
                    console.log(error);
                    return pageData;
                })
            );
    }
}
