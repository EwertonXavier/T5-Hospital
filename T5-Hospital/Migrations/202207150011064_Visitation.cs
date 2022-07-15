namespace T5_Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Visitation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Visitations",
                c => new
                    {
                        VisitationId = c.Int(nullable: false, identity: true),
                        ArrivalDate = c.DateTime(nullable: false),
                        DepartureDate = c.DateTime(nullable: false),
                        RelationToVisitor = c.String(),
                    })
                .PrimaryKey(t => t.VisitationId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Visitations");
        }
    }
}
