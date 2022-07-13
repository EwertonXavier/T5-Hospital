namespace T5_Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class staff : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Appointments");
            CreateTable(
                "dbo.Staffs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            RenameColumn("dbo.Appointments", "AppointmentId", "Id");
            AddColumn("dbo.Appointments", "StaffId", c => c.Int(nullable: false));
            AddColumn("dbo.Appointments", "PatientId", c => c.Int(nullable: false));
            AddColumn("dbo.Appointments", "StartDate", c => c.String());
            DropColumn("dbo.Appointments", "name");
            DropColumn("dbo.Appointments", "email");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Appointments", "email", c => c.String());
            AddColumn("dbo.Appointments", "name", c => c.String());
            DropColumn("dbo.Appointments", "StartDate");
            DropColumn("dbo.Appointments", "PatientId");
            DropColumn("dbo.Appointments", "StaffId");
            DropColumn("dbo.Appointments", "Id");
            DropTable("dbo.Staffs");
        }
    }
}
