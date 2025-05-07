namespace OtoAksesuarSatisWebAp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveXMLFavori : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.XMLFavoris", "UyeID", "dbo.Uyes");
            DropForeignKey("dbo.XMLFavoris", "XmlUrunID", "dbo.XMLUruns");
            DropIndex("dbo.XMLFavoris", new[] { "XmlUrunID" });
            DropIndex("dbo.XMLFavoris", new[] { "UyeID" });
            DropTable("dbo.XMLFavoris");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.XMLFavoris",
                c => new
                    {
                        XmlFavoriID = c.Int(nullable: false, identity: true),
                        XmlUrunID = c.Int(nullable: false),
                        UyeID = c.Int(nullable: false),
                        Silinmis = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.XmlFavoriID);
            
            CreateIndex("dbo.XMLFavoris", "UyeID");
            CreateIndex("dbo.XMLFavoris", "XmlUrunID");
            AddForeignKey("dbo.XMLFavoris", "XmlUrunID", "dbo.XMLUruns", "XmlUrunID", cascadeDelete: true);
            AddForeignKey("dbo.XMLFavoris", "UyeID", "dbo.Uyes", "UyeID", cascadeDelete: true);
        }
    }
}
