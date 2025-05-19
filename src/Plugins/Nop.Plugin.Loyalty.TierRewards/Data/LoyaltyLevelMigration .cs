using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;
using Nop.Core.Domain.Catalog;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Customer.Loyalty.Domain;

namespace Nop.Plugin.Customer.Loyalty.Data;

[NopMigration("2025/05/01 12:00:00", "Loyalty Base Schema", MigrationProcessType.Installation)]
public class LoyaltyLevelMigration : AutoReversingMigration
{
    public override void Up()
    {
        Create.TableFor<LoyaltyLevel>();
        Create.TableFor<CustomerLoyaltyHistory>();
        Create.TableFor<CustomerLoyaltyTotal>();
        Create.TableFor<LoyaltyLevelDiscountMapping>();
        //Create.Index("IX_CustomerLoyaltyHistory_CustomerId")
        //    .OnTable(nameof(CustomerLoyaltyHistory))
        //    .OnColumn(nameof(CustomerLoyaltyHistory.CustomerId))
        //    .Ascending()
        //    .WithOptions()
        //    .NonClustered();

        //Create.Index("IX_CustomerLoyaltyHistory_OrderId")
        //    .OnTable(nameof(CustomerLoyaltyHistory))
        //    .OnColumn(nameof(CustomerLoyaltyHistory.OrderId))
        //    .Ascending()
        //    .WithOptions()
        //    .NonClustered();
    }
}
