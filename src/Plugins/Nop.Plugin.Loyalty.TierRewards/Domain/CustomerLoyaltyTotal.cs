using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;

namespace Nop.Plugin.Customer.Loyalty.Domain;
public class CustomerLoyaltyTotal : BaseEntity
{
    public int CustomerId { get; set; }

    public decimal TotalSpentAmount { get; set; }

    public int TotalPurchasedQuantity { get; set; }

    public DateTime UpdatedOnUtc { get; set; }
}
