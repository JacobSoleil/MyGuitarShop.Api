using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Common.DTOs
{
    public class CustomerDto
    {
        public int? CustomerID { get; set; }

        [MaxLength(255)]
        public required string EmailAddress { get; set; }

        [MaxLength(60)]
        public required string Password { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public int? ShippingAddressID { get; set; }

        public int? BillingAddressID { get; set; }
    }
}
