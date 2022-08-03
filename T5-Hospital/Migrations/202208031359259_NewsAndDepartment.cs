namespace T5_Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewsAndDepartment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "DepartmentId", c => c.Int(nullable: false));
            CreateIndex("dbo.News", "DepartmentId");
            AddForeignKey("dbo.News", "DepartmentId", "dbo.Departments", "DepartmentId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.News", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.News", new[] { "DepartmentId" });
            DropColumn("dbo.News", "DepartmentId");
        }
    }
}
