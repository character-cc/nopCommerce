using AutoMapper;
using Nop.Core.Domain.Discounts;
using Nop.Core.Infrastructure.Mapper;
using Nop.Plugin.Customer.Loyalty.Domain;
using Nop.Plugin.Customer.Loyalty.Models;


namespace Nop.Plugin.Widgets.FacebookPixel.Infrastructure.Mapper;

/// <summary>
/// Represents AutoMapper configuration for plugin models
/// </summary>
public class MapperConfiguration : Profile, IOrderedMapperProfile
{
    #region Ctor

    public MapperConfiguration()
    {
        CreateMap<LoyaltyLevel, LoyaltyLevelModel>();
        CreateMap<LoyaltyLevelModel, LoyaltyLevel>();
        CreateMap<LoyaltyDiscountModel, Discount>();
        CreateMap<Discount, LoyaltyDiscountModel>();
    }

    #endregion

    #region Properties

    /// <summary>
    /// Order of this mapper implementation
    /// </summary>
    public int Order => 1;

    #endregion
}