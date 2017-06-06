namespace GIS_visualization.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Images", "Longitude", c => c.String());
            AlterColumn("dbo.Images", "Latitude", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Images", "Latitude", c => c.Double(nullable: false));
            AlterColumn("dbo.Images", "Longitude", c => c.Double(nullable: false));
        }
    }
}
