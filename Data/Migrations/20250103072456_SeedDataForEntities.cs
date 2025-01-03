using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Demo3.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataForEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TicketBase",
                columns: table => new
                {
                    TicketId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    TicketType = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    SeatNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoungeAccess = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketBase", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK_TicketBase_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Name", "Type" },
                values: new object[,]
                {
                    { 1, "Music", 0 },
                    { 2, "Sports", 2 },
                    { 3, "Theater", 1 },
                    { 4, "OperaHouse", 4 }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "Email", "Name", "Type" },
                values: new object[,]
                {
                    { 1, "johndoe@example.com", "John Doe", 0 },
                    { 2, "janesmith@example.com", "Jane Smith", 1 },
                    { 3, "alice.johnson@example.com", "Alice Johnson", 0 },
                    { 4, "bob.brown@example.com", "Bob Brown", 3 }
                });

            migrationBuilder.InsertData(
                table: "Performers",
                columns: new[] { "PerformerId", "Name", "Type" },
                values: new object[,]
                {
                    { 1, "John Doe", 0 },
                    { 2, "Emily Davis", 4 },
                    { 3, "Michael Smith", 2 }
                });

            migrationBuilder.InsertData(
                table: "Venues",
                columns: new[] { "VenueId", "Location", "Name", "RowVersion", "Type" },
                values: new object[,]
                {
                    { 1, "Hanoi, Vietnam", "Hanoi Concert Hall", new byte[] { 0, 0, 0, 0 }, 0 },
                    { 2, "Ho Chi Minh City, Vietnam", "Ho Chi Minh Stadium", new byte[] { 0, 0, 0, 1 }, 2 },
                    { 3, "Da Nang, Vietnam", "Da Nang Opera House", new byte[] { 0, 0, 0, 2 }, 3 },
                    { 4, "Hue, Vietnam", "Hue Cultural Center", new byte[] { 0, 0, 0, 3 }, 3 }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "EventId", "CategoryId", "Date", "EventType", "RowVersion", "Status", "Title", "VenueId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Event", new byte[] { 0, 0, 0, 0 }, 0, "Music Concert 2025", 1 },
                    { 2, 2, new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Event", new byte[] { 0, 0, 0, 1 }, 0, "Tech Workshop", 2 },
                    { 3, 1, new DateTime(2025, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Event", new byte[] { 0, 0, 0, 2 }, 0, "Music Event 2025", 1 }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "CustomerId", "OrderDate", "RowVersion", "Status" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new byte[] { 0, 0, 0, 0 }, 0 },
                    { 2, 2, new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new byte[] { 0, 0, 0, 1 }, 2 },
                    { 3, 3, new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new byte[] { 0, 0, 0, 2 }, 3 }
                });

            migrationBuilder.InsertData(
                table: "EventPerformers",
                columns: new[] { "EventId", "PerformerId", "Role" },
                values: new object[,]
                {
                    { 1, 1, 0 },
                    { 1, 2, 3 },
                    { 2, 3, 0 },
                    { 2, 4, 4 }
                });

            migrationBuilder.InsertData(
                table: "TicketBase",
                columns: new[] { "TicketId", "EventId", "Price", "RowVersion", "SeatNumber", "TicketType" },
                values: new object[] { 4, 1, 120.00m, new byte[] { 0, 0, 0, 0 }, "A1", "ConcertTicket" });

            migrationBuilder.InsertData(
                table: "TicketBase",
                columns: new[] { "TicketId", "EventId", "LoungeAccess", "Price", "RowVersion", "TicketType" },
                values: new object[] { 5, 2, "Gold Lounge", 200.00m, new byte[] { 0, 0, 0, 0 }, "VipTicket" });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "TicketId", "EventId", "OrderId", "Price", "RowVersion", "Type" },
                values: new object[,]
                {
                    { 1, 1, null, 50.00m, new byte[] { 0, 0, 0, 0 }, 0 },
                    { 2, 1, 1, 100.00m, new byte[] { 0, 0, 0, 1 }, 0 },
                    { 3, 2, null, 75.00m, new byte[] { 0, 0, 0, 2 }, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketBase_EventId",
                table: "TicketBase",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketBase");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "EventPerformers",
                keyColumns: new[] { "EventId", "PerformerId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "EventPerformers",
                keyColumns: new[] { "EventId", "PerformerId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "EventPerformers",
                keyColumns: new[] { "EventId", "PerformerId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "EventPerformers",
                keyColumns: new[] { "EventId", "PerformerId" },
                keyValues: new object[] { 2, 4 });

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "TicketId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "TicketId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "TicketId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Venues",
                keyColumn: "VenueId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Venues",
                keyColumn: "VenueId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Performers",
                keyColumn: "PerformerId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Performers",
                keyColumn: "PerformerId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Performers",
                keyColumn: "PerformerId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Venues",
                keyColumn: "VenueId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Venues",
                keyColumn: "VenueId",
                keyValue: 2);
        }
    }
}
