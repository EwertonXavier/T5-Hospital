namespace T5_Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VisitationPatient_FK : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Visitations", "PatientId", c => c.Int(nullable: false));
            CreateIndex("dbo.Visitations", "PatientId");
            AddForeignKey("dbo.Visitations", "PatientId", "dbo.Patients", "PatientId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Visitations", "PatientId", "dbo.Patients");
            DropIndex("dbo.Visitations", new[] { "PatientId" });
            DropColumn("dbo.Visitations", "PatientId");
        }
    }
}
