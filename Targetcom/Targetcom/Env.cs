using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom
{
    public class Env
    {
        public static string WebName = "Target";

        /* Roles */
        public static string UserRole = "DefaultUser";
        public static string ModerRole = "Moderator";
        public static string AdminRole = "Administrator";

        /* Gmail */
        public static string Gmail = "targetweb.commerce@gmail.com";
        public static string GmailPassword = "Helloworld123";

        /* MailJet */
        public static string MailJetPassword = "Helloworld123)";
        public static string MailJetApi = "2e755aca5628a4a4a869dfc6da66c74e";
        public static string MailJetSecret = "b33bd8da79f1272bf98ae8487467c4ea";

        /*Afk status*/
        public static string Online = "online";
        public static string Offline = "offline";

        /* Privacy */

        public static string PrivateProfile = "Private";
        public static string PublicProfile = "Public";

        public static string DefaultImageUrl = "https://cdn.pixabay.com/photo/2018/04/18/18/56/user-3331257_960_720.png";

        public static string GameStatusFollowed = "Followed";
        public static string GameStatusBuyed = "Buyed";

        /* Target coins buy */

        public static string LitePackage = "Lite";
        public static int LitePackageCoin = 40;
        public static string MiddlePackage = "Middle";
        public static int MiddlePackageCoin = 60;
        public static string SuperPackage = "Super";
        public static int SuperPackageCoin = 360;
        public static string RichPackage = "Rich";
        public static int RichPackageCoin = 800;
    }
}
