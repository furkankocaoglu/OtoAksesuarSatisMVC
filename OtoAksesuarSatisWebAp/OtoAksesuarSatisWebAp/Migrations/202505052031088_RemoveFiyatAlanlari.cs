namespace OtoAksesuarSatisWebAp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveFiyatAlanlari : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Uruns", "BronzFiyat");
            DropColumn("dbo.Uruns", "SilverFiyat");
            DropColumn("dbo.Uruns", "GoldFiyat");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Uruns", "GoldFiyat", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Uruns", "SilverFiyat", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Uruns", "BronzFiyat", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
