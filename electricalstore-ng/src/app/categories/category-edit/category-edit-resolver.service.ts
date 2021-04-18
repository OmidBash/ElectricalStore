import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { Category } from '../category.model';
import { CategoryService } from '../category.service';

@Injectable({ providedIn: 'root' })
export class CategoryEditResolverService implements Resolve<Category> {
    constructor(private categoryService: CategoryService) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot):
        Observable<Category> | Promise<Category> | Category {
        if (route.params['id']) {
            const id = String(route.params['id']);
            return this.categoryService.getCategoryDetailById(id);
        } else {
            return new Category();
        }
    }
}
