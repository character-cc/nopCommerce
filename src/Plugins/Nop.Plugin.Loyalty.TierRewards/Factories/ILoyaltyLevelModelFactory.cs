using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Customer.Loyalty.Domain;
using Nop.Plugin.Customer.Loyalty.Models;

namespace Nop.Plugin.Customer.Loyalty.Factories;
public interface ILoyaltyLevelModelFactory
{
      Task<LoyaltyLevelListModel> PrepareLoyaltyLevelListModelAsync(LoyaltyLevelSearchModel searchModel);

    Task<LoyaltyLevelModel> PrepareLoyaltyLevelModelAsync(LoyaltyLevelModel model = null, LoyaltyLevel loyaltyLevel = null);

    Task PrepareLoyaltyDiscountSearchModelAsync(LoyaltyDiscountSearchModel loyaltyDiscountSearchModel, LoyaltyLevel loyaltyLevel);

    Task<LoyaltyDiscountListModel> PrepareLoyaltyDicountListModelAsync(LoyaltyDiscountSearchModel searchModel, LoyaltyLevel loyaltyLevel);

    Task<AddOrDeleteDiscountToLoyaltyLevelSearchModel> PrepareAddDiscountToLoyaltyLevelSearchModelAsync(AddOrDeleteDiscountToLoyaltyLevelSearchModel searchModel = null, LoyaltyLevel loyaltyLevel = null);

    Task<AddOrDeleteDiscountToLoyaltyLevelListModel> PrepareAddDiscountToLoyaltyLevelListModelAsync(AddOrDeleteDiscountToLoyaltyLevelSearchModel searchModel, LoyaltyLevel loyaltyLevel);

    Task<AddOrDeleteDiscountToLoyaltyLevelListModel> PrepareDeleteDiscountToLoyaltyLevelListModelAsync(AddOrDeleteDiscountToLoyaltyLevelSearchModel searchModel, LoyaltyLevel loyaltyLevel);

}
