namespace HouseManager.Web.ViewModels.DueAmount
{
    public class MounthAmountViewModel
    {
        public string PropertyName { get; set; }

        public string ResidentName { get; set; }

        public int ResidentsCount { get; set; }

        public decimal RegularDueAmount { get; set; }

        public decimal NotRegularDueAmount { get; set; }
    }
}
