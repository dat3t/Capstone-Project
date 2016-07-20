using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneVietnam.BLL
{
    public class Constant
    {
        public const string IconTypeGender = "gender";
        public const string IconTypeSos = "sos";
        public const string IconTypePost = "postType";

        public enum PostTypEnum
        {
            ViecLam = 4,
            NhaO = 5,
            XachTay = 6,
            MuaBan = 7,
            ChoDo = 8,
            LoaiKhac = 9
        }

        public enum Gender
        {
            Female=0,
            Male = 1,
            Other = 3
        }

    }
}