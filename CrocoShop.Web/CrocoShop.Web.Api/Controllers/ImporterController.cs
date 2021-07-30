using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Zoo.DataImporter;
using Zoo.DataImporter.Models;

namespace CrocoShop.Web.Api.Controllers
{
    [Route("Api/Importer")]
    public class ImporterController : ControllerBase
    {
        Importer Importer { get; }

        public ImporterController(Importer importer)
        {
            Importer = importer;
        }

        [HttpPost("InitApp")]
        public Task<DataImportResult> InitApp()
        {
            return Importer.InitApp();
        }
    }
}