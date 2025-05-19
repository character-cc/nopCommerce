using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Customer.Loyalty.Models;
public record AddOrDeleteDiscountToLoyaltyLevelModel : BaseNopModel
{
    public AddOrDeleteDiscountToLoyaltyLevelModel()
    {
        SelectedDiscountIds = new List<int>();
        EnableInheritIds = new List<int>();
    }
    public int LoyaltyLevelId { get; set; }
   
    public IList<int> SelectedDiscountIds { get; set; }

    public IList<int> EnableInheritIds { get; set; } 
}
