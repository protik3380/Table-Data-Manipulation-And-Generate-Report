namespace MindFork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class student : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Student",
                c => new
                    {
                        S_Id = c.Int(nullable: false, identity: true),
                        StudentId = c.String(nullable: false),
                        StudentName = c.String(nullable: false),
                        SubjectId = c.Int(nullable: false),
                        Mark = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.S_Id)
                .ForeignKey("dbo.Subject", t => t.SubjectId)
                .Index(t => t.SubjectId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Student", "SubjectId", "dbo.Subject");
            DropIndex("dbo.Student", new[] { "SubjectId" });
            DropTable("dbo.Student");
        }
    }
}
