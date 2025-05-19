using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Caching;
using Nop.Core.Domain.Customers;
using Nop.Plugin.Customer.Loyalty.Domain;
using Nop.Services.Caching;

namespace Nop.Plugin.Customer.Loyalty.Cache;
public class LoyaltyLevelCacheEventConsumerOnCustomerRoleEvent : CacheEventConsumer<CustomerRole>
{
    protected override async Task ClearCacheAsync(CustomerRole entity, EntityEventType entityEventType)
    {
        if(entityEventType == EntityEventType.Delete)
        await RemoveByPrefixAsync(NopEntityCacheDefaults<LoyaltyLevel>.AllPrefix);
    }
}
