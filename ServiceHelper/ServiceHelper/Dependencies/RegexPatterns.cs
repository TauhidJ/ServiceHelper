namespace ServiceHelper.Dependencies
{
    public class RegexPatterns
    {
        public const string MobileNumber = "(^[\\+1-9]\\d{9,15}$)";

        public const string MobileNumberWithCountryCode = "^\\+[1-9]\\d{10,14}$";

        public const string EmailAddress = "^(([^<>()[\\]\\\\.,;:\\s@\\\"]+(\\.[^<>()[\\]\\\\.,;:\\s@\\\"]+)*)|(\\\".+\\\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$";

        public const string NumericZeroId = "^\\d+(:\\d+)*$";

        public const string Zrid = "^[A-Z]{2}(:\\d+)*$";

        public const string Zeid = "^[a-z]+(:[a-z]+)*(:\\d+)+$";

        public const string UPIVPA = "^[\\w.-]+@[\\w.-]+$";
    }
}
