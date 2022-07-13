namespace T5_Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Service : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        ServiceId = c.Int(nullable: false, identity: true),
                        DepartmentId = c.Int(nullable: false),
                        ServiceName = c.String(),
                        ServiceDetail = c.String(),
                    })
                .PrimaryKey(t => t.ServiceId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .Index(t => t.DepartmentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Services", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.Services", new[] { "DepartmentId" });
            DropTable("dbo.Services");
        }
    }
}
