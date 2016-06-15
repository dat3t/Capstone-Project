using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
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
        [Required]
        [Display(Name = "Tên quốc gia")]
        public string CountryName { get; set; }

        [Required]
        [Display(Name = "Mã quốc gia")]
        public string CountryCode { get; set; }

        [Required]
        [Display(Name = "Quốc kỳ")]
        public byte[] CountryIcon { get; set; }
    }
}