using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Orders;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Customer.Loyalty.Domain;
namespace Nop.Plugin.Customer.Loyalty.Data;
public class CustomerLoyaltyHistoryBuilder : NopEntityBuilder<CustomerLoyaltyHistory>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(CustomerLoyaltyHistory.CustomerId)).AsInt32().ForeignKey<Nop.Core.Domain.Customers.Customer>() 
            .WithColumn(nameof(CustomerLoyaltyHistory.OrderId)).AsInt32().ForeignKey<Order>()
            .WithColumn(nameof(CustomerLoyaltyHistory.OrderStatusId)).AsInt32().NotNullable()
            .WithColumn(nameof(CustomerLoyaltyHistory.ChangedAmount)).AsDecimal(18, 4).WithDefaultValue(0)
            .WithColumn(nameof(CustomerLoyaltyHistory.ChangedQuantity)).AsInt32().WithDefaultValue(0)
            .WithColumn(nameof(CustomerLoyaltyHistory.CreatedOnUtc)).AsDateTime().NotNullable();
    }
}
