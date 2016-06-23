using OneVietnam.DTL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneVietnam.Models
{
    public class AddLocationViewModel
    {
        public double x { get; set; }
        public double y { get; set; }
        public string address { get; set; }
        public string userid { get; set; }

        public AddLocationViewModel()
        {

        }
        public AddLocationViewModel(Location location)
        {
            x = location.XCoordinate;
            y = location.YCoordinate;
            address = location.Address;
        }

    }
}