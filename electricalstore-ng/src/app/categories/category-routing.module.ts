import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../auth/auth.guard';
import { CategoriesComponent } from './categories.component';
import { CategoryEditComponent } from './category-edit/category-edit.component';
import { CategoryListResolverService } from './category-list/category-list-resolver.service';
import { CategoryEditResolverService } from './category-edit/category-edit-resolver.service';

const categoryRoutes: Routes = [
    {
        path: '',
        component: CategoriesComponent,
        canActivate: [AuthGuard],
        resolve: [CategoryListResolverService],
        children: [
            {
                path: 'new',
                component: CategoryEditComponent,
                resolve: [CategoryEditResolverService],
            },            {
                path: ':id',
                component: CategoryEditComponent,
                resolve: [CategoryEditResolverService],
            },
        ]
    },
];

@NgModule({
    imports: [RouterModule.forChild(categoryRoutes)]
})
export class CategoryRoutingModule { }
