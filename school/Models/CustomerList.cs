using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace school.Models
{
    public class CustomerList : List<Customer>
    {
        public string ImageUrl { get; set; }
    }
}
