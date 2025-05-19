using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Data;
using Nop.Plugin.Customer.Loyalty.Domain;

namespace Nop.Plugin.Customer.Loyalty.Services;
public class CustomerLoyaltyTotalService : ICustomerLoyaltyTotalService
{

    private readonly IRepository<CustomerLoyaltyTotal> _customerLoyaltyTotalRepository;

    public CustomerLoyaltyTotalService(IRepository<CustomerLoyaltyTotal> customerLoyaltyTotalRepository)
    {
        _customerLoyaltyTotalRepository = customerLoyaltyTotalRepository;
    }
    public async Task UpdateQuantityAndSpentTotalByCustomerIdAsync(int custmoerId, decimal amountDelta, int quanityDelta)
    {
       var loyaltyTotal = await GetCustomerLoyaltyTotalByCustomerIdAsync(custmoerId);
        if (loyaltyTotal == null)
        {
            loyaltyTotal = new CustomerLoyaltyTotal
            {
                CustomerId = custmoerId,
                TotalSpentAmount = amountDelta,
                TotalPurchasedQuantity = quanityDelta,
                UpdatedOnUtc = DateTime.UtcNow
            };
            // Insert the new record into the database
            await InsertCustomerLoyaltyTotalAsync(loyaltyTotal);
        }
        else
        {
            loyaltyTotal.TotalSpentAmount += amountDelta;
            loyaltyTotal.TotalPurchasedQuantity += quanityDelta;
            loyaltyTotal.UpdatedOnUtc = DateTime.UtcNow;
            // Update the existing record in the database
            await UpdateCustomerLoyaltyTotalAsync(loyaltyTotal);
        }
    }


    public async Task<CustomerLoyaltyTotal> GetCustomerLoyaltyTotalByCustomerIdAsync(int customerId)
    {
        return await _customerLoyaltyTotalRepository.Table
            .FirstOrDefaultAsync(x => x.CustomerId == customerId);
    }

    public async Task UpdateCustomerLoyaltyTotalAsync(CustomerLoyaltyTotal customerLoyaltyTotal)
    {
        if (customerLoyaltyTotal == null)
            throw new ArgumentNullException(nameof(customerLoyaltyTotal));
        // Update the existing record in the database
        await _customerLoyaltyTotalRepository.UpdateAsync(customerLoyaltyTotal);

    }

    public async Task InsertCustomerLoyaltyTotalAsync(CustomerLoyaltyTotal customerLoyaltyTotal)
    {
        if (customerLoyaltyTotal == null)
            throw new ArgumentNullException(nameof(customerLoyaltyTotal));
        // Insert the new record into the database
        await _customerLoyaltyTotalRepository.InsertAsync(customerLoyaltyTotal);
    }
}
