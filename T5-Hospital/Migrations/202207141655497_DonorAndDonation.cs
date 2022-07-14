namespace T5_Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DonorAndDonation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Donations",
                c => new
                    {
                        DonationId = c.Int(nullable: false, identity: true),
                        DonationAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DonatedAt = c.DateTime(nullable: false),
                        DonationDescription = c.String(),
                        DonorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DonationId)
                .ForeignKey("dbo.Donors", t => t.DonorId, cascadeDelete: true)
                .Index(t => t.DonorId);
            
            CreateTable(
                "dbo.Donors",
                c => new
                    {
                        DonorId = c.Int(nullable: false, identity: true),
                        DonorFirstName = c.String(),
                        DonorLastName = c.String(),
                        DonorEmail = c.String(),
                        DonorPhone = c.String(),
                    })
                .PrimaryKey(t => t.DonorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Donations", "DonorId", "dbo.Donors");
            DropIndex("dbo.Donations", new[] { "DonorId" });
            DropTable("dbo.Donors");
            DropTable("dbo.Donations");
        }
    }
}
