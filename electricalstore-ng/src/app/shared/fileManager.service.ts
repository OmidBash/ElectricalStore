import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AppConfig } from '../config/config';
import { ResponseData } from './Response/response.model';

export interface FileInformation{
    fileName: string;
    filePath: string;
}

@Injectable({
    providedIn: 'root'
})
export class FileManagerService {
    constructor(
        private config: AppConfig,
        private http: HttpClient
    ) { }

    private pathAPI = this.config.setting['PathAPI'];
    private root = 'file/';

    uploadFile(formData: FormData): Observable<any> {
        return this.http.post<ResponseData>(
            this.pathAPI + this.root,
            formData,
            { reportProgress: true, observe: 'response' }
        );
    }
}
