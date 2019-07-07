using System.Text;

namespace TA.Domains.Constants
{
    public static class Data
    {
        //this is a salt for test 
        public static byte[] PasswordSalt = Encoding.ASCII.GetBytes("TAPasswordSalt|Generated:Sun Jul 07 2019");
        public static byte[] Salt = Encoding.ASCII.GetBytes("TASalt|Generated:Sun Jul 07 2019");
        public const long DefaultTokenExpiryPeriodInDays = 365;
        public static byte[] InitialVector = {1, 2, 4, 6, 8, 10, 12, 16, 18, 32, 44, 64, 75, 128, 156, 255};
    }
}