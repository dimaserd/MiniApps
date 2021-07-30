namespace CrocoShop.Web.Model.OwnedModels
{
    public class CourtDescription
    {
        public string Type { get; set; }
        public string Number { get; set; }

        public CourtDescription Copy()
        {
            return new CourtDescription
            {
                Type = Type,
                Number = Number
            };
        }
    }
}