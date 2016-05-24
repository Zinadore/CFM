namespace CFM.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class assignment_entity_added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assignments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Deadline = c.DateTime(nullable: false),
                        Type = c.String(),
                        Title = c.String(),
                        UnitId = c.Int(nullable: false),
                        Unit_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Units", t => t.Unit_Id)
                .ForeignKey("dbo.Units", t => t.UnitId, cascadeDelete: true)
                .Index(t => t.UnitId)
                .Index(t => t.Unit_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Assignments", "UnitId", "dbo.Units");
            DropForeignKey("dbo.Assignments", "Unit_Id", "dbo.Units");
            DropIndex("dbo.Assignments", new[] { "Unit_Id" });
            DropIndex("dbo.Assignments", new[] { "UnitId" });
            DropTable("dbo.Assignments");
        }
    }
}
