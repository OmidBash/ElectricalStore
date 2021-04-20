import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { PaginationResponse } from '../../shared/Response/pagination-response.model';
import { CategoryService } from '../category.service';

@Injectable({ providedIn: 'root' })
export class CategoryListResolverService implements Resolve<PaginationResponse> {
    constructor(
        private categoryService: CategoryService,
    ) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot):
        Observable<PaginationResponse> | Promise<PaginationResponse> | PaginationResponse {
        return this.categoryService.getCategories();
    }
}
