using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneVietnam.DTL;

namespace OneVietnam.Models
{
    public class AddCountryViewModel
    {
        private Country country;

        public AddCountryViewModel(Country country)
        {
            this.country = country;
        }
        public AddCountryViewModel() { }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string CountryIcon { get; set; }
    }
}