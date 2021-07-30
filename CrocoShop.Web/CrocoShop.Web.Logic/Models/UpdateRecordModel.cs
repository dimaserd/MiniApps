using CrocoShop.Web.Model.OwnedModels;

namespace CrocoShop.Web.Logic.Models
{
    public class UpdateRecordModel
    {
        public string Id { get; set; }
        public CourtTenant Tenant { get; set; }
    }
}