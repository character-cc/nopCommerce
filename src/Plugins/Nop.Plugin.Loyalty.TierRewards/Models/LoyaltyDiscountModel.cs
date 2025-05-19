using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Customer.Loyalty.Models;
public record LoyaltyDiscountModel : BaseNopEntityModel
{

    public string Name { get; set; }

    public string AdminComment { get; set; }
}
