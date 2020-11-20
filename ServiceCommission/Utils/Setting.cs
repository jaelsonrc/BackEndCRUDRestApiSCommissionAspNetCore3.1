

namespace ServiceCommission.Utils
{
    public static class Setting
    {
        public static string DefaultEmail { get; private set; }
        public static string PasswordEmail { get; private set; }
        public static string DbConn { get; private set; }
        public static string JwtKey { get; private set; }
        public static int TimeJwt { get; private set; }

        public static void SetDatas(string defaulEmail, string passwordEmail, string dbConn, string jwtKey, int timeJwt = 36)
        {
            DefaultEmail = defaulEmail;
            PasswordEmail = passwordEmail;
            DbConn = dbConn;
            JwtKey = jwtKey;
            TimeJwt = timeJwt;
        }

    }
}
