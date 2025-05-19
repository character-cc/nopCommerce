using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;

namespace Nop.Plugin.Customer.Loyalty.Domain;
public class LoyaltyLevel : BaseEntity
{
    public string FriendlyName { get; set; } 

    public string SystemName { get; set; }
    public int MinimumSpentAmount { get; set; }
    public int MinimumProductQuantity { get; set; }



}
