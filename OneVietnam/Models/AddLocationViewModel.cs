using OneVietnam.DTL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneVietnam.Models
{
    public class AddLocationViewModel
    {
        public double X { get; set; }
        public double Y { get; set; }        
        public string UserId { get; set; }
        public string PostId { get; set; }
        public int PostType { get; set; }
        public int Gender { get; set; }                        
    }
}