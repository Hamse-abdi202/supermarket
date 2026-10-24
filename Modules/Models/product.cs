using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Modules.Models
{
    public class product:BaseEntity
    {
        public string productname { get; set; }
        public int productPrice { get; set; }

        public int productQuentity { get; set; }
        
    }
}