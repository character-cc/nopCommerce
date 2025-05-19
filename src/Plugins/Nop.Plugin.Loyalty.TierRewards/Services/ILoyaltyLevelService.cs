using Nop.Core;
using Nop.Core.Domain.Discounts;
using Nop.Plugin.Customer.Loyalty.Domain;
using Nop.Plugin.Customer.Loyalty.Models;

namespace Nop.Plugin.Customer.Loyalty.Services;
public interface ILoyaltyLevelService
{
    Task<IList<LoyaltyLevel>> GetAllLoyaltyLevelsAsync();

    Task<IDictionary<int, LoyaltyLevel>> GetAllLoyaltyLevelDictionaryAsync();

    Task<LoyaltyLevel> GetLoyaltyLevelByIdAsync(int id);

    Task InsertLoyaltyLevelAsync(LoyaltyLevel loyaltyLevel);


    Task UpdateLoyaltyLevelAsync(LoyaltyLevel loyaltyLevel);

    Task DeleteLoyaltyLevelAsync(LoyaltyLevel loyaltyLevel);

    Task<IPagedList<LoyaltyLevelDiscountMapping>> GetLoyaltyLevelDiscountMappingByLoyaltyLevelIdAsync(int loyaltyLevelId, int pageIndex = 0, int pageSize = int.MaxValue);

    Task<IPagedList<Discount>> SearchDiscountsNotMappingToLoyaltyLevelByLoyaltyLevelIdAsync(int loyaltyLevelId, int pageIndex = 0, int pageSize = int.MaxValue , string name = null);

    Task<IList<int>> GetMappedDiscountIds(int loyaltyLevelId, IList<int> discountIds);

    Task InsertLoyaltyLevelDiscountMappingsAsync(IList<LoyaltyLevelDiscountMapping> loyaltyLevelDiscountMappings);

    Task DeleteLoyaltyLevelDiscountMappingsAsync(IList<LoyaltyLevelDiscountMapping> loyaltyLevelDiscountMappings);

    Task<IPagedList<Discount>> SearchDiscountsMappingToLoyaltyLevelByLoyaltyLevelIdAsync(int loyaltyLevelId, int pageIndex = 0, int pageSize = int.MaxValue, string name = null);

    Task InsertLoyaltyLevelToDiscountRequirementAsync(IList<LoyaltyLevelDiscountMapping> loyaltyLevelDiscountMappings);

    Task DeleteLoyaltyLevelToDiscountRequirementAsync(IList<LoyaltyLevelDiscountMapping> loyaltyLevelDiscountMappings);

    Task<IList<LoyaltyLevelDiscountMapping>> GetLoyaltyLevelDiscountMappingsByDiscountIdAsync(int discountId);

}
