import { Component, OnInit } from '@angular/core';
import { Category } from '../category.model';
import { CategoryService } from '../category.service';
import { PaginationRequest } from 'src/app/shared/RequestModels/pagination-request.model';
import { PageEvent } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { PaginationResponse } from 'src/app/shared/Response/pagination-response.model';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css']
})
export class CategoryListComponent implements OnInit {
  categories: Category[];

  totalCount = 0;
  pageIndex = 0;
  pageSize = 5;

  constructor(private router: Router, private route: ActivatedRoute, private categoryService: CategoryService) { }

  ngOnInit(): void {
    this.route.data.subscribe((pageData: PaginationResponse) => {
      this.categories = pageData[0].data;
      this.totalCount = pageData[0].length;
    });
  }

  getPageData(pageRequest?: PaginationRequest) {
    if (pageRequest === undefined) {
      this.pageIndex = 1;
      pageRequest = new PaginationRequest(String(this.pageIndex), String(this.pageSize));
    }

    this.categoryService.getCategoriesCount().subscribe(count => {
      this.totalCount = count;
    },
      error => {
        console.log(error);
      }
    );

    this.categoryService.getCategories(pageRequest).subscribe(paginationData => {
      this.categories = paginationData.data;
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
