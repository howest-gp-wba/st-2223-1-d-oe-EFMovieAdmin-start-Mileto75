using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wba.Oefening.RateAMovie.Web.Models
{
    public class PersonCheckbox
    {
        [HiddenInput]
        public long Id { get; set; }
        
        public string Name { get; set; }
        public bool Selected { get; set; }
    }
}
