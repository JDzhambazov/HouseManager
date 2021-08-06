namespace HouseManager.Web.ViewModels.Home
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using HouseManager.Web.ViewModels.Addresses;

    public class HomeViewModel
    {
        public IEnumerable<AddressViewModel> UsersAddresses { get; set; }

        public AddressViewModel CurrntAddress { get; set; }
    }
}
