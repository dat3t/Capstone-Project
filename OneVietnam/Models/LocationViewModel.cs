using OneVietnam.DTL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneVietnam.Models
{
    public class LocationViewModel
    {
        public double x { get; set; }
        public double y { get; set; }
        public string address { get; set; }
        public string userid { get; set; }

        public LocationViewModel(Location location)
        {
            x = location.XCoordinate;
            y = location.XCoordinate;
            address = location.Address;
        }

    }
}