using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Orders;

namespace Nop.Plugin.Customer.Loyalty.Domain;
public class CustomerLoyaltyHistory : BaseEntity
{
    public int CustomerId { get; set; }

    public int OrderId { get; set; }

    public int OrderStatusId { get; set; }

    public decimal ChangedAmount { get; set; }

    public int ChangedQuantity { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public OrderStatus OrderStatus
    {
        get => (OrderStatus)OrderStatusId;
        set => OrderStatusId = (int)value;
    }


    public CustomerLoyaltyHistory(int customerId, int orderId, OrderStatus orderStatus, decimal changedAmount, int changedQuantity)
    {
        CustomerId = customerId;
        OrderId = orderId;
        OrderStatus = orderStatus;
        ChangedAmount = changedAmount;
        ChangedQuantity = changedQuantity;
        CreatedOnUtc = DateTime.Now;
    }

    public CustomerLoyaltyHistory()
    {
    }
}
