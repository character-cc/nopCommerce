using Nop.Services.Plugins;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Core;
using Nop.Services.Discounts;
using Nop.Plugin.Customer.Loyalty;
using Nop.Plugin.Customer.Loyalty.Services;
namespace Nop.Plugin.Loyalty.TierRewards;

public class LoyaltyCustomerMethod : BasePlugin, IDiscountRequirementRule
{

    #region Fields

    private readonly IWebHelper _webHelper;

    private readonly IDiscountService _discountService;

    private readonly ILoyaltyLevelService _loyaltyLevelService;

    private readonly ICustomerLoyaltyTotalService _customerLoyaltyTotalService;

    private readonly ILocalizationService _localizationService;



    #endregion


    #region Ctor

    public LoyaltyCustomerMethod(IWebHelper webHelper,
        IDiscountService discountService,
        ILoyaltyLevelService loyaltyLevelService,
        ICustomerLoyaltyTotalService customerLoyaltyTotalService,
        ILocalizationService localizationService)
    {
        _webHelper = webHelper;
        _discountService = discountService;
        _loyaltyLevelService = loyaltyLevelService;
        _customerLoyaltyTotalService = customerLoyaltyTotalService;
        _localizationService = localizationService;
    }

    #endregion


    #region Methods    
    public async Task<DiscountRequirementValidationResult> CheckRequirementAsync(DiscountRequirementValidationRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        //invalid by default
        var result = new DiscountRequirementValidationResult();
        var dicountRequirement = await _discountService.GetDiscountRequirementByIdAsync(request.DiscountRequirementId);
        var customer = request.Customer;       
        var customerLoyaltyTotal = await _customerLoyaltyTotalService.GetCustomerLoyaltyTotalByCustomerIdAsync(customer.Id);

        if (customer == null || dicountRequirement == null 
            || dicountRequirement.DiscountRequirementRuleSystemName != LoyaltyLevelDefault.SystemName 
            || customerLoyaltyTotal == null)
            return result;
        
        var allLoyaltyLevels = (await _loyaltyLevelService.GetAllLoyaltyLevelsAsync()).OrderByDescending(x => x.MinimumSpentAmount)
            .ThenByDescending(x => x.MinimumProductQuantity).ToList();
        var customerLevel = allLoyaltyLevels.FirstOrDefault(x => customerLoyaltyTotal.TotalSpentAmount  >= x.MinimumSpentAmount 
                                                      && customerLoyaltyTotal.TotalPurchasedQuantity >= x.MinimumProductQuantity);
        if (customerLevel == null) return result;

        var loyaltyLevelDicountMappings = await _loyaltyLevelService.GetLoyaltyLevelDiscountMappingsByDiscountIdAsync(dicountRequirement.DiscountId);
        foreach (var loyaltyLevelDicountMapping in loyaltyLevelDicountMappings)
        {
            var level = allLoyaltyLevels.FirstOrDefault(x => x.Id == loyaltyLevelDicountMapping.LoyaltyLevelId);
            if (loyaltyLevelDicountMapping.EnableInherit)
            {
                
                if (level != null && customerLevel.MinimumProductQuantity >= level.MinimumProductQuantity
                    && customerLevel.MinimumSpentAmount >= level.MinimumSpentAmount)
                {
                    result.IsValid = true;
                    break;
                }
            }
            else
            {
                if(customerLevel.Equals(level))
                {
                    result.IsValid = true;
                    break;
                }
            }
        }
        return result;
    }

    

    public string GetConfigurationUrl(int discountId, int? discountRequirementId)
    {
        return "/hehehhe";
    }

/// <summary>
    /// Gets a configuration page URL
    /// </summary>
    public override string GetConfigurationPageUrl()
    {
        return $"{_webHelper.GetStoreLocation()}Admin/Loyalty/Configure";
    }

    /// <summary>
    /// Install the plugin
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public override async Task InstallAsync()
    {
        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {
            ["Plugins.Customer.Loyalty.LoyaltyLevel.Title"] = "Loyalty Level",
            ["Plugins.Customer.Loyalty.LoyaltyLevel.FriendlyName"] = "Friendly Name",
            ["Plugins.Customer.Loyalty.LoyaltyLevel.SystemName"] = "System Name",
            ["Plugins.Customer.Loyalty.LoyaltyLevel.MinimumProductQuantity"] = "Minimum Product Quantity",
            ["Plugins.Customer.Loyalty.LoyaltyLevel.MinimumSpentAmount"] = "Minimum Spent Amount",
            ["Plugins.Customer.Loyalty.LoyaltyLevel.SaveBeforeEdit"] = "You need save Loyalty Level to have this fuction",
            ["Plugins.Customer.Loyalty.LoyaltyLevel.Create"] = "Create Loyalty Level",
            ["Plugins.Customer.Loyalty.LoyaltyLevel.Edit"] = "Edit Loyalty Level",
            ["Plugins.Customer.Loyalty.LoyaltyLevel.BackToList"] = "Back to Loyalty Level List",
            ["Plugins.Customer.Loyalty.LoyaltyLevelToDicount.EnableInheritId"] = "Enable Inherit",
            ["Plugins.Customer.Loyalty.LoyaltyLevelToDicount.Delete"] = "Delete Discount From Loyalty Level",
            ["Plugins.Customer.Loyalty.LoyaltyLevelToDicount.AddNew"] = "Add Discount To Loyalty Level"

        });

        await base.InstallAsync();
    }

    /// <summary>
    /// Uninstall the plugin
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public override async Task UninstallAsync()
    {
        await _localizationService.DeleteLocaleResourcesAsync("Plugins.Customer.Loyalty");

        await base.UninstallAsync();
    }
    #endregion

}
