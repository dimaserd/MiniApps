using Croco.Core.Contract;
using Croco.Core.Contract.Models;
using CrocoShop.Web.Logic.Models;
using CrocoShop.Web.Logic.Settings;
using CrocoShop.Web.Model.Contexts;
using CrocoShop.Web.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CrocoShop.Web.Logic.Services
{
    public class CourtRecordService
    {
        CourtRecordDbContext Db { get; }
        ISettingsFactory SettingsFactory { get; }

        private static readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

        public CourtRecordService(CourtRecordDbContext db, ISettingsFactory settingsFactory)
        {
            Db = db;
            SettingsFactory = settingsFactory;
        }

        public Task<CourtRecordModel[]> Get(DateTime day)
        {
            return ExecuteLocked(async () =>
            {
                day = day.Date;

                var records = await GetByDay(day);

                if (records.Length == 0)
                {
                    await AddRecordsForDay(day);
                    records = await GetByDay(day);
                }

                return records;
            });
        }

        public async Task<BaseApiResponse> Update(UpdateRecordModel model)
        {
            var record = await Db.CourtRecords.FirstOrDefaultAsync(x => x.Id == model.Id);

            if(record == null)
            {
                return new BaseApiResponse(false, "Запись не найдена по указанному идентифкатору");
            }

            record.Tenant = model.Tenant;

            Db.CourtRecords.Update(record);
            await Db.SaveChangesAsync();

            return new BaseApiResponse(true, "Запись обновлена");
        }

        private Task<CourtRecordModel[]> GetByDay(DateTime day)
        {
            return Db.CourtRecords
                .AsQueryable()
                .AsNoTracking()
                .Where(x => x.Day == day)
                .OrderBy(x => x.TimeStart)
                .ThenBy(x => x.Court.Type)
                .ThenBy(x => x.Court.Number)
                .Select(x => new CourtRecordModel
                {
                    Id = x.Id,
                    Time = x.TimeStart,
                    Court = x.Court,
                    Tenant = x.Tenant
                })
                .ToArrayAsync();
        }

        private async Task AddRecordsForDay(DateTime day)
        {
            var setting = await SettingsFactory.GetSettingAsync<CourtSettings>();

            var hourTimeSpans = setting.GetHourTimeSpans();

            var courtRecords = new List<CourtRecord>();

            foreach(var court in setting.Courts)
            {
                foreach(var hourTimeSpan in hourTimeSpans)
                {
                    courtRecords.Add(new CourtRecord
                    {
                        Id = $"Court={court.Type} {court.Number}; Day={day.Day}.{day.Month}.{day.Year}; TimeStart={hourTimeSpan}",
                        Court = court.Copy(),
                        Day = day,
                        Tenant = null,
                        TimeStart = hourTimeSpan.ToString(),
                    });
                }
            }

            Db.CourtRecords.AddRange(courtRecords);
            await Db.SaveChangesAsync();
        }

        private static async Task<T> ExecuteLocked<T>(Func<Task<T>> task)
        {
            await _semaphoreSlim.WaitAsync();

            T result;
            try
            {
                result = await task();
            }
            finally
            {
                _semaphoreSlim.Release();
            }

            return result;
        }

    }
}