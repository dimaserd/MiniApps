
export class CountRecordCreator {

  static GetTimes(): string[] {
    var time = "06:00";

    var result: string[] = [];
    for (let i = 0; i < 16; i++) {
      result.push(CountRecordCreator.plusTime(time, i));
    }
    return result;
  }

  static plusTime(time: string, count: number): string {
    let f1 = Number(time.split(':')[0]);
    f1 += count;

    return CountRecordCreator.zeroPad(f1, 2) + ":00";
  }

  static zeroPad(num: number, places: number): string {
    return String(num).padStart(places, '0');
  }
}

