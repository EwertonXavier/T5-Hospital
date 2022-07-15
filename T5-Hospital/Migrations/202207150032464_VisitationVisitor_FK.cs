namespace T5_Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VisitationVisitor_FK : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Visitations", "VisitorId", c => c.Int(nullable: false));
            CreateIndex("dbo.Visitations", "VisitorId");
            AddForeignKey("dbo.Visitations", "VisitorId", "dbo.Visitors", "VisitorId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Visitations", "VisitorId", "dbo.Visitors");
            DropIndex("dbo.Visitations", new[] { "VisitorId" });
            DropColumn("dbo.Visitations", "VisitorId");
        }
    }
}
