using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom
{
    public class Env
    {
        public static string WebName = "Havecome";
        public static string WebVersion = "1.0";

        /* Roles */
        public static string UserRole = "DefaultUser";
        public static string ModerRole = "Moderator";
        public static string AdminRole = "Administrator";

        /* Gmail */
        public static string Gmail = "targetweb.commerce@gmail.com";
        public static string GmailPassword = "Helloworld123";

        public static string QuestionGmail = "tracelighters@gmail.com";
        public static string QuestionPassword = "cb16da90cb16da90";
        

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

        /* Premium Price */

        public static int PremiumPrice = 199;

        /* Target coins buy */

        public static string LitePackage = "Lite";
        public static int LitePackageCoin = 40;
        public static string MiddlePackage = "Middle";
        public static int MiddlePackageCoin = 60;
        public static string SuperPackage = "Super";
        public static int SuperPackageCoin = 360;
        public static string RichPackage = "Rich";
        public static int RichPackageCoin = 800;

        /* Friend Status */

        public static string Invite = "Invite";
        public static string Friend = "Friend";
        public static string Subscribe = "Subscribe";
        public static string Blacklist = "Blacklist";

        public static int VerifySubscribe = 50;

        /* Case Type */

        public static string DefaultCase = "Default";
        public static string PremiumCase = "Premium";

        /* Name UI */

        public static string RainbowNone = "None";
        public static string RainbowDropped = "Rainbow";
        public static string RainbowOn = "Rainbowon";
        public static string RainbowOff = "Rainbowoff";

        /* Max messages room */
        public static long MAX_MESSAGES = 50;

        /* Case Price */

        public static int SilverCasePrice = 40;
        public static int PremiumCasePrice = 60;

        /* Coin prize cases*/

        public static int SilverLvl1PrizeCoin = 20;
        public static int SilverLvl2PrizeCoin = 30;
        public static int SilverLvl3PrizeCoin = 40;
        public static int SilverLvl4PrizeCoin = 50;
        public static int SilverJackpotPrizeCoin = 120;
        public static int SilverRangeGamePrice = 100;

        public static int PremiumLvl1PrizeCoin = 40;
        public static int PremiumLvl2PrizeCoin = 50;
        public static int PremiumLvl3PrizeCoin = 60;
        public static int PremiumLvl4PrizeCoin = 70;
        public static int PremiumRangeGamePrice = 100;

        /* Admin managements */

        public static string DeletePersonalAccountPassword = "delete--rootwebpersonal--acc";
        public static string ToggleAdminPassword = "toggle--rootwebtoggleadmin--acc";
        public static string BannedPassword = "block--rootbannedwebpersonal--acc";

        /* Avatar Grid */

        public static string GridNone = "None";

        public static string GridMolecular = "Molecular";
        public static string GridMolecularUrl = @"https://cdn.akamai.steamstatic.com/steamcommunity/public/images/items/803050/a013f1ab4e5fb0f8b49ef9f4cde7a5bf65bbecef.png";
        
        public static string GridGhost = "Ghost";
        public static string GridGhostUrl = "https://cdn.akamai.steamstatic.com/steamcommunity/public/images/items/322330/441c842d0a004472197b448c5dc26e4ab43afe37.png";

        public static string GridBlick = "Blick";
        public static string GridBlickUrl = "https://cdn.akamai.steamstatic.com/steamcommunity/public/images/items/604780/c3c0b1a25a8c499f9430914d1ccf2d15f0aa74b0.png";

        public static string GridSlime = "Slime";
        public static string GridSlimeUrl = "https://cdn.akamai.steamstatic.com/steamcommunity/public/images/items/382560/a2af18d987a14aba583c3b06deebd7ca8349c540.png";

        public static string GridCogti = "Cogti";
        public static string GridCogtiUrl = "https://cdn.akamai.steamstatic.com/steamcommunity/public/images/items/601840/f2ddd1b708f2091e3d807e82ac762edd205300a9.png";

        public static string GridShark = "Shark";
        public static string GridSharkUrl = "https://cdn.akamai.steamstatic.com/steamcommunity/public/images/items/629820/b721b2611c9de1693f1e817b99c2ad87ee480776.png";
        
        public static string GridScan = "Scan";
        public static string GridScanUrl = "https://cdn.akamai.steamstatic.com/steamcommunity/public/images/items/855630/bf835f556a3ba4f778ffd2550e592de4a0cefe27.png";

        public static string GridHeart = "Heart";
        public static string GridHeartUrl = "https://cdn.akamai.steamstatic.com/steamcommunity/public/images/items/527230/18ec71cb7da1182d32164890bac8411a09e26e46.png";

        public static string GridExclusive = "Exclusive";
        public static string GridExclusiveUrl = "https://cdn.akamai.steamstatic.com/steamcommunity/public/images/items/788100/63abe72f9ea57defcf13b125cb91dcf692bf7b6e.png";

        /* Cryptodata */

        public static int FrequencyGenerateCryptRate = 3;
        public static int CRYPTRATE_LIMIT = 100;

        public static int LowRangeCryptoGenerated = 5;
        public static int UpperRangeCryptoGenerated = 9;

        /* Loading limits */

        public static int GAME_LOADING_LIMIT = 9;
        public static int NEWS_LOADING_LIMIT = 10;
        public static int PROFILE_NEWS_LOADING_LIMIT = 10;
        public static int FRIENDPEOPLE_LOADING_LIMIT = 20;
        public static int IMAGE_LOADING_LIMIT = 8;
        public static int MANAGEPANEL_PEOPLES_LOADING_LIMIT = 20;
        public static int MANAGEPANEL_NEWS_LOADING_LIMIT = 20;
        public static int MANAGEPANEL_GAMES_LOADING_LIMIT = 9;

        /* Others */

        public static string AllFeeds = "All";
        public static string Post = "textpost";
        public static string Updates = "updates";
        public static string ApiFeeds = "apifeeds";

        public static string AllPost = "All";
        public static string SharedPost = "sharedpost";
    }
}
