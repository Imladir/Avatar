﻿// <auto-generated />
using System;
using EmeraldBot.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EmeraldBot.Model.Migrations
{
    [DbContext(typeof(AvatarBotContext))]
    partial class AvatarBotContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0-preview6.19304.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EmeraldBot.Model.Characters.Character", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Alias")
                        .IsRequired();

                    b.Property<int>("Colour");

                    b.Property<string>("Description");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<bool>("Hidden");

                    b.Property<string>("Icon");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<int>("ServerID");

                    b.HasKey("ID");

                    b.HasIndex("ServerID");

                    b.ToTable("Characters");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Character");
                });

            modelBuilder.Entity("EmeraldBot.Model.Identity.Role", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<DateTime>("LastUpdate");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedRoleName");

                    b.HasKey("ID");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("EmeraldBot.Model.Identity.RoleClaim", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int?>("RoleID");

                    b.HasKey("ID");

                    b.HasIndex("RoleID");

                    b.ToTable("RoleClaims");
                });

            modelBuilder.Entity("EmeraldBot.Model.Identity.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessFailedCount");

                    b.Property<long>("DiscordID");

                    b.Property<DateTime>("LastUpdate");

                    b.Property<string>("NormalizedUserName");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("UserName");

                    b.Property<bool>("Verbose");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EmeraldBot.Model.Identity.UserClaim", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int?>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("EmeraldBot.Model.Identity.UserRole", b =>
                {
                    b.Property<int>("UserID");

                    b.Property<int>("RoleID");

                    b.Property<int?>("ServerID");

                    b.HasKey("UserID", "RoleID");

                    b.HasIndex("RoleID");

                    b.HasIndex("ServerID");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("EmeraldBot.Model.Identity.UserToken", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int?>("UserID");

                    b.Property<string>("Value");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("EmeraldBot.Model.Servers.DefaultCharacter", b =>
                {
                    b.Property<int>("ServerID");

                    b.Property<int>("PlayerID");

                    b.Property<int?>("CharacterID");

                    b.HasKey("ServerID", "PlayerID");

                    b.HasIndex("CharacterID");

                    b.HasIndex("PlayerID");

                    b.ToTable("DefaultCharacter");
                });

            modelBuilder.Entity("EmeraldBot.Model.Servers.Message", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Colour");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<long>("DiscordChannelID");

                    b.Property<long>("DiscordMessageID");

                    b.Property<string>("Icon");

                    b.Property<DateTime>("LastUpdated");

                    b.Property<int?>("PlayerID");

                    b.Property<int?>("ServerID");

                    b.Property<string>("Text");

                    b.Property<string>("Title");

                    b.HasKey("ID");

                    b.HasIndex("PlayerID");

                    b.HasIndex("ServerID");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("EmeraldBot.Model.Servers.PrivateChannel", b =>
                {
                    b.Property<int>("PlayerID");

                    b.Property<int>("ServerID");

                    b.Property<long>("ChannelDiscordID");

                    b.HasKey("PlayerID", "ServerID");

                    b.HasIndex("ServerID");

                    b.ToTable("PrivateChannels");
                });

            modelBuilder.Entity("EmeraldBot.Model.Servers.Server", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("DiceChannelID");

                    b.Property<long>("DiscordID");

                    b.Property<string>("Name");

                    b.Property<string>("Prefix")
                        .IsRequired()
                        .HasMaxLength(1);

                    b.HasKey("ID");

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("ProviderKey");

                    b.HasKey("UserId");

                    b.ToTable("IdentityUserLogin<string>");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId");

                    b.ToTable("IdentityUserToken<string>");
                });

            modelBuilder.Entity("EmeraldBot.Model.Characters.NPC", b =>
                {
                    b.HasBaseType("EmeraldBot.Model.Characters.Character");

                    b.ToTable("NonPlayerCharacters");

                    b.HasDiscriminator().HasValue("NPC");
                });

            modelBuilder.Entity("EmeraldBot.Model.Characters.PC", b =>
                {
                    b.HasBaseType("EmeraldBot.Model.Characters.Character");

                    b.Property<int?>("PlayerID");

                    b.HasIndex("PlayerID");

                    b.ToTable("PlayerCharacters");

                    b.HasDiscriminator().HasValue("PC");
                });

            modelBuilder.Entity("EmeraldBot.Model.Characters.Character", b =>
                {
                    b.HasOne("EmeraldBot.Model.Servers.Server", "Server")
                        .WithMany("Characters")
                        .HasForeignKey("ServerID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("EmeraldBot.Model.Identity.RoleClaim", b =>
                {
                    b.HasOne("EmeraldBot.Model.Identity.Role", "Role")
                        .WithMany("Claims")
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EmeraldBot.Model.Identity.UserClaim", b =>
                {
                    b.HasOne("EmeraldBot.Model.Identity.User", "User")
                        .WithMany("Claims")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EmeraldBot.Model.Identity.UserRole", b =>
                {
                    b.HasOne("EmeraldBot.Model.Identity.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EmeraldBot.Model.Servers.Server", "Server")
                        .WithMany("Roles")
                        .HasForeignKey("ServerID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EmeraldBot.Model.Identity.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("EmeraldBot.Model.Identity.UserToken", b =>
                {
                    b.HasOne("EmeraldBot.Model.Identity.User", "User")
                        .WithMany("Tokens")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EmeraldBot.Model.Servers.DefaultCharacter", b =>
                {
                    b.HasOne("EmeraldBot.Model.Characters.Character", "Character")
                        .WithMany()
                        .HasForeignKey("CharacterID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EmeraldBot.Model.Identity.User", "Player")
                        .WithMany("DefaultCharacters")
                        .HasForeignKey("PlayerID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EmeraldBot.Model.Servers.Server", "Server")
                        .WithMany("DefaultCharacters")
                        .HasForeignKey("ServerID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("EmeraldBot.Model.Servers.Message", b =>
                {
                    b.HasOne("EmeraldBot.Model.Identity.User", "Player")
                        .WithMany("Messages")
                        .HasForeignKey("PlayerID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EmeraldBot.Model.Servers.Server", "Server")
                        .WithMany()
                        .HasForeignKey("ServerID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EmeraldBot.Model.Servers.PrivateChannel", b =>
                {
                    b.HasOne("EmeraldBot.Model.Identity.User", "Player")
                        .WithMany("PrivateChannels")
                        .HasForeignKey("PlayerID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EmeraldBot.Model.Servers.Server", "Server")
                        .WithMany()
                        .HasForeignKey("ServerID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("EmeraldBot.Model.Characters.PC", b =>
                {
                    b.HasOne("EmeraldBot.Model.Identity.User", "Player")
                        .WithMany("Characters")
                        .HasForeignKey("PlayerID")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
