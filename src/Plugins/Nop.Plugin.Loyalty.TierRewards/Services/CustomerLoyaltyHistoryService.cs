using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Data;
using Nop.Plugin.Customer.Loyalty.Domain;

namespace Nop.Plugin.Customer.Loyalty.Services;
public class CustomerLoyaltyHistoryService : ICustomerLoyaltyHistoryService
{
    private readonly IRepository<CustomerLoyaltyHistory> _customerLoyaltyHistoryRepository;

    public CustomerLoyaltyHistoryService(IRepository<CustomerLoyaltyHistory> customerLoyaltyHistoryRepository)
    {
        _customerLoyaltyHistoryRepository = customerLoyaltyHistoryRepository;
    }
    public async Task InsertCustomerLoyaltyHistoryAsync(CustomerLoyaltyHistory customerLoyaltyHistory)
    {
        if (customerLoyaltyHistory == null)
            throw new ArgumentNullException(nameof(customerLoyaltyHistory));
        await _customerLoyaltyHistoryRepository.InsertAsync(customerLoyaltyHistory);
    }
    public async Task UpdateCustomerLoyaltyHistoryAsync(CustomerLoyaltyHistory customerLoyaltyHistory)
    {
        if (customerLoyaltyHistory == null)
            throw new ArgumentNullException(nameof(customerLoyaltyHistory));
        await _customerLoyaltyHistoryRepository.UpdateAsync(customerLoyaltyHistory);
    }

    public async Task<CustomerLoyaltyHistory> GetCustomerLoyaltyHistoryByOrderIdAsync(int orderId)
    {
        return await _customerLoyaltyHistoryRepository.Table.FirstOrDefaultAsync(x => x.OrderId == Convert.ToInt32(orderId));
    }    
    
}
