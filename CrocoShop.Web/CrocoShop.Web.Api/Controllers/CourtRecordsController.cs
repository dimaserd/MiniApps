using Croco.Core.Contract;
using Croco.Core.Contract.Models;
using CrocoShop.Web.Logic.Models;
using CrocoShop.Web.Logic.Services;
using CrocoShop.Web.Logic.Settings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CrocoShop.Web.Api.Controllers
{
    [Route("api/Court")]
    public class CourtRecordsController : ControllerBase
    {
        CourtRecordService Service { get; }
        ISettingsFactory SettingsFactory { get; }

        public CourtRecordsController(CourtRecordService service, ISettingsFactory settingsFactory)
        {
            Service = service;
            SettingsFactory = settingsFactory;
        }

        [HttpGet("settings")]
        public Task<CourtSettings> GetAccountSettings()
        {
            return SettingsFactory.GetSettingAsync<CourtSettings>();
        }

        /// <summary>
        /// Показ товара
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        [HttpGet("Records/GetByDay")]
        public Task<CourtRecordModel[]> GetByDay([FromQuery] DateTime day)
            => Service.Get(day);

        /// <summary>
        /// Показ товара
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        [HttpPost("Records/Update")]
        public Task<BaseApiResponse> Update([FromBody] UpdateRecordModel model)
            => Service.Update(model);
    }
}