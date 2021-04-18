import { Component, Input, OnInit, Output } from '@angular/core';
import { Subject } from 'rxjs';
import { Product } from '../../product.model';
import { ProductsService } from '../../products.service';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.css']
})
export class ProductItemComponent {
  @Input() product: Product;
  @Output() selectProductSub = new Subject<Product>();

  constructor(private productService: ProductsService) { }

  onSelect() {
    this.selectProductSub.next(this.product);
  }

  onRemove(id: string){
    this.productService.deleteProduct(id).subscribe();
  }
}
