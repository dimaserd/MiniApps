import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BaseApiResponse } from "./Importer";

export interface CourtTenant {
  name: string;
  phoneNumber: string;
}

export interface CourtDescription {
  type: string;
  number: string;
}

export interface CourtRecord {
  id:string;
  time: string;
  court: CourtDescription;
  tenant: CourtTenant;
}

export interface CourtSettings{
  courtTypes: string[];
  courts: CourtDescription[]
}

export interface UpdateRecordModel{
  id:string;
  tenant: CourtTenant;
}

export class DateExtensions{
  static addDays(oldDate: Date, days:number) {
    var date = new Date(oldDate.valueOf());
    date.setDate(date.getDate() + days);
    return date;
  }
}

@Injectable({
  providedIn: "root",
})
export class CourtRecordService {
  baseControllerUrl: string = "";
  constructor(
    private _httpClient: HttpClient,
    @Inject("BASE_URL") baseUrl: string
  ) {
    this.baseControllerUrl = baseUrl + "api/Court/";
  }

  public GetSettings():Observable<CourtSettings>{
    return this._httpClient.get<CourtSettings>(
      this.baseControllerUrl + `settings`
    );
  }

  public UpdateRecord(model: UpdateRecordModel): Observable<BaseApiResponse>{
    return this._httpClient.post<BaseApiResponse>(
      this.baseControllerUrl + `Records/Update`, model
    );
  }

  public GetRecordsByDay(dayShift: number):Observable<CourtRecord[]> {
    var dateNow = DateExtensions.addDays(new Date(), dayShift).toISOString();
    return this._httpClient.get<CourtRecord[]>(
      this.baseControllerUrl + `Records/GetByDay?day=${dateNow}`
    );
  }
}
