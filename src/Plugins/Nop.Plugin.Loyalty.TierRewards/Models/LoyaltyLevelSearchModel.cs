
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Customer.Loyalty.Models;
public record LoyaltyLevelSearchModel : BaseSearchModel
{
    [NopResourceDisplayName("Plugins.Customer.Loyalty.LoyaltyLevel.FriendlyName")]

    public string SearchFriendlyName { get; set; }

    [NopResourceDisplayName("Plugins.Customer.Loyalty.LoyaltyLevel.SystemName")]
   public string SearchSystemName { get; set; }

}
