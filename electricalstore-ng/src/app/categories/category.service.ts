import { Injectable } from '@angular/core';
import { AppConfig } from '../config/config';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Category } from './category.model';
import { PaginationRequest } from '../shared/RequestModels/pagination-request.model';
import { PaginationResponse } from '../shared/Response/pagination-response.model';
import { Observable, Subject } from 'rxjs';
import { ResponseData } from '../shared/Response/response.model';
import { map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class CategoryService {
    public categoriesChangedSub = new Subject();

    private pathAPI = this.config.setting['PathAPI'];
    private root = 'category/';

    constructor(
        private config: AppConfig,
        private http: HttpClient
    ) { }

    getCategories(paginationReq: PaginationRequest): Observable<PaginationResponse> {
        return this.http.get<PaginationResponse>(
            this.pathAPI + this.root,
            {
                params: new HttpParams()
                    .set('pageNumber', paginationReq.pageIndex)
                    .set('pageSize', paginationReq.pageSize)
            })
            .pipe(map(data => data));
    }

    getCategoriesCount(): Observable<number> {
        return this.http.get<number>(this.pathAPI + this.root + 'count')
        .pipe(map(data => data));
    }

    getCategoryDetailById(id: string): Observable<Category> {
        return this.http.get<ResponseData>(this.pathAPI + this.root + id + '/detail')
            .pipe(
                map(responseData => {
                    const category: Category = responseData.data;
                    return category;
                })
            );
    }

    addCategory(category: Category) {
        return this.http.post<Category>(this.pathAPI + this.root, category);
    }

    updateCategory(category: Category): Observable<Category> {
        return this.http.put<Category>(this.pathAPI + this.root, category);
    }

    deleteCategory(id: string): Observable<Category> {
        return this.http.delete<Category>(this.pathAPI + this.root + id);
    }
}
