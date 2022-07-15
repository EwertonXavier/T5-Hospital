namespace T5_Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Appointment_fk : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Appointments", "PatientId");
            CreateIndex("dbo.Appointments", "StaffId");
            AddForeignKey("dbo.Appointments", "PatientId", "dbo.Patients", "PatientId", cascadeDelete: true);
            AddForeignKey("dbo.Appointments", "StaffId", "dbo.Staffs", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "StaffId", "dbo.Staffs");
            DropForeignKey("dbo.Appointments", "PatientId", "dbo.Patients");
            DropIndex("dbo.Appointments", new[] { "StaffId" });
            DropIndex("dbo.Appointments", new[] { "PatientId" });
        }
    }
}
