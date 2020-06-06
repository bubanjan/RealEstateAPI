using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateWebAPI.Models
{
    public class Estate
    {
        [Key]
        public int EstateID { get; set; }

        public string Type { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public int Size { get; set; }
        public int Price { get; set; }
        //test comment
    }
}
