namespace CFM.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Goals : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Goals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 4000),
                        Deadline = c.DateTime(nullable: false),
                        Assignment_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assignments", t => t.Assignment_Id, cascadeDelete: true)
                .Index(t => t.Assignment_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Goals", "Assignment_Id", "dbo.Assignments");
            DropIndex("dbo.Goals", new[] { "Assignment_Id" });
            DropTable("dbo.Goals");
        }
    }
}
