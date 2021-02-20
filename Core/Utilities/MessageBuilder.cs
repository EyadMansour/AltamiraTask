using Core.Extensions;
using System;

namespace Core.Utilities
{
    public static class MessageBuilder
    {
        public static string Login = "Successfully logged in";
        public static string Logout = "Successfully logged out";
        public static string UserOrEmailExist = "User Or Email Already Exist!";
        public static string NotFound = "Not Exist";
        public static string LoginFault = "Email/Username or Password is not valid";
        public static string Successfully = "Successfully";
        public static string Success = "Success";
        public static string Fail = "Fail!";
        public static string NotEditable = "You cant't edit this record!";
        public static string CreateLimitReached = "User Create Limit Reached!";

        public static string Added(string key = "")
        {
            var msg = $"{Successfully} Added";
            if (string.IsNullOrEmpty(key))
                return msg.FirstLetterToUpper();
            return $"{key} {msg}";
        }

        public static string Deleted(string key = "")
        {
            var msg = $"{Successfully} Deleted";
            if (string.IsNullOrEmpty(key))
                return msg.FirstLetterToUpper();
            return $"{key} {msg}";
        }

        public static string Edited(string key = "")
        {
            var msg = $"{Successfully} Edited";
            if (string.IsNullOrEmpty(key))
                return msg.FirstLetterToUpper();
            return $"{key} {msg}";
        }
        public static string AlreadyExist(string key = "")
        {
            var msg = $"Already Exist!";
            if (string.IsNullOrEmpty(key))
                return msg.FirstLetterToUpper();
            return $"{key} {msg}";
        }
        public static string RelatedDataExist(params string[] keys)
        {
            var keysString = String.Join(",", keys);
            var msg = $"Any data is related with this record({keysString}). First Delete Them!";

            return $"{msg}";
        }


    }
}
