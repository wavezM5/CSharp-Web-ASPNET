﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Homies.Data.Migrations
{
    public partial class removedUselessSecondEventId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventsParticipants_Events_EventId",
                table: "EventsParticipants");

            migrationBuilder.DropForeignKey(
                name: "FK_EventsParticipants_Events_EventId1",
                table: "EventsParticipants");

            migrationBuilder.DropIndex(
                name: "IX_EventsParticipants_EventId1",
                table: "EventsParticipants");

            migrationBuilder.DropColumn(
                name: "EventId1",
                table: "EventsParticipants");

            migrationBuilder.AddForeignKey(
                name: "FK_EventsParticipants_Events_EventId",
                table: "EventsParticipants",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventsParticipants_Events_EventId",
                table: "EventsParticipants");

            migrationBuilder.AddColumn<int>(
                name: "EventId1",
                table: "EventsParticipants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventsParticipants_EventId1",
                table: "EventsParticipants",
                column: "EventId1");

            migrationBuilder.AddForeignKey(
                name: "FK_EventsParticipants_Events_EventId",
                table: "EventsParticipants",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EventsParticipants_Events_EventId1",
                table: "EventsParticipants",
                column: "EventId1",
                principalTable: "Events",
                principalColumn: "Id");
        }
    }
}
