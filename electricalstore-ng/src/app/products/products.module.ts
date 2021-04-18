import { NgModule } from '@angular/core';
import { ProductListComponent } from './product-list/product-list.component';
import { ProductItemComponent } from './product-list/product-item/product-item.component';
import { ProductEditComponent } from './product-edit/product-edit.component';
import { SharedModule } from '../shared/shared.module';
import { ProductsComponent } from './products.component';
import { ProductRoutingModule } from './products-routing.module';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations:
    [
      ProductListComponent,
      ProductItemComponent,
      ProductEditComponent,
      ProductsComponent
    ],
  imports: [
    SharedModule,
    ProductRoutingModule,
    ReactiveFormsModule
  ]
})
export class ProductsModule { }
