using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Data;
using Nop.Plugin.Customer.Loyalty.Domain;
using Nop.Plugin.Customer.Loyalty.Models;
using Nop.Plugin.Customer.Loyalty.Services;
using Nop.Services.Customers;
using Nop.Services.Discounts;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Customer.Loyalty.Factories;
public class LoyaltyLevelModelFactory : ILoyaltyLevelModelFactory
{

    #region Field

    private readonly ILoyaltyLevelService _loyaltyLevelService;
    private readonly ICustomerService _customerService;
    private readonly IDiscountService _discountService;

    #endregion

    #region ctor
    public LoyaltyLevelModelFactory(ILoyaltyLevelService loyaltyLevelService, ICustomerService customerService, IDiscountService discountService)
    {
        _loyaltyLevelService = loyaltyLevelService;
        _customerService = customerService;
        _discountService = discountService;
    }

    #endregion

    #region method
    public async Task<LoyaltyLevelListModel> PrepareLoyaltyLevelListModelAsync(LoyaltyLevelSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        var loyaltyLevels = (await _loyaltyLevelService.GetAllLoyaltyLevelsAsync()).ToPagedList(searchModel);
    
        var model = await new LoyaltyLevelListModel().PrepareToGridAsync(searchModel, loyaltyLevels, () =>
        {
            return loyaltyLevels.SelectAwait(async level =>
            {
                return level.ToModel<LoyaltyLevelModel>();
               
            });
        });
        return model;
    }

    public virtual async Task<LoyaltyLevelModel> PrepareLoyaltyLevelModelAsync(LoyaltyLevelModel model = null, LoyaltyLevel loyaltyLevel = null)
    {
        model ??= new LoyaltyLevelModel(); 
        if (model.LoyaltyDiscountSearchModel == null)
            model.LoyaltyDiscountSearchModel = new LoyaltyDiscountSearchModel();
        if (loyaltyLevel != null)
        {
            model = loyaltyLevel.ToModel<LoyaltyLevelModel>();  
            await PrepareLoyaltyDiscountSearchModelAsync(model.LoyaltyDiscountSearchModel, loyaltyLevel);
        }
       
     
        return model;
    }

    public virtual async Task PrepareLoyaltyDiscountSearchModelAsync(LoyaltyDiscountSearchModel loyaltyDiscountSearchModel, LoyaltyLevel loyaltyLevel)
    {
        ArgumentNullException.ThrowIfNull(loyaltyLevel);
        ArgumentNullException.ThrowIfNull(loyaltyDiscountSearchModel);
        loyaltyDiscountSearchModel.LoyaltyLevelId = loyaltyLevel.Id;
        loyaltyDiscountSearchModel.SetGridPageSize();
    }

    public virtual async Task<LoyaltyDiscountListModel> PrepareLoyaltyDicountListModelAsync(LoyaltyDiscountSearchModel searchModel, LoyaltyLevel loyaltyLevel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);
        ArgumentNullException.ThrowIfNull(loyaltyLevel);

        var loyaltyLevelDiscountMappings = await _loyaltyLevelService.GetLoyaltyLevelDiscountMappingByLoyaltyLevelIdAsync(loyaltyLevel.Id);
        var model = await new LoyaltyDiscountListModel().PrepareToGridAsync(searchModel, loyaltyLevelDiscountMappings, () =>
        {
            return loyaltyLevelDiscountMappings.SelectAwait(async loyaltyLevelDiscountMapping =>
            {
                var discount = await _discountService.GetDiscountByIdAsync(loyaltyLevelDiscountMapping.DiscountId);
                return discount.ToModel<LoyaltyDiscountModel>();
            });
        });
        return model;

    }

    public virtual async Task<AddOrDeleteDiscountToLoyaltyLevelSearchModel> PrepareAddDiscountToLoyaltyLevelSearchModelAsync(AddOrDeleteDiscountToLoyaltyLevelSearchModel searchModel = null, LoyaltyLevel loyaltyLevel = null)
    {
        searchModel ??= new AddOrDeleteDiscountToLoyaltyLevelSearchModel();
        searchModel.SetGridPageSize();
        searchModel.LoyaltyLevelId = loyaltyLevel?.Id ?? 0;
        return searchModel;
    }

    public virtual async Task<AddOrDeleteDiscountToLoyaltyLevelListModel> PrepareAddDiscountToLoyaltyLevelListModelAsync(AddOrDeleteDiscountToLoyaltyLevelSearchModel searchModel, LoyaltyLevel loyaltyLevel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);
        ArgumentNullException.ThrowIfNull(loyaltyLevel);
        var discounts = await _loyaltyLevelService.SearchDiscountsNotMappingToLoyaltyLevelByLoyaltyLevelIdAsync(loyaltyLevel.Id, searchModel.Page - 1, searchModel.PageSize, searchModel.SearchName);
        var model = await new AddOrDeleteDiscountToLoyaltyLevelListModel().PrepareToGridAsync(searchModel, discounts, () =>
        {
            return discounts.SelectAwait(async discount =>
            {
                var discountModel = discount.ToModel<LoyaltyDiscountModel>();
                return discountModel;
            });
        });

        return model;
    }

    public virtual async Task<AddOrDeleteDiscountToLoyaltyLevelListModel> PrepareDeleteDiscountToLoyaltyLevelListModelAsync(AddOrDeleteDiscountToLoyaltyLevelSearchModel searchModel, LoyaltyLevel loyaltyLevel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);
        ArgumentNullException.ThrowIfNull(loyaltyLevel);
        var discounts = await _loyaltyLevelService.SearchDiscountsMappingToLoyaltyLevelByLoyaltyLevelIdAsync(loyaltyLevel.Id, searchModel.Page - 1, searchModel.PageSize, searchModel.SearchName);
        var model = await new AddOrDeleteDiscountToLoyaltyLevelListModel().PrepareToGridAsync(searchModel, discounts, () =>
        {
            return discounts.SelectAwait(async discount =>
            {
                var discountModel = discount.ToModel<LoyaltyDiscountModel>();
                return discountModel;
            });
        });

        return model;
    }

}

#endregion