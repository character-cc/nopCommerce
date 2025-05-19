using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Customer.Loyalty.Models;
using Nop.Plugin.Customer.Loyalty.Domain;
using Nop.Data;
using Nop.Core.Caching;
using Nop.Core;
using Nop.Core.Domain.Discounts;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using LinqToDB;

namespace Nop.Plugin.Customer.Loyalty.Services;
public class LoyaltyLevelService : ILoyaltyLevelService
{

    private readonly IRepository<LoyaltyLevel> _loyaltyLevelRepository;

    private readonly IStaticCacheManager _staticCacheManager;

    private readonly IRepository<LoyaltyLevelDiscountMapping> _loyaltyLevelDiscountMappingRepository;

    private readonly IRepository<Discount> _discountRepository;

    private readonly IRepository<LoyaltyLevelDiscountMapping> _loyaltyLevelDicountMappingRepository;

    private readonly IDiscountService _discountService;

    private readonly ILocalizationService _localizationService;

    private readonly IRepository<DiscountRequirement> _discountRequirementRepository;

    public LoyaltyLevelService(IRepository<LoyaltyLevel> loyaltyLevelRepository, IStaticCacheManager staticCacheManager, IRepository<LoyaltyLevelDiscountMapping> loyaltyLevelDiscountMappingRepository, IRepository<Discount> discountRepository, IRepository<LoyaltyLevelDiscountMapping> loyaltyLevelDicountMappingRepository, IDiscountService discountService, ILocalizationService localizationService, IRepository<DiscountRequirement> discountRequirementRepository)
    {
        _loyaltyLevelRepository = loyaltyLevelRepository;
        _staticCacheManager = staticCacheManager;
        _loyaltyLevelDiscountMappingRepository = loyaltyLevelDiscountMappingRepository;
        _discountRepository = discountRepository;
        _loyaltyLevelDicountMappingRepository = loyaltyLevelDicountMappingRepository;
        _discountService = discountService;
        _localizationService = localizationService;
        _discountRequirementRepository = discountRequirementRepository;
    }

    public virtual async Task<IList<LoyaltyLevel>> GetAllLoyaltyLevelsAsync()
    {
        var allLevel = await GetAllLoyaltyLevelDictionaryAsync();

        return allLevel.Values.ToList();
    }

    public virtual async Task<IDictionary<int, LoyaltyLevel>> GetAllLoyaltyLevelDictionaryAsync()
    {
        return await _staticCacheManager.GetAsync(
         _staticCacheManager.PrepareKeyForDefaultCache(NopEntityCacheDefaults<LoyaltyLevel>.AllCacheKey),
            async () => await LinqToDB.AsyncExtensions.ToDictionaryAsync(_loyaltyLevelRepository.Table, ll => ll.Id));


    }

    public virtual async Task<LoyaltyLevel> GetLoyaltyLevelByIdAsync(int id)
    {
        if (id <= 0)
            return null;
        var allLevel = await GetAllLoyaltyLevelDictionaryAsync();
        return allLevel.TryGetValue(id, out var loyaltyLevel) ? loyaltyLevel : null;
    }

    public async Task UpdateLoyaltyLevelAsync(LoyaltyLevel loyaltyLevel)
    {
        await _loyaltyLevelRepository.UpdateAsync(loyaltyLevel);
    }

    public async Task InsertLoyaltyLevelAsync(LoyaltyLevel loyaltyLevel)
    {
        await _loyaltyLevelRepository.InsertAsync(loyaltyLevel);
    }

    public async Task DeleteLoyaltyLevelAsync(LoyaltyLevel loyaltyLevel)
    {
        await _loyaltyLevelRepository.DeleteAsync(loyaltyLevel);
    }

    public async Task<IPagedList<LoyaltyLevelDiscountMapping>> GetLoyaltyLevelDiscountMappingByLoyaltyLevelIdAsync(int loyaltyLevelId, int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var query = from lldm in _loyaltyLevelDiscountMappingRepository.Table
                    where lldm.LoyaltyLevelId == loyaltyLevelId
                    select lldm;
        return await query.ToPagedListAsync(pageIndex, pageSize);
    }

    public async Task<IPagedList<Discount>> SearchDiscountsNotMappingToLoyaltyLevelByLoyaltyLevelIdAsync(int loyaltyLevelId, int pageIndex = 0, int pageSize = int.MaxValue, string name = null)
    {
        var query = _discountRepository.Table.Where(d => !_loyaltyLevelDicountMappingRepository.Table
        .Any(lldm => lldm.DiscountId == d.Id && lldm.LoyaltyLevelId == loyaltyLevelId));
        if (!string.IsNullOrEmpty(name))
            query = query.Where(d => d.Name.Contains(name));
        return await query.ToPagedListAsync(pageIndex, pageSize);
    }

    public async Task<IPagedList<Discount>> SearchDiscountsMappingToLoyaltyLevelByLoyaltyLevelIdAsync(int loyaltyLevelId, int pageIndex = 0, int pageSize = int.MaxValue, string name = null)
    {
        var query = from d in _discountRepository.Table
                    join lldm in _loyaltyLevelDicountMappingRepository.Table on d.Id equals lldm.DiscountId
                    where lldm.LoyaltyLevelId == loyaltyLevelId
                    select d;
        if (!string.IsNullOrEmpty(name))
            query = query.Where(d => d.Name.Contains(name));
        return await query.ToPagedListAsync(pageIndex, pageSize);
    }

    public async Task InsertLoyaltyLevelDiscountMappingsAsync(IList<LoyaltyLevelDiscountMapping> loyaltyLevelDiscountMappings)
    {
        if (loyaltyLevelDiscountMappings == null)
            throw new ArgumentNullException(nameof(loyaltyLevelDiscountMappings));
        await _loyaltyLevelDicountMappingRepository.InsertAsync(loyaltyLevelDiscountMappings);
    }

    public async Task DeleteLoyaltyLevelDiscountMappingsAsync(IList<LoyaltyLevelDiscountMapping> loyaltyLevelDiscountMappings)
    {
        if (loyaltyLevelDiscountMappings == null)
            throw new ArgumentNullException(nameof(loyaltyLevelDiscountMappings));
        await _loyaltyLevelDicountMappingRepository.DeleteAsync(loyaltyLevelDiscountMappings);
    }

    public async Task<IList<int>> GetMappedDiscountIds(int loyaltyLevelId, IList<int> discountIds)
    {
        var query = from lldm in _loyaltyLevelDiscountMappingRepository.Table
                    where lldm.LoyaltyLevelId == loyaltyLevelId && discountIds.Contains(lldm.DiscountId)
                    select lldm.DiscountId;
        return await query.ToListAsync();
    }

    public async Task<IList<LoyaltyLevelDiscountMapping>> GetLoyaltyLevelDiscountMappingsByDiscountIdAsync(int discountId)
    {
        var query = from lldm in _loyaltyLevelDiscountMappingRepository.Table
                    where lldm.DiscountId == discountId
                    select lldm;
        return await query.ToListAsync();
    }

    public async Task InsertLoyaltyLevelToDiscountRequirementAsync(IList<LoyaltyLevelDiscountMapping> loyaltyLevelDiscountMappings)
    {
        ArgumentNullException.ThrowIfNull(loyaltyLevelDiscountMappings);

        var requirements = new List<DiscountRequirement>();

        foreach (var loyaltyLevelDiscountMapping in loyaltyLevelDiscountMappings)
        {
            var discount = await _discountRepository.GetByIdAsync(loyaltyLevelDiscountMapping.DiscountId);
            if (discount == null)
                continue;
            var existingRequirement = await _discountRequirementRepository.Table
                    .FirstOrDefaultAsync(x => x.DiscountId == discount.Id && x.DiscountRequirementRuleSystemName.Equals(LoyaltyLevelDefault.SystemName));
            if (existingRequirement != null)
                continue;
            var defaultGroupId = (await _discountService.GetAllDiscountRequirementsAsync(discount.Id, true)).FirstOrDefault(requirement => requirement.IsGroup)?.Id ?? 0;
            if (defaultGroupId == 0)
            {
                var defaultGroup = new DiscountRequirement
                {
                    IsGroup = true,
                    DiscountId = discount.Id,
                    InteractionType = RequirementGroupInteractionType.And,
                    DiscountRequirementRuleSystemName = await _localizationService
                        .GetResourceAsync("Admin.Promotions.Discounts.Requirements.DefaultRequirementGroup")
                };
                await _discountService.InsertDiscountRequirementAsync(defaultGroup);
                defaultGroupId = defaultGroup.Id;
            }
            var requirement = new DiscountRequirement
            {
                DiscountId = discount.Id,
                DiscountRequirementRuleSystemName = LoyaltyLevelDefault.SystemName,
                InteractionType = RequirementGroupInteractionType.And,
                IsGroup = false,
                ParentId = defaultGroupId
            };
            requirements.Add(requirement);
        }
        if (requirements.Any())
            await _discountRequirementRepository.InsertAsync(requirements);

    }

    public async Task DeleteLoyaltyLevelToDiscountRequirementAsync(IList<LoyaltyLevelDiscountMapping> loyaltyLevelDiscountMappings)
    {
        ArgumentNullException.ThrowIfNull(loyaltyLevelDiscountMappings);

        var discountIdsInMapping = loyaltyLevelDiscountMappings
            .Select(x => x.DiscountId)
            .Distinct()
            .ToList();
        var existingMappedDiscountIds = await _loyaltyLevelDiscountMappingRepository.Table
            .Where(x => discountIdsInMapping.Contains(x.DiscountId))
            .Select(x => x.DiscountId)
            .Distinct()
            .ToListAsync();
        var discountIdsToDelete = discountIdsInMapping
            .Except(existingMappedDiscountIds)
            .ToList();
        if (!discountIdsToDelete.Any())
            return;
        await _discountRequirementRepository.Table
            .Where(x =>
                discountIdsToDelete.Contains(x.DiscountId) &&
                x.DiscountRequirementRuleSystemName == LoyaltyLevelDefault.SystemName)
            .DeleteAsync();
    }

}
