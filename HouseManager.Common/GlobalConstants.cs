namespace HouseManager.Common
{
    using System.Globalization;

    public static class GlobalConstants
    {
        public const string SystemName = "HouseManager";

        public const string AdministratorRoleName = "Administrator";

        public const NumberStyles decimalStyle = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.Number;

        public const string AddressCookieName = "CurrentAddressId";

        public const int MaxRowPerPage = 10;
    }
}
