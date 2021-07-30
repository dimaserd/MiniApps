import { Component } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import {
  CourtDescription,
  CourtRecord,
  CourtRecordService,
  CourtSettings,
} from "src/app/services/CourtRecordService";
import { FormControl } from "@angular/forms";
import { MatSliderChange } from "@angular/material/slider";
import { Title } from "@angular/platform-browser";

export interface OptionValue {
  value: string;
  viewValue: string;
}

export interface CourtDescriptionWithOptionValue {
  court: CourtDescription;
  option: OptionValue;
}

interface HomeComponentFilter {
  courtType: string;
  busyType: string;
  court: CourtDescription;
  dayShift: string;
  hoursFrom: number;
}

@Component({
  selector: "app-home",
  styleUrls: ["./home.component.scss"],
  templateUrl: "./home.component.html",
})
export class HomeComponent {
  dataSource: CourtRecord[] = [];
  wholeDataSource: CourtRecord[] = [];

  static readonly ALL: string = "all";
  static readonly Busy: string = "busy";
  static readonly Free: string = "free";

  courtTypes: OptionValue[] = [
    { value: HomeComponent.ALL, viewValue: "Все корты" },
    { value: "Зал", viewValue: "Зал" },
    { value: "Грунт", viewValue: "Грунт" },
  ];

  days: OptionValue[] = [];

  courtNumbers: CourtDescriptionWithOptionValue[] = [];

  busyTypes: OptionValue[] = [
    { value: HomeComponent.ALL, viewValue: "Показывать все" },
    { value: HomeComponent.Busy, viewValue: "Показывать занятые" },
    { value: HomeComponent.Free, viewValue: "Показывать свободные" },
  ];

  _defaultCourt: CourtDescription = {
    type: HomeComponent.ALL,
    number: HomeComponent.ALL,
  };

  filter: HomeComponentFilter = {
    courtType: HomeComponent.ALL,
    court: this._defaultCourt,
    busyType: HomeComponent.ALL,
    dayShift: "0",
    hoursFrom: 6,
  };

  _settings: CourtSettings = null;

  fontStyleControl = new FormControl();
  fontStyle?: string;

  constructor(
    public dialog: MatDialog,
    title: Title,
    private _courtRecordService: CourtRecordService
  ) {
    title.setTitle("Теннисные корты");
    this.days = [
      { value: "-1", viewValue: "Вчера" },
      { value: "0", viewValue: "Сегодня" },
      { value: "+1", viewValue: "Завтра" },
    ];
    this.fontStyleControl.setValue("0");
    _courtRecordService.GetSettings().subscribe((data) => {
      this._settings = data;
      this.courtNumbers = [
        {
          court: this._defaultCourt,
          option: { value: HomeComponent.ALL, viewValue: "Показывать все" },
        },
      ];

      let options: CourtDescriptionWithOptionValue[] =
        this._settings.courts.map((x) => ({
          court: x,
          option: {
            value: `${x.type} ${x.number}`,
            viewValue: `${x.type} ${x.number}`,
          },
        }));

      this.courtNumbers = this.courtNumbers.concat(options);
      this.getRecords(+this.filter.dayShift);
    });
  }

  formatLabel(value: number) {
    var strValue = value.toString();

    if (strValue.length === 1) {
      strValue = `0${strValue}`;
    }

    return ` ${strValue}:00 `;
  }

  courtTypeChanged() {
    this.courtNumbers = [];
    this.courtNumbers.push({
      court: this._defaultCourt,
      option: { value: HomeComponent.ALL, viewValue: "Показывать все" },
    });

    let courtsToFilter = this._settings.courts;

    if (this.filter.courtType !== HomeComponent.ALL) {
      courtsToFilter = courtsToFilter.filter(
        (x) => x.type === this.filter.courtType
      );
    }

    var data: CourtDescriptionWithOptionValue[] = courtsToFilter.map((x) => ({
      court: x,
      option: {
        value: `${x.type} ${x.number}`,
        viewValue: `${x.type} ${x.number}`,
      },
    }));

    this.courtNumbers = this.courtNumbers.concat(data);

    this.filter.court = this._defaultCourt;
    this.filterChanged();
  }

  dayChanged() {
    var dayShift = +this.filter.dayShift;

    this.getRecords(dayShift);
  }

  getRecords(dayShift: number) {
    this._courtRecordService.GetRecordsByDay(dayShift).subscribe((data) => {
      this.wholeDataSource = data;
      this.dataSource = data;
      this.filterChanged();
    });
  }

  sliderChanged(data: MatSliderChange) {
    this.filter.hoursFrom = data.value;
    this.filterChanged();
  }

  filterChanged() {
    this.dataSource = this.wholeDataSource;

    if (this.filter.busyType === HomeComponent.Busy) {
      this.dataSource = this.dataSource.filter((x) => x.tenant !== null);
    } else if (this.filter.busyType === HomeComponent.Free) {
      this.dataSource = this.dataSource.filter((x) => x.tenant === null);
    }

    if (this.filter.courtType !== HomeComponent.ALL) {
      this.dataSource = this.dataSource.filter(
        (x) => x.court.type === this.filter.courtType
      );
    }

    if (
      this.filter.court.number !== HomeComponent.ALL &&
      this.filter.court.type !== HomeComponent.ALL
    ) {
      this.dataSource = this.dataSource.filter(
        (x) =>
          x.court.type == this.filter.court.type &&
          x.court.number === this.filter.court.number
      );
    }

    let timeString = this.filter.hoursFrom.toString();

    if (timeString.length === 1) {
      timeString = `0${timeString}`;
    }

    timeString += ":00:00";

    this.dataSource = this.dataSource.filter((x) => timeString <= x.time);

    console.log("filterChanged()", this.dataSource);
  }
}