using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBooking.DataAccess.MigrationsNew
{
    /// <inheritdoc />
    public partial class AddedHotelIdToBookedRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "hotelId",
                table: "BookedRooms",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "hotelId",
                table: "BookedRooms");
        }
    }
}
