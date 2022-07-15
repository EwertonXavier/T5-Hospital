namespace T5_Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Staff_FK : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "AppointmentDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Staffs", "DepartmentId", c => c.Int(nullable: false));
            CreateIndex("dbo.Staffs", "DepartmentId");
            AddForeignKey("dbo.Staffs", "DepartmentId", "dbo.Departments", "DepartmentId", cascadeDelete: true);
            DropColumn("dbo.Appointments", "StartDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Appointments", "StartDate", c => c.String());
            DropForeignKey("dbo.Staffs", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.Staffs", new[] { "DepartmentId" });
            DropColumn("dbo.Staffs", "DepartmentId");
            DropColumn("dbo.Appointments", "AppointmentDate");
        }
    }
}
