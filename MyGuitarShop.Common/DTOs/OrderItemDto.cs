using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Common.DTOs
{
    public class OrderItemDto
    {
        public int? ItemID { get; set; }

        public int? OrderID { get; set; }

        public int? ProductID { get; set; }

        public required decimal ItemPrice { get; set; }

        public required decimal DiscountAmount { get; set; }

        public required int Quantity { get; set; }
    }
}
