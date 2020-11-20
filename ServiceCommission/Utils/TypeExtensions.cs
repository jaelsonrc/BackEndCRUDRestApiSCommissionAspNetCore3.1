using ServiceCommission.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCommission.Utils
{
    public static class TypeExtensions
    {
        public static string GetHash(this string value, string salt)
        {
            return SecureHelper.GerarHash(salt, value);
        }

        public static string GetHash(this User user)
        {
            return SecureHelper.GerarHash(user.Login, user.Password);
        }

        public static DateTime UnixTimestampToDateTime(this long unixTime)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTime).LocalDateTime;
        }
        public static long DateTimeToUnixTimestamp(this DateTime dateTime)
        {

            return ((DateTimeOffset)dateTime).ToUnixTimeSeconds();

        }


        public static bool IsValidEmail(this string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
