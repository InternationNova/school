using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using school.Models;
using System.Data.SqlClient;

namespace school.Classes
{
    public class escolasIndex
    {
        public int id { get; set; }
        public escolas escolas { get; set; }
        public string estados_nome { get; set; }
    }
}