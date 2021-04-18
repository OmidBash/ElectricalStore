import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../auth/auth.guard';
import { ProductEditResolverService } from './product-edit/product-edit-resolver.service';
import { ProductEditComponent } from './product-edit/product-edit.component';
import { ProductListResolverService } from './product-list/products-list-resolver.service';
import { ProductsComponent } from './products.component';

const productRoutes: Routes = [
    {
        path: '', component: ProductsComponent,
        canActivate: [AuthGuard],
        resolve: [ProductListResolverService],
        children:
            [
                {
                    path: 'new',
                    component: ProductEditComponent,
                    resolve: [ProductEditResolverService]
                },
                {
                    path: ':id',
                    component: ProductEditComponent,
                    resolve: [ProductEditResolverService]
                }
            ]
    },
];

@NgModule({
    imports: [RouterModule.forChild(productRoutes)]
})
export class ProductRoutingModule { }
