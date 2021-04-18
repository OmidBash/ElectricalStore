import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { forkJoin, Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { AppConfig } from '../../config/config';
import { PaginationRequest } from '../../shared/RequestModels/pagination-request.model';
import { PaginationResponse } from '../../shared/Response/pagination-response.model';
import { CategoryService } from '../category.service';

@Injectable({ providedIn: 'root' })
export class CategoryListResolverService implements Resolve<PaginationResponse> {
    constructor(
        private categoryService: CategoryService,
        private appConfig: AppConfig
    ) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot):
        Observable<PaginationResponse> | Promise<PaginationResponse> | PaginationResponse {
        const pageReq = new PaginationRequest("1", this.appConfig.setting['DefaultPageSize']);
        return forkJoin([
            this.categoryService.getCategories(pageReq),
            this.categoryService.getCategoriesCount()
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
