import { Injectable } from '@angular/core';
import { AppConfig } from '../config/config';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Category } from './category.model';
import { PaginationRequest } from '../shared/RequestModels/pagination-request.model';
import { PaginationResponse } from '../shared/Response/pagination-response.model';
import { forkJoin, Subject } from 'rxjs';
import { ResponseData } from '../shared/Response/response.model';
import { catchError, map} from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class CategoryService {
    public categoriesChangedSub = new Subject<PaginationResponse>();

    private pathAPI = this.appConfig.setting['PathAPI'];
    private root = 'category/';

    constructor(
        private appConfig: AppConfig,
        private http: HttpClient
    ) { }

    getCategories(paginationReq?: PaginationRequest): Promise<PaginationResponse> {
        if (paginationReq === undefined) {
            paginationReq = new PaginationRequest("1", this.appConfig.setting['DefaultPageSize']);
        }

        return forkJoin([
            this.http.get<PaginationResponse>(
                this.pathAPI + this.root,
                {
                    params: new HttpParams()
                    .set('pageNumber', paginationReq.pageIndex)
                    .set('pageSize', paginationReq.pageSize)
                }),
                this.http.get<number>(this.pathAPI + this.root + 'count')
            ]).pipe(
                map(result => {
                let paginationResponse = new PaginationResponse();
                paginationResponse = result[0];
                paginationResponse.length = result[1];
                this.categoriesChangedSub.next(paginationResponse);
                return paginationResponse;
            }),
            catchError((error) => {
                console.log(error);
                return null;
            })
        ).toPromise();
    }

    getCategoryDetailById(id: string): Promise<Category> {
        return this.http.get<ResponseData>(this.pathAPI + this.root + id + '/detail').pipe(
            map(responseData => {
                const category: Category = responseData.data;
                return category;
            })
        ).toPromise();
    }

    addCategory(category: Category): Promise<Category> {
        return this.http.post<Category>(this.pathAPI + this.root, category).toPromise();
    }

    updateCategory(category: Category): Promise<Category> {
        return this.http.put<Category>(this.pathAPI + this.root, category).toPromise();
    }

    deleteCategory(id: string): Promise<Category> {
        return this.http.delete<Category>(this.pathAPI + this.root + id).toPromise();
    }
}
