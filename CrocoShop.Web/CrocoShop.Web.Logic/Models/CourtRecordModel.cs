using CrocoShop.Web.Model.OwnedModels;

namespace CrocoShop.Web.Logic.Models
{
    public class CourtRecordModel
    {
        public string Id { get; set; }
        public string Time { get; set; }
        public CourtTenant Tenant { get; set; }
        public CourtDescription Court { get; set; }
    }
}