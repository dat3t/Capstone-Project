using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneVietnam.BLL
{
    public class Constant
    {
        public static string IconTypeGender = "gender";
        public static string IconTypeSos = "sos";
        public static string IconTypePost = "postType";

        public enum PostTypEnum
        {
            ViecLam = 4,
            NhaO = 5,
            XachTay = 6,
            MuaBan = 7,
            ChoDo = 8,
            LoaiKhac = 9
        }

    }
}