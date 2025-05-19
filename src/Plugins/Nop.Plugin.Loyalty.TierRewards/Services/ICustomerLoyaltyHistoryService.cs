using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Customer.Loyalty.Domain;

namespace Nop.Plugin.Customer.Loyalty.Services;
public interface ICustomerLoyaltyHistoryService
{
    Task<CustomerLoyaltyHistory> GetCustomerLoyaltyHistoryByOrderIdAsync(int orderId);

    Task InsertCustomerLoyaltyHistoryAsync(CustomerLoyaltyHistory customerLoyaltyHistory);

    Task UpdateCustomerLoyaltyHistoryAsync(CustomerLoyaltyHistory customerLoyaltyHistory);
}
