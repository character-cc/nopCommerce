using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Customers;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Customer.Loyalty.Domain;

namespace Nop.Plugin.Customer.Loyalty.Data;
public class LoyaltyLevelBuilder : NopEntityBuilder<LoyaltyLevel>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(LoyaltyLevel.MinimumProductQuantity)).AsInt32()
            .WithColumn(nameof(LoyaltyLevel.MinimumSpentAmount)).AsInt32()
            .WithColumn(nameof(LoyaltyLevel.FriendlyName)).AsString(100).NotNullable()
            .WithColumn(nameof(LoyaltyLevel.SystemName)).AsString(100).NotNullable();
            
    }
}

