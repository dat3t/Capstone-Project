using OneVietnam.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneVietnam.DTL
{
    public class Location
    {
        public double XCoordinate { get; set; }
        public double YCoordinate { get; set; }

        [Required(ErrorMessage = "{0} chưa được điền.")]
        [Display(Name = "Địa chỉ bài đăng")]
        public string Address { get; set; }

        public Location()
        {
            
        }

        public Location(double xCoordinate, double yCoordinate, string address)
        {
            XCoordinate = xCoordinate;
            YCoordinate = yCoordinate;
            Address = address;
        }
    }      
}