using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Phishing_Platform_Midterm.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "phishingtemplate",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    templatemail = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("phishingtemplate_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "targetemail",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    targetemail = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("targetemail_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    sourcepage = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    registrationDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sentemail",
                columns: table => new
                {
                    emailid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    templateid = table.Column<int>(type: "integer", nullable: true),
                    targetid = table.Column<int>(type: "integer", nullable: true),
                    sentat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    isclicked = table.Column<bool>(type: "boolean", nullable: true),
                    clickedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("sentemail_pkey", x => x.emailid);
                    table.ForeignKey(
                        name: "sentemail_targetid_fkey",
                        column: x => x.targetid,
                        principalTable: "targetemail",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "sentemail_templateid_fkey",
                        column: x => x.templateid,
                        principalTable: "phishingtemplate",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "userinteraction",
                columns: table => new
                {
                    interactionid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    emailid = table.Column<int>(type: "integer", nullable: true),
                    userid = table.Column<int>(type: "integer", nullable: true),
                    interactiontype = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    interactiondetail = table.Column<string>(type: "text", nullable: true),
                    interactiontime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("userinteraction_pkey", x => x.interactionid);
                    table.ForeignKey(
                        name: "userinteraction_emailid_fkey",
                        column: x => x.emailid,
                        principalTable: "sentemail",
                        principalColumn: "emailid");
                    table.ForeignKey(
                        name: "userinteraction_userid_fkey",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_sentemail_targetid",
                table: "sentemail",
                column: "targetid");

            migrationBuilder.CreateIndex(
                name: "IX_sentemail_templateid",
                table: "sentemail",
                column: "templateid");

            migrationBuilder.CreateIndex(
                name: "IX_userinteraction_emailid",
                table: "userinteraction",
                column: "emailid");

            migrationBuilder.CreateIndex(
                name: "IX_userinteraction_userid",
                table: "userinteraction",
                column: "userid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userinteraction");

            migrationBuilder.DropTable(
                name: "sentemail");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "targetemail");

            migrationBuilder.DropTable(
                name: "phishingtemplate");
        }
    }
}
