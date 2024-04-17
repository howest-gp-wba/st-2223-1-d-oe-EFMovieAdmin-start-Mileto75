using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wba.Oefening.RateAMovie.Web.Migrations
{
    public partial class FixSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Actors",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1L, "Keanu", "Reeves" },
                    { 2L, "Laurence", "Fishburne" },
                    { 3L, "Carrie-Anne", "Moss" },
                    { 4L, "Sean", "Bean" }
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "The Wachowski Brothers" },
                    { 20L, "Newline Cinema" }
                });

            migrationBuilder.InsertData(
                table: "Directors",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1L, "Lana", "Wachowski" },
                    { 2L, "Lilly", "Wachowski" },
                    { 3L, "Peter", "Jackson" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FirstName", "LastName", "Password", "Username" },
                values: new object[,]
                {
                    { 1L, "Siegfried", "Derdeyn", null, "siegfried.derdeyn" },
                    { 2L, "Joachim", "François", null, "joachim.francois" }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "CompanyId", "ImageFileName", "ReleaseDate", "Title" },
                values: new object[] { 1L, 1L, null, new DateTime(1999, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Matrix" });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "CompanyId", "ImageFileName", "ReleaseDate", "Title" },
                values: new object[] { 2L, 20L, null, new DateTime(2001, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Fellowship of the Ring" });

            migrationBuilder.InsertData(
                table: "ActorMovie",
                columns: new[] { "ActorsId", "MoviesId" },
                values: new object[,]
                {
                    { 1L, 1L },
                    { 2L, 1L },
                    { 3L, 1L },
                    { 4L, 2L }
                });

            migrationBuilder.InsertData(
                table: "DirectorMovie",
                columns: new[] { "DirectorsId", "MoviesId" },
                values: new object[,]
                {
                    { 1L, 1L },
                    { 2L, 1L },
                    { 3L, 2L }
                });

            migrationBuilder.InsertData(
                table: "Ratings",
                columns: new[] { "Id", "RatedMovieId", "Review", "ReviewerId", "Score" },
                values: new object[,]
                {
                    { 100L, 1L, "I think this movie is ok!", 1L, (byte)3 },
                    { 102L, 1L, null, 2L, (byte)4 },
                    { 101L, 2L, "I think this movie is really nice!", 1L, (byte)4 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActorMovie",
                keyColumns: new[] { "ActorsId", "MoviesId" },
                keyValues: new object[] { 1L, 1L });

            migrationBuilder.DeleteData(
                table: "ActorMovie",
                keyColumns: new[] { "ActorsId", "MoviesId" },
                keyValues: new object[] { 2L, 1L });

            migrationBuilder.DeleteData(
                table: "ActorMovie",
                keyColumns: new[] { "ActorsId", "MoviesId" },
                keyValues: new object[] { 3L, 1L });

            migrationBuilder.DeleteData(
                table: "ActorMovie",
                keyColumns: new[] { "ActorsId", "MoviesId" },
                keyValues: new object[] { 4L, 2L });

            migrationBuilder.DeleteData(
                table: "DirectorMovie",
                keyColumns: new[] { "DirectorsId", "MoviesId" },
                keyValues: new object[] { 1L, 1L });

            migrationBuilder.DeleteData(
                table: "DirectorMovie",
                keyColumns: new[] { "DirectorsId", "MoviesId" },
                keyValues: new object[] { 2L, 1L });

            migrationBuilder.DeleteData(
                table: "DirectorMovie",
                keyColumns: new[] { "DirectorsId", "MoviesId" },
                keyValues: new object[] { 3L, 2L });

            migrationBuilder.DeleteData(
                table: "Ratings",
                keyColumn: "Id",
                keyValue: 100L);

            migrationBuilder.DeleteData(
                table: "Ratings",
                keyColumn: "Id",
                keyValue: 101L);

            migrationBuilder.DeleteData(
                table: "Ratings",
                keyColumn: "Id",
                keyValue: 102L);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Directors",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Directors",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Directors",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 20L);
        }
    }
}
