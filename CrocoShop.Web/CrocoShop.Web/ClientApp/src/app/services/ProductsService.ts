import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root',
})
export class ProductsService {

    constructor(private _httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    }

    public GetFilterModel(): Observable<object> {
        return this._httpClient.post<object>(this.baseUrl + 'Api/Catalogue/Filter/GetModel', {});
    }
}
