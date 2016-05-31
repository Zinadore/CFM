namespace CFM.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_feedback : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 4000),
                        Assignment_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assignments", t => t.Assignment_Id, cascadeDelete: true)
                .Index(t => t.Assignment_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Feedbacks", "Assignment_Id", "dbo.Assignments");
            DropIndex("dbo.Feedbacks", new[] { "Assignment_Id" });
            DropTable("dbo.Feedbacks");
        }
    }
}
