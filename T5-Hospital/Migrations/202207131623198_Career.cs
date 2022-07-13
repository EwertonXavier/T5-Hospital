namespace T5_Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Career : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Careers",
                c => new
                    {
                        JobId = c.Int(nullable: false, identity: true),
                        DepartmentId = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        Experience_In_Years = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .Index(t => t.DepartmentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Careers", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.Careers", new[] { "DepartmentId" });
            DropTable("dbo.Careers");
        }
    }
}
