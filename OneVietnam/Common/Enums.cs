using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneVietnam
{
    public enum PostTypEnum
    {
        ViecLam = 4,
        NhaO = 5,
        XachTay = 6,
        MuaBan = 7,
        ChoDo = 8,
        LoaiKhac = 9
    }

    public enum MessageType
    {
        Send = 0,
        Receive = 1
    }
    public enum Gender
    {
        male = 0,
        female = 1,
        Other = 2
    }
}