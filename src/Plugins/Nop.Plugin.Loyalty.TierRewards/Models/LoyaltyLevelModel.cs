using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;
using System.ComponentModel.DataAnnotations;

namespace Nop.Plugin.Customer.Loyalty.Models
{
    public record LoyaltyLevelModel : BaseNopEntityModel
    {

        
        [NopResourceDisplayName("Plugins.Customer.Loyalty.LoyaltyLevel.FriendlyName")]
        [Required(ErrorMessage = "Trường này bắt buộc.")]
        public string FriendlyName { get; set; }


        [Required(ErrorMessage = "Trường này bắt buộc.")]
        [NopResourceDisplayName("Plugins.Customer.Loyalty.LoyaltyLevel.SystemName")]
        public string SystemName { get; set; }

        [NopResourceDisplayName("Plugins.Customer.Loyalty.LoyaltyLevel.MinimumSpentAmount")]
        public int MinimumSpentAmount { get; set; }

        [NopResourceDisplayName("Plugins.Customer.Loyalty.LoyaltyLevel.MinimumProductQuantity")]
        public int MinimumProductQuantity { get; set; }


        public LoyaltyDiscountSearchModel LoyaltyDiscountSearchModel { get; set; }


        public LoyaltyLevelModel()
        {
            LoyaltyDiscountSearchModel = new LoyaltyDiscountSearchModel();
        }

    }
}
