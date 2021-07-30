using CrocoShop.Web.Model.OwnedModels;
using System;

namespace CrocoShop.Web.Model.Entities
{
    public class CourtRecord
    {
        public string Id { get; set; }
        public DateTime Day { get; set; }
        public string TimeStart { get; set; }
        public CourtDescription Court { get; set; }
        public CourtTenant Tenant { get; set; }
    }
}