namespace HouseManager.Web.Tests.StatisticsArea.Controller
{
    using HouseManager.Web.Areas.Statistics.Controllers;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class IncomeControllerTest
    {

        [Fact]
        public void StatisticsAreaIncomeControllerMastHaveAutorizeAtribute()
            => MyController<HouseManager.Web.Areas.Statistics.Controllers.IncomeController>
            .Instance()
            .ShouldHave()
            .Attributes(attribute => attribute
            .SpecifyingArea("Statistics")
            .RestrictingForAuthorizedRequests());
    }
}
