import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, PRIMARY_OUTLET } from '@angular/router';
import { forkJoin, Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Category } from 'src/app/categories/category.model';
import { CategoryService } from 'src/app/categories/category.service';
import { FeatureType } from 'src/app/features/feature-type.model';
import { FeatureTypeService } from 'src/app/features/feature-types.service';
import { PaginationRequest } from 'src/app/shared/RequestModels/pagination-request.model';
import { Product } from '../product.model';
import { ProductsService } from '../products.service';

export interface IProductEditor {
    product: Product;
    categories: Category[];
    featureTypes: FeatureType[];
}

@Injectable({ providedIn: 'root' })
export class ProductEditResolverService implements Resolve<IProductEditor> {
    productEditor: IProductEditor;
    constructor(
        private productService: ProductsService,
        private categoryService: CategoryService,
        private featureTypeService: FeatureTypeService
    ) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot):
        Observable<IProductEditor> | Promise<IProductEditor> | IProductEditor {

        const pageReq = new PaginationRequest("1", "2000");
        const paramId = route.params['id'];

        let product = new Product();
        const categories = Array<Category>();
        const featureTypes = Array<FeatureType>();

        if (paramId) {
            return forkJoin([
                this.productService.getProductDetailById(String(paramId))
                    .pipe(
                        map(data => product = data),
                    ),
                this.categoryService.getCategories(pageReq).then(pageData => categories.push(...pageData.data)),
                this.featureTypeService.getFeatureTypes(pageReq)
                    .pipe(
                        map(pageData => featureTypes.push(...pageData.data))
                    ),
            ])
                .pipe(
                    map(() => {
                        return { product, categories, featureTypes };
                    }),
                    catchError((error, pageData) => {
                        console.log(error);
                        return pageData;
                    })
                );
        } else {
            return forkJoin([
                this.categoryService.getCategories(pageReq).then(pageData => categories.push(...pageData.data)),
                this.featureTypeService.getFeatureTypes(pageReq)
                    .pipe(
                        map(pageData => featureTypes.push(...pageData.data))
                    ),
            ])
                .pipe(
                    map(() => {
                        return { product, categories, featureTypes };
                    }),
                    catchError((error, pageData) => {
                        console.log(error);
                        return pageData;
                    })
                );
        }
    }
}
