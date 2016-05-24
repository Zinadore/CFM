namespace CFM.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixed_assignment_references : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Assignments", "UnitId", "dbo.Units");
            DropIndex("dbo.Assignments", new[] { "UnitId" });
            RenameColumn(table: "dbo.Assignments", name: "UnitId", newName: "Unit_Id1");
            AlterColumn("dbo.Assignments", "Unit_Id1", c => c.Int());
            CreateIndex("dbo.Assignments", "Unit_Id1");
            AddForeignKey("dbo.Assignments", "Unit_Id1", "dbo.Units", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Assignments", "Unit_Id1", "dbo.Units");
            DropIndex("dbo.Assignments", new[] { "Unit_Id1" });
            AlterColumn("dbo.Assignments", "Unit_Id1", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Assignments", name: "Unit_Id1", newName: "UnitId");
            CreateIndex("dbo.Assignments", "UnitId");
            AddForeignKey("dbo.Assignments", "UnitId", "dbo.Units", "Id", cascadeDelete: true);
        }
    }
}
