namespace T5_Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDonation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Donations", "DonationDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Donations", "DonatedAt");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Donations", "DonatedAt", c => c.DateTime(nullable: false));
            DropColumn("dbo.Donations", "DonationDate");
        }
    }
}
