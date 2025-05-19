using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Customer.Loyalty.Domain;

namespace Nop.Plugin.Customer.Loyalty.Services;
public interface ICustomerLoyaltyTotalService
{
    Task UpdateQuantityAndSpentTotalByCustomerIdAsync(int customerId, decimal amountDelta, int quanityDelta);

    Task<CustomerLoyaltyTotal> GetCustomerLoyaltyTotalByCustomerIdAsync(int customerId);
}
