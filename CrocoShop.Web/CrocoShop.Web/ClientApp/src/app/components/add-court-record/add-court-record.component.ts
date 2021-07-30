import { Component, Inject, OnDestroy, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { CountRecordCreator } from "src/app/services/CountRecordCreator";
import { CourtRecord, CourtTenant } from "src/app/services/CourtRecordService";

@Component({
  selector: "app-add-court-record",
  templateUrl: "./add-court-record.component.html",
  styleUrls: ["./add-court-record.component.css"],
})
export class AddCourtRecordComponent implements OnInit, OnDestroy {
  
  tenant: CourtTenant = {
    name: "",
    phoneNumber: "",
  };

  constructor(
    @Inject(MAT_DIALOG_DATA) private record: CourtRecord,
    private dialogRef: MatDialogRef<AddCourtRecordComponent>
  ) {
    this.header = `Записать арендатора на корт ${record.court.type} ${record.court.number} на ${record.time}-${CountRecordCreator.plusTime(record.time, 1)}`;
    this.tenant = {
      name: record.tenant?.name,
      phoneNumber: record.tenant?.phoneNumber
    }
  }
  ngOnDestroy(): void {
    this.dialogRef.close({ data: this.record });
  }

  public header:string = null;
  
  ngOnInit(): void {}

  addRecord() {
    this.record.tenant = this.tenant;
    // closing itself and sending data to parent component
    this.dialogRef.close({ data: this.record });
  }
}
