import { Injectable } from '@angular/core';
import { AppConfig } from '../config/config';
import { HttpClient, HttpParams } from '@angular/common/http';
import { PaginationRequest } from '../shared/RequestModels/pagination-request.model';
import { PaginationResponse } from '../shared/Response/pagination-response.model';
import { Observable } from 'rxjs';
import { ResponseData } from '../shared/Response/response.model';
import { Product } from './product.model';
import { map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class ProductsService {

    private pathAPI = this.config.setting['PathAPI'];
    private root = 'product/';

    constructor(
        private config: AppConfig,
        private http: HttpClient
    ) { }

    getProducts(paginationReq: PaginationRequest): Observable<PaginationResponse> {
        return this.http.get<PaginationResponse>(
            this.pathAPI + this.root,
            {
                params: new HttpParams()
                    .set('pageNumber', paginationReq.pageIndex)
                    .set('pageSize', paginationReq.pageSize)
            });
    }

    getProductsCount(): Observable<number> {
        return this.http.get<number>(this.pathAPI + this.root + 'count');
    }

    getProductDetailById(id: string): Observable<Product> {
        return this.http.get<ResponseData>(this.pathAPI + this.root + id + '/detail')
            .pipe(
                map(responseData => {
                    const product: Product = responseData.data;
                    return product;
                })
            );
    }

    addProduct(product: Product) {
        return this.http.post<Product>(this.pathAPI + this.root, product);
    }

    updateProduct(product: Product) {
        return this.http.put<Product>(this.pathAPI + this.root, product);
    }

    deleteProduct(id: string) {
        return this.http.delete<Product>(this.pathAPI + this.root + id);
    }
}
