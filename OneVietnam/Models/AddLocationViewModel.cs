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
        public int gender { get; set; }
        public Dictionary<Location, string> infoForInitMap { get; set; }
        public List<Post> Posts { get; set; } 
        public AddLocationViewModel()
        {

        }
        public AddLocationViewModel(Location location)
        {
            x = location.XCoordinate;
            y = location.YCoordinate;
            address = location.Address;
        }

        public AddLocationViewModel(Dictionary<Location,string> dic)
        {
            infoForInitMap = dic;
        }

        public AddLocationViewModel(Location location, string userid,int gender, List<Post> Posts)
        {
            x = location.XCoordinate;
            y = location.YCoordinate;
            address = location.Address;
            this.gender = gender;
            this.userid = userid;
            this.Posts = Posts;

        }
    }
}