using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Data.Mapping.Builders;
using FluentMigrator.Builders.Create.Table;
using Nop.Plugin.Customer.Loyalty.Domain;
using Nop.Core.Domain.Discounts;
using Nop.Data.Extensions;
namespace Nop.Plugin.Customer.Loyalty.Data;
public class LoyaltyLevelDiscountMappingBuilder : NopEntityBuilder<LoyaltyLevelDiscountMapping>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(LoyaltyLevelDiscountMapping.DiscountId)).AsInt32().ForeignKey<Discount>().PrimaryKey()
            .WithColumn(nameof(LoyaltyLevelDiscountMapping.LoyaltyLevelId)).AsInt32().ForeignKey<LoyaltyLevel>().PrimaryKey().
            WithColumn(nameof(LoyaltyLevelDiscountMapping.EnableInherit)).AsBoolean();
    }
}
