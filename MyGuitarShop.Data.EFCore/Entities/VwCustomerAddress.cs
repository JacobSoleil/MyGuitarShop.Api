using System;
using System.Collections.Generic;

namespace MyGuitarShop.Data.EFCore.Entities;

public partial class VwCustomerAddress
{
    public int CustomerId { get; set; }

    public string EmailAddress { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string BillLine1 { get; set; } = null!;

    public string? BillLine2 { get; set; }

    public string BillCity { get; set; } = null!;

    public string BillState { get; set; } = null!;

    public string BillZip { get; set; } = null!;

    public string ShipLine1 { get; set; } = null!;

    public string? ShipLine2 { get; set; }

    public string ShipCity { get; set; } = null!;

    public string ShipState { get; set; } = null!;

    public string ShipZip { get; set; } = null!;
}
