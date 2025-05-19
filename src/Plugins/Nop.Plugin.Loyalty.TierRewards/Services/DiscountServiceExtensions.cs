using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Discounts;
using Nop.Data;
using Nop.Services.Discounts;

namespace Nop.Plugin.Customer.Loyalty.Services;
public static class DiscountServiceExtensions 
{
    public static async Task<IList<Discount>> GetDiscountsByIdsAsync(
        this IDiscountService discountService,
        IRepository<Discount> discountRepository,
        IList<int> ids)
    {
        if (ids == null || !ids.Any())
            return new List<Discount>();

        return await discountRepository.Table
            .Where(d => ids.Contains(d.Id))
            .ToListAsync();
    }
}
