// <auto-generated />
using System;
using AvatarBot.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AvatarBot.Model.Migrations
{
    [DbContext(typeof(AvatarBotContext))]
    [Migration("20210420135140_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0-preview6.19304.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AvatarBot.Model.Characters.Character", b =>
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

            modelBuilder.Entity("AvatarBot.Model.Identity.Role", b =>
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

            modelBuilder.Entity("AvatarBot.Model.Identity.RoleClaim", b =>
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

            modelBuilder.Entity("AvatarBot.Model.Identity.User", b =>
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

            modelBuilder.Entity("AvatarBot.Model.Identity.UserClaim", b =>
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

            modelBuilder.Entity("AvatarBot.Model.Identity.UserRole", b =>
                {
                    b.Property<int>("UserID");

                    b.Property<int>("RoleID");

                    b.Property<int?>("ServerID");

                    b.HasKey("UserID", "RoleID");

                    b.HasIndex("RoleID");

                    b.HasIndex("ServerID");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("AvatarBot.Model.Identity.UserToken", b =>
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

            modelBuilder.Entity("AvatarBot.Model.Servers.DefaultCharacter", b =>
                {
                    b.Property<int>("ServerID");

                    b.Property<int>("PlayerID");

                    b.Property<int?>("CharacterID");

                    b.HasKey("ServerID", "PlayerID");

                    b.HasIndex("CharacterID");

                    b.HasIndex("PlayerID");

                    b.ToTable("DefaultCharacter");
                });

            modelBuilder.Entity("AvatarBot.Model.Servers.Message", b =>
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

            modelBuilder.Entity("AvatarBot.Model.Servers.PrivateChannel", b =>
                {
                    b.Property<int>("PlayerID");

                    b.Property<int>("ServerID");

                    b.Property<long>("ChannelDiscordID");

                    b.HasKey("PlayerID", "ServerID");

                    b.HasIndex("ServerID");

                    b.ToTable("PrivateChannels");
                });

            modelBuilder.Entity("AvatarBot.Model.Servers.Server", b =>
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

            modelBuilder.Entity("AvatarBot.Model.Characters.NPC", b =>
                {
                    b.HasBaseType("AvatarBot.Model.Characters.Character");

                    b.ToTable("NonPlayerCharacters");

                    b.HasDiscriminator().HasValue("NPC");
                });

            modelBuilder.Entity("AvatarBot.Model.Characters.PC", b =>
                {
                    b.HasBaseType("AvatarBot.Model.Characters.Character");

                    b.Property<int?>("PlayerID");

                    b.HasIndex("PlayerID");

                    b.ToTable("PlayerCharacters");

                    b.HasDiscriminator().HasValue("PC");
                });

            modelBuilder.Entity("AvatarBot.Model.Characters.Character", b =>
                {
                    b.HasOne("AvatarBot.Model.Servers.Server", "Server")
                        .WithMany("Characters")
                        .HasForeignKey("ServerID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("AvatarBot.Model.Identity.RoleClaim", b =>
                {
                    b.HasOne("AvatarBot.Model.Identity.Role", "Role")
                        .WithMany("Claims")
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("AvatarBot.Model.Identity.UserClaim", b =>
                {
                    b.HasOne("AvatarBot.Model.Identity.User", "User")
                        .WithMany("Claims")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("AvatarBot.Model.Identity.UserRole", b =>
                {
                    b.HasOne("AvatarBot.Model.Identity.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("AvatarBot.Model.Servers.Server", "Server")
                        .WithMany("Roles")
                        .HasForeignKey("ServerID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("AvatarBot.Model.Identity.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("AvatarBot.Model.Identity.UserToken", b =>
                {
                    b.HasOne("AvatarBot.Model.Identity.User", "User")
                        .WithMany("Tokens")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("AvatarBot.Model.Servers.DefaultCharacter", b =>
                {
                    b.HasOne("AvatarBot.Model.Characters.Character", "Character")
                        .WithMany()
                        .HasForeignKey("CharacterID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("AvatarBot.Model.Identity.User", "Player")
                        .WithMany("DefaultCharacters")
                        .HasForeignKey("PlayerID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("AvatarBot.Model.Servers.Server", "Server")
                        .WithMany("DefaultCharacters")
                        .HasForeignKey("ServerID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("AvatarBot.Model.Servers.Message", b =>
                {
                    b.HasOne("AvatarBot.Model.Identity.User", "Player")
                        .WithMany("Messages")
                        .HasForeignKey("PlayerID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("AvatarBot.Model.Servers.Server", "Server")
                        .WithMany()
                        .HasForeignKey("ServerID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("AvatarBot.Model.Servers.PrivateChannel", b =>
                {
                    b.HasOne("AvatarBot.Model.Identity.User", "Player")
                        .WithMany("PrivateChannels")
                        .HasForeignKey("PlayerID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("AvatarBot.Model.Servers.Server", "Server")
                        .WithMany()
                        .HasForeignKey("ServerID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("AvatarBot.Model.Characters.PC", b =>
                {
                    b.HasOne("AvatarBot.Model.Identity.User", "Player")
                        .WithMany("Characters")
                        .HasForeignKey("PlayerID")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
