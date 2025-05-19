using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;


namespace Nop.Plugin.Customer.Loyalty.Models;
public record AddOrDeleteDiscountToLoyaltyLevelSearchModel : BaseSearchModel 
{
    public int LoyaltyLevelId { get; set; }

    [NopResourceDisplayName("Admin.Promotions.Discounts.Fields.Name")]
    public string SearchName { get; set; }
}
