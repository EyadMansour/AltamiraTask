namespace Core.Constants
{
    public class RegexConstants
    {
        public const string UserNameRegex = @"^[a-zA-Z0-9._]{4,32}$";
        public const string EmailRegex =
            @"(([^<>()\[\]\\.,;:\s@""]+(\.[^<>()\[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$";
        public const string PasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,100}$";
        public const string DecimalGeoRegex = @"^-?[0-9]\d*(\.\d{0,6})?$";

    }
}
