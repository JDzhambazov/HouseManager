namespace HouseManager.Services.Data.Models
{
    public class AmountListServiceModel
    {
        public int Id { get; set; }

        public string PropertyName { get; set; }

        public string ResidentName { get; set; }

        public int ResidentsCount { get; set; }

        public decimal RegularDueAmount { get; set; }

        public decimal NotRegularDueAmount { get; set; }
    }
}
