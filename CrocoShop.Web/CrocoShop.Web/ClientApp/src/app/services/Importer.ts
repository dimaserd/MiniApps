import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";

export interface BaseApiResponse{
    isSucceeded: boolean;
    message: string;
}

@Injectable({
    providedIn: 'root',
})
export class Importer{

    constructor(private _httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: string){
        
    }

    public ImportData() : Observable<BaseApiResponse>{
        return this._httpClient.post<BaseApiResponse>(this.baseUrl + 'Api/Importer/InitApp', {});
    } 
}