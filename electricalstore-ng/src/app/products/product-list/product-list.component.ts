import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { PageEvent } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { PaginationRequest } from 'src/app/shared/RequestModels/pagination-request.model';
import { PaginationResponse } from 'src/app/shared/Response/pagination-response.model';
import { Product } from '../product.model';
import { ProductsService } from '../products.service';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {

  products: Product[];
  editMode = false;

  totalCount = 0;
  pageIndex = 1;
  pageSize = 5;

  constructor(private route: ActivatedRoute, private router: Router, private productService: ProductsService) { }

  ngOnInit(): void {
    this.route.data.subscribe((pageData: PaginationResponse) => {
      this.products = pageData[0].data;
      this.totalCount = pageData[0].length;
    });
  }

  getPageData(pageRequest?: PaginationRequest) {
    if (pageRequest === undefined) {
      pageRequest = new PaginationRequest(String(++this.pageIndex), String(this.pageSize));
    }

    this.productService.getProductsCount().subscribe(count => {
      this.totalCount = count;
      this.productService.getProducts(pageRequest).subscribe(paginationData => {
        this.products = paginationData.data;
      },
        error => {
          console.log(error);
        });
    },
      error => {
        console.log(error);
      }
    );
  }

  onPageChanged(pageEvent?: PageEvent) {
    const pageRequest = new PaginationRequest(String(++pageEvent.pageIndex), String(pageEvent.pageSize));
    this.getPageData(pageRequest);
  }

  onClickAdd() {
    this.router.navigate(['new'], { relativeTo: this.route });
  }
}
