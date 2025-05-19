using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;

namespace Nop.Plugin.Customer.Loyalty.Domain;
public class LoyaltyLevelDiscountMapping : BaseEntity
{

    public int DiscountId { get; set; }

    public int LoyaltyLevelId { get; set; }

    public bool EnableInherit { get; set; } 
}
