namespace T5_Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DonationAndDepartment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Donations", "DepartmentId", c => c.Int(nullable: false));
            CreateIndex("dbo.Donations", "DepartmentId");
            AddForeignKey("dbo.Donations", "DepartmentId", "dbo.Departments", "DepartmentId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Donations", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.Donations", new[] { "DepartmentId" });
            DropColumn("dbo.Donations", "DepartmentId");
        }
    }
}
