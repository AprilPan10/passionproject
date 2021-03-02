namespace mypassionproject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pet2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pets", "PetHasPic", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pets", "PetHasPic");
        }
    }
}
