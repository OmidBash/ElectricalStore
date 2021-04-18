import { NgModule } from '@angular/core';
import { CategoryListComponent } from './category-list/category-list.component';
import { CategoryItemComponent } from './category-list/category-item/category-item.component';
import { CategoriesComponent } from './categories.component';
import { CategoryRoutingModule } from './category-routing.module';
import { SharedModule } from '../shared/shared.module';
import { CategoryEditComponent } from './category-edit/category-edit.component';

@NgModule({
  declarations:
  [
    CategoryListComponent,
    CategoryItemComponent,
    CategoryEditComponent,
    CategoriesComponent
  ],
  imports: [
    SharedModule,
    CategoryRoutingModule
  ],
  exports:
  [
    CategoryListComponent,
    CategoryItemComponent,
    CategoriesComponent
  ]
})
export class CategoriesModule { }
