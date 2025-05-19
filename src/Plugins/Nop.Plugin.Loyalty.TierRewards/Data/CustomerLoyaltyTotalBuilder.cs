
using FluentMigrator.Builders.Create.Table;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Customer.Loyalty.Domain;

namespace Nop.Plugin.Customer.Loyalty.Data;
public class CustomerLoyaltyTotalBuilder : NopEntityBuilder<CustomerLoyaltyTotal>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
           .WithColumn(nameof(CustomerLoyaltyTotal.CustomerId)).AsInt32().ForeignKey<Nop.Core.Domain.Customers.Customer>()
           .WithColumn(nameof(CustomerLoyaltyTotal.TotalSpentAmount)).AsDecimal(18, 4).WithDefaultValue(0)
           .WithColumn(nameof(CustomerLoyaltyTotal.TotalPurchasedQuantity)).AsInt32()
           .WithColumn(nameof(CustomerLoyaltyTotal.UpdatedOnUtc)).AsDateTime().NotNullable();
    }
}
