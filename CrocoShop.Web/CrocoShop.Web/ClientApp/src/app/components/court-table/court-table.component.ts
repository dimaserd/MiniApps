import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CourtRecord, CourtRecordService } from 'src/app/services/CourtRecordService';
import { AddCourtRecordComponent } from '../add-court-record/add-court-record.component';

@Component({
  selector: 'app-court-table',
  templateUrl: './court-table.component.html',
  styleUrls: ['./court-table.component.css']
})
export class CourtTableComponent implements OnInit {

  displayedColumns: string[] = ['court','position', 'name', 'weight'];

  constructor(public dialog: MatDialog, 
    private _courtRecordService: CourtRecordService,
    private _snackBar: MatSnackBar) { }

  @Input() dataSource: CourtRecord[] = [];

  ngOnInit(): void {
  }

  record(record: CourtRecord){
    this.openCreateRecordDialog(record);
  }
  
  removeRecord(record: CourtRecord){
    record.tenant = null;
    this._courtRecordService
      .UpdateRecord(record)
      .subscribe(data => this._snackBar.open(data.message,'Закрыть', {
        duration: 1500
      }));
  }

  openCreateRecordDialog(record: CourtRecord) {
    console.log(record);
    let dialogRef = this.dialog.open(AddCourtRecordComponent, {
      data: record,
      width: '600px'
    })

    dialogRef.afterClosed().subscribe(res => {
      var record = res.data as CourtRecord;
      
      if(record.tenant !== null){
        this._courtRecordService.UpdateRecord(record).subscribe(data => this._snackBar.open(data.message,'Закрыть', {
          duration: 1500
        }));
      }

    })
  }
}