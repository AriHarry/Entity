using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoginInMVC4WithEF.Models
{
    public class ClsProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
         
        public string Name { get; set; }
        
        public string ModelPlace { get; set; }
         
        public string Rate { get; set; }

        public string Description { get; set; }

        public List<Department> lstProduct = new List<Department>();
    }
    public class Department
    {
        public int ProductId { get; set; }
        public string Description { get; set; }
    }
}