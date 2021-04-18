import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { PaginationRequest } from 'src/app/shared/RequestModels/pagination-request.model';
import { PaginationResponse } from 'src/app/shared/Response/pagination-response.model';
import { FeatureType } from '../feature-type.model';
import { FeatureTypeService } from '../feature-types.service';

@Component({
  selector: 'app-feature-list',
  templateUrl: './feature-list.component.html',
  styleUrls: ['./feature-list.component.css']
})
export class FeatureListComponent implements OnInit {
  featureTypes: FeatureType[];

  totalCount = 0;
  pageIndex = 1;
  pageSize = 5;

  constructor(private route: ActivatedRoute, private router: Router, private featureTypeService: FeatureTypeService) { }

  ngOnInit(): void {
    this.route.data.subscribe((pageData: PaginationResponse) => {
      this.featureTypes = pageData[0].data;
      this.totalCount = pageData[0].length;
    });
  }

  getPageData(pageRequest?: PaginationRequest) {
    if (pageRequest === undefined) {
      pageRequest = new PaginationRequest(String(++this.pageIndex), String(this.pageSize));
    }

    this.featureTypeService.getFeatureTypesCount().subscribe(count => {
      this.totalCount = count;
      this.featureTypeService.getFeatureTypes(pageRequest).subscribe(paginationData => {
        this.featureTypes = paginationData.data;
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
