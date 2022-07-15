namespace T5_Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Visitor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Visitors",
                c => new
                    {
                        VisitorId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.VisitorId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Visitors");
        }
    }
}
