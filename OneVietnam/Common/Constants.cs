using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace OneVietnam
{
    public static class Constants
    {
        public const int ResultMaximumNumber = 7;
        public const int DescriptionMaxLength = 200;
        public const int MessagePreviewMaxLength = 35;
        public const int TitleMaxLength = 100;
        public const int LimitedNumberDisplayUsers = 5;
        public const int LimitedNumberOfPost = 5;
        public const string IconTypeGender = "gender";
        public const string IconTypePost = "postType";
        public const string DefaultAvatarLink = "/Content/Images/Avatar_Default.jpg";
        public const string DefaultCoverLink = "/Content/Images/Cover_default.jpg";
        public const string CommentDescription = " đã bình luận bài : ";
        public const string accessTokenFacebook = "EAAW9j1nWUtoBAMdZBJTMkLD3kctB5h96LQTD3IOEzSvCRtDs3QZB0wz0SfEv0FZC6qnM3tqOSWbgt08xdTZCxC5TzH5IoM4sopyoZCmJrZCsjO7l9g0ZAbs0vTGkQVjwx1IfXkt7mflD1K4CtGzZAxQk6eOLHLlENpDlir4PybkPoQZDZD";
    }
    public static class CustomRoles
    {
        public const string Admin = "Admin";
        public const string Mod = "Mod";        
    }
}