import { Injectable } from '@angular/core';
import { AppConfig } from '../config/config';
import { HttpClient, HttpParams } from '@angular/common/http';
import { PaginationRequest } from '../shared/RequestModels/pagination-request.model';
import { PaginationResponse } from '../shared/Response/pagination-response.model';
import { Observable } from 'rxjs';
import { ResponseData } from '../shared/Response/response.model';
import { FeatureType } from './feature-type.model';
import { map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class FeatureTypeService {

    private pathAPI = this.config.setting['PathAPI'];
    private root = 'featuretype/';

    constructor(
        private config: AppConfig,
        private http: HttpClient
    ) { }

    getFeatureTypes(paginationReq: PaginationRequest): Observable<PaginationResponse> {
        return this.http.get<PaginationResponse>(
            this.pathAPI + this.root,
            {
                params: new HttpParams()
                    .set('pageNumber', paginationReq.pageIndex)
                    .set('pageSize', paginationReq.pageSize)
            });
    }

    getFeatureTypesCount(): Observable<number> {
        return this.http.get<number>(this.pathAPI + this.root + 'count');
    }

    getFeatureTypeDetailById(id: string): Observable<FeatureType> {
        return this.http.get<ResponseData>(this.pathAPI + this.root + id + '/detail')
            .pipe(
                map(responseData => {
                    const feature: FeatureType = responseData.data;
                    return feature;
                })
            );
    }

    addFeatureType(featureType: FeatureType) {
        return this.http.post<FeatureType>(this.pathAPI + this.root, featureType);
    }

    updateFeatureType(featureType: FeatureType) {
        return this.http.put<FeatureType>(this.pathAPI + this.root, featureType);
    }

    deleteFeatureType(id: string) {
        return this.http.delete<FeatureType>(this.pathAPI + this.root + id);
    }
}
