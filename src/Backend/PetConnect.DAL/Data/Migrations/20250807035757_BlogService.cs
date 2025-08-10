using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetConnect.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class BlogService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          
            migrationBuilder.CreateTable(
                name: "BlogCommentReplies",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommentReply = table.Column<string>(type: "varchar(max)", nullable: false),
                    Media = table.Column<string>(type: "varchar(200)", nullable: false),
                    CommentORReplyORLikeType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogCommentReplies", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BlogComments",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Comment = table.Column<string>(type: "varchar(max)", nullable: false),
                    Media = table.Column<string>(type: "varchar(200)", nullable: false),
                    CommentORReplyType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogComments", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "varchar(max)", nullable: false),
                    Media = table.Column<string>(type: "varchar(200)", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    BlogType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GetUtcDate()"),
                    DoctorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Blogs_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserBlogCommentReplyLikes",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BlogCommentReplyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBlogCommentReplyLikes", x => new { x.BlogCommentReplyId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserBlogCommentReplyLikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserBlogCommentReplyLikes_BlogCommentReplies_BlogCommentReplyId",
                        column: x => x.BlogCommentReplyId,
                        principalTable: "BlogCommentReplies",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "UserBlogCommentLikes",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BlogCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBlogCommentLikes", x => new { x.BlogCommentId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserBlogCommentLikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserBlogCommentLikes_BlogComments_BlogCommentId",
                        column: x => x.BlogCommentId,
                        principalTable: "BlogComments",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "UserBlogCommentReplies",
                columns: table => new
                {
                    BlogCommentReplyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BlogCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReplyDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GetUtcDate()"),
                    UserType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBlogCommentReplies", x => x.BlogCommentReplyId);
                    table.ForeignKey(
                        name: "FK_UserBlogCommentReplies_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserBlogCommentReplies_BlogCommentReplies_BlogCommentReplyId",
                        column: x => x.BlogCommentReplyId,
                        principalTable: "BlogCommentReplies",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_UserBlogCommentReplies_BlogComments_BlogCommentId",
                        column: x => x.BlogCommentId,
                        principalTable: "BlogComments",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "UserBlogComments",
                columns: table => new
                {
                    BlogCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CommentDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GetUtcDate()"),
                    UserType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBlogComments", x => x.BlogCommentId);
                    table.ForeignKey(
                        name: "FK_UserBlogComments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserBlogComments_BlogComments_BlogCommentId",
                        column: x => x.BlogCommentId,
                        principalTable: "BlogComments",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_UserBlogComments_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "UserBlogLikes",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BlogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBlogLikes", x => new { x.BlogId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserBlogLikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserBlogLikes_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "ID");
                });

     
            migrationBuilder.CreateIndex(
                name: "IX_Blogs_DoctorId",
                table: "Blogs",
                column: "DoctorId");

        
            migrationBuilder.CreateIndex(
                name: "IX_UserBlogCommentLikes_UserId",
                table: "UserBlogCommentLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBlogCommentReplies_BlogCommentId",
                table: "UserBlogCommentReplies",
                column: "BlogCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBlogCommentReplies_UserId",
                table: "UserBlogCommentReplies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBlogCommentReplyLikes_UserId",
                table: "UserBlogCommentReplyLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBlogComments_BlogId",
                table: "UserBlogComments",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBlogComments_UserId",
                table: "UserBlogComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBlogLikes_UserId",
                table: "UserBlogLikes",
                column: "UserId");

 
    
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
       

            migrationBuilder.DropTable(
                name: "UserBlogCommentLikes");

            migrationBuilder.DropTable(
                name: "UserBlogCommentReplies");

            migrationBuilder.DropTable(
                name: "UserBlogCommentReplyLikes");

            migrationBuilder.DropTable(
                name: "UserBlogComments");

            migrationBuilder.DropTable(
                name: "UserBlogLikes");


            migrationBuilder.DropTable(
                name: "BlogCommentReplies");

            migrationBuilder.DropTable(
                name: "BlogComments");

            migrationBuilder.DropTable(
                name: "Blogs");


        }
    }
}
