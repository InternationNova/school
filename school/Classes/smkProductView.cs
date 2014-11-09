using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using school.Models;

namespace school.Classes
{
    public class smkProductView
    {
        public int id { get; set; }
        public smk_itens smk_itensObj { get; set; }
        public List<smkSubProdus> smkSubProdus { get; set; }
        public List<smkAccessory> smkAccessory1 { get; set; }
        public List<smkAccessory> smkAccessory2 { get; set; }
    }
}