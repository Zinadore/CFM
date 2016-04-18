namespace CFM.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUnitModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Units",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Professors", "Unit_Id", c => c.Int());
            CreateIndex("dbo.Professors", "Unit_Id");
            AddForeignKey("dbo.Professors", "Unit_Id", "dbo.Units", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Professors", "Unit_Id", "dbo.Units");
            DropIndex("dbo.Professors", new[] { "Unit_Id" });
            DropColumn("dbo.Professors", "Unit_Id");
            DropTable("dbo.Units");
        }
    }
}
