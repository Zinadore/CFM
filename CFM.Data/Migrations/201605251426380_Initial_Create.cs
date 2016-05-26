namespace CFM.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_Create : DbMigration
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
                        Unit_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Units", t => t.Unit_Id)
                .Index(t => t.Unit_Id);
            
            CreateTable(
                "dbo.Units",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Professors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UnitProfessors",
                c => new
                    {
                        Unit_Id = c.Int(nullable: false),
                        Professor_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Unit_Id, t.Professor_Id })
                .ForeignKey("dbo.Units", t => t.Unit_Id, cascadeDelete: true)
                .ForeignKey("dbo.Professors", t => t.Professor_Id, cascadeDelete: true)
                .Index(t => t.Unit_Id)
                .Index(t => t.Professor_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UnitProfessors", "Professor_Id", "dbo.Professors");
            DropForeignKey("dbo.UnitProfessors", "Unit_Id", "dbo.Units");
            DropForeignKey("dbo.Assignments", "Unit_Id", "dbo.Units");
            DropIndex("dbo.UnitProfessors", new[] { "Professor_Id" });
            DropIndex("dbo.UnitProfessors", new[] { "Unit_Id" });
            DropIndex("dbo.Assignments", new[] { "Unit_Id" });
            DropTable("dbo.UnitProfessors");
            DropTable("dbo.Professors");
            DropTable("dbo.Units");
            DropTable("dbo.Assignments");
        }
    }
}
