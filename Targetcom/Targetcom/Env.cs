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

        /*User Status*/
        public static string Loose = "Loose!";
        public static string Loves = "In love with";
        public static string Married = "Married with";

        /* Privacy */

        public static string PrivateProfile = "Private";
        public static string PublicProfile = "Public";

        public static string DefaultImageUrl = "https://icon-library.com/images/unknown-person-icon/unknown-person-icon-4.jpg";
    }
}
