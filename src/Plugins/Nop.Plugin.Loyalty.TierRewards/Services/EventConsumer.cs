using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Orders;
using Nop.Data;
using Nop.Plugin.Customer.Loyalty.Domain;
using Nop.Services.Events;

namespace Nop.Plugin.Customer.Loyalty.Services;
public class EventConsumer : IConsumer<OrderStatusChangedEvent>
{
    private readonly ICustomerLoyaltyHistoryService _customerLoyaltyHistoryService;

    private readonly ICustomerLoyaltyTotalService _customerLoyaltyTotalService;

    private readonly IRepository<Order> _orderRepository;

    private readonly IRepository<OrderItem> _orderItemRepository;

    public EventConsumer(ICustomerLoyaltyHistoryService customerLoyaltyHistoryService,
        ICustomerLoyaltyTotalService customerLoyaltyTotalService,
        IRepository<Order> orderRepository, IRepository<OrderItem> orderItemRepository)
    {
        _customerLoyaltyHistoryService = customerLoyaltyHistoryService;
        _customerLoyaltyTotalService = customerLoyaltyTotalService;
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
    }

    public async Task HandleEventAsync(OrderStatusChangedEvent eventMessage)
    {
        Order order = eventMessage.Order;
        if (order == null || order.Deleted)
            return;
        var orderTotal = order.OrderTotal;
        var quantityPurchasedTotal = (from o in _orderRepository.Table
                                      join oi in _orderItemRepository.Table on o.Id equals oi.OrderId
                                      where o.CustomerId == order.CustomerId && o.Id == order.Id
                                      select oi)
                             .SumAsync(x => x.Quantity).Result;

        var loyaltyHistory = await _customerLoyaltyHistoryService.GetCustomerLoyaltyHistoryByOrderIdAsync(order.Id);
        if (order.OrderStatus == OrderStatus.Complete)
        {
            if (loyaltyHistory == null || loyaltyHistory.OrderStatus == OrderStatus.Cancelled)
            {
                if (loyaltyHistory == null)
                {
                    await _customerLoyaltyHistoryService.InsertCustomerLoyaltyHistoryAsync(new CustomerLoyaltyHistory
                    {
                        CustomerId = order.CustomerId,
                        OrderId = order.Id,
                        OrderStatus = order.OrderStatus,
                        ChangedAmount = orderTotal,
                        ChangedQuantity = quantityPurchasedTotal,
                        CreatedOnUtc = DateTime.UtcNow
                    });
                }

                else
                {
                    loyaltyHistory.OrderStatus = order.OrderStatus;
                    await _customerLoyaltyHistoryService.UpdateCustomerLoyaltyHistoryAsync(loyaltyHistory);
                }
                await _customerLoyaltyTotalService.UpdateQuantityAndSpentTotalByCustomerIdAsync(order.CustomerId, orderTotal, quantityPurchasedTotal);
            }


        }
        else if (order.OrderStatus == OrderStatus.Cancelled)
        {
            if (loyaltyHistory != null && loyaltyHistory.OrderStatus == OrderStatus.Complete)
            {
                loyaltyHistory.OrderStatus = order.OrderStatus;
                await _customerLoyaltyHistoryService.UpdateCustomerLoyaltyHistoryAsync(loyaltyHistory);
                await _customerLoyaltyTotalService.UpdateQuantityAndSpentTotalByCustomerIdAsync(order.CustomerId, -orderTotal, -quantityPurchasedTotal);
            }
        }


    }
}


