using OneVietnam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneVietnam.DTL
{
    public class Location
    {
        public double XCoordinate { get; set; }
        public double YCoordinate { get; set; }
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