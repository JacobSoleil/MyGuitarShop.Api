using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Common.DTOs
{
    public class CategoryDto
    {
        public int? CategoryID { get; set; }

        public required string CategoryName { get; set; }
    }
}
