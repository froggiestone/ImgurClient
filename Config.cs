using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgurClient
{
   public static class Config
    {
        public const string Client_ID = "14405003e83a167";
        public const string Client_Secret = "3bab802e5ba21f431e64e5ce5e8b0b490af82b3b";
        public const string Endpoint = "https://api.imgur.com/3/";
        public static string UserToken = "";
        public static bool EnableCache = true;
    }

    public static class Globals
    {
        public static object NavigatedTo;
    }
}
