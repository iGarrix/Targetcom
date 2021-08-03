﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Targetcom.Data;

namespace Targetcom.Migrations
{
    [DbContext(typeof(TargetDbContext))]
    [Migration("20210803104737_Change key friendship")]
    partial class Changekeyfriendship
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Targetcom.Models.Friendship", b =>
                {
                    b.Property<string>("ProfileId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FriendId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FriendStatus")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProfileId");

                    b.HasIndex("FriendId");

                    b.ToTable("Friendships");
                });

            modelBuilder.Entity("Targetcom.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GameUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TargetPrice")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Targetcom.Models.LikedProfilePostage", b =>
                {
                    b.Property<string>("ProfileId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ProfilePostageId")
                        .HasColumnType("int");

                    b.HasKey("ProfileId", "ProfilePostageId");

                    b.HasIndex("ProfilePostageId");

                    b.ToTable("LikedProfilePostages");
                });

            modelBuilder.Entity("Targetcom.Models.ProfileGame", b =>
                {
                    b.Property<string>("ProfileId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProfileId", "GameId");

                    b.HasIndex("GameId");

                    b.ToTable("ProfileGames");
                });

            modelBuilder.Entity("Targetcom.Models.ProfilePostage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Alert")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsNessessaredCommentedPost")
                        .HasColumnType("bit");

                    b.Property<bool>("IsNessessaredLikedPost")
                        .HasColumnType("bit");

                    b.Property<bool>("IsNessessaredSharedPost")
                        .HasColumnType("bit");

                    b.Property<bool>("IsObject")
                        .HasColumnType("bit");

                    b.Property<string>("ProfileId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("WritterId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ProfileId");

                    b.HasIndex("WritterId");

                    b.ToTable("ProfilePostages");
                });

            modelBuilder.Entity("Targetcom.Models.ProfilePostageComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PostageId")
                        .HasColumnType("int");

                    b.Property<string>("ProfileCommentatorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PostageId");

                    b.HasIndex("ProfileCommentatorId");

                    b.ToTable("ProfilePostageComments");
                });

            modelBuilder.Entity("Targetcom.Models.SharedProfilePostage", b =>
                {
                    b.Property<string>("ProfileId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ProfilePostageId")
                        .HasColumnType("int");

                    b.HasKey("ProfileId", "ProfilePostageId");

                    b.HasIndex("ProfilePostageId");

                    b.ToTable("SharedProfilePostages");
                });

            modelBuilder.Entity("Targetcom.Models.Profile", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.Property<DateTime>("Age")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreateStamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Hobbies")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsNessessaredCommentedPost")
                        .HasColumnType("bit");

                    b.Property<bool>("IsNessessaredLikedPost")
                        .HasColumnType("bit");

                    b.Property<bool>("IsNessessaredPublishPost")
                        .HasColumnType("bit");

                    b.Property<bool>("IsNessessaredSharedPost")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPremium")
                        .HasColumnType("bit");

                    b.Property<bool>("IsShortDate")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVerify")
                        .HasColumnType("bit");

                    b.Property<string>("JobGeoplace")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Privacy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Quote")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Status")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("StudyGeoplace")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TargetCoins")
                        .HasColumnType("int");

                    b.Property<string>("UrlAvatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("VisibilityAboutMe")
                        .HasColumnType("bit");

                    b.Property<bool>("VisibilityCommerceData")
                        .HasColumnType("bit");

                    b.Property<bool>("VisibilityCommunity")
                        .HasColumnType("bit");

                    b.Property<bool>("VisibilityFriends")
                        .HasColumnType("bit");

                    b.Property<bool>("VisibilityImages")
                        .HasColumnType("bit");

                    b.Property<bool>("VisibilityPlaylist")
                        .HasColumnType("bit");

                    b.Property<bool>("VisibilityPostage")
                        .HasColumnType("bit");

                    b.Property<bool>("VisibilityQuote")
                        .HasColumnType("bit");

                    b.Property<bool>("VisibilityRole")
                        .HasColumnType("bit");

                    b.Property<bool>("VisibilitySharePostage")
                        .HasColumnType("bit");

                    b.Property<bool>("VisibilitySubscribers")
                        .HasColumnType("bit");

                    b.HasDiscriminator().HasValue("Profile");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Targetcom.Models.Friendship", b =>
                {
                    b.HasOne("Targetcom.Models.Profile", "Friend")
                        .WithMany()
                        .HasForeignKey("FriendId");

                    b.HasOne("Targetcom.Models.Profile", "Profile")
                        .WithMany("Friendships")
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Friend");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("Targetcom.Models.LikedProfilePostage", b =>
                {
                    b.HasOne("Targetcom.Models.Profile", "Profile")
                        .WithMany("LikedProfilePostages")
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Targetcom.Models.ProfilePostage", "Postage")
                        .WithMany("LikedProfiles")
                        .HasForeignKey("ProfilePostageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Postage");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("Targetcom.Models.ProfileGame", b =>
                {
                    b.HasOne("Targetcom.Models.Game", "Game")
                        .WithMany("ProfileGames")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Targetcom.Models.Profile", "Profile")
                        .WithMany("ProfileGames")
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("Targetcom.Models.ProfilePostage", b =>
                {
                    b.HasOne("Targetcom.Models.Profile", "Profile")
                        .WithMany("ProfilePostages")
                        .HasForeignKey("ProfileId");

                    b.HasOne("Targetcom.Models.Profile", "Writter")
                        .WithMany("WritterPostages")
                        .HasForeignKey("WritterId");

                    b.Navigation("Profile");

                    b.Navigation("Writter");
                });

            modelBuilder.Entity("Targetcom.Models.ProfilePostageComment", b =>
                {
                    b.HasOne("Targetcom.Models.ProfilePostage", "Postage")
                        .WithMany("ProfilePostageComments")
                        .HasForeignKey("PostageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Targetcom.Models.Profile", "ProfileCommentator")
                        .WithMany("ProfilePostageComments")
                        .HasForeignKey("ProfileCommentatorId");

                    b.Navigation("Postage");

                    b.Navigation("ProfileCommentator");
                });

            modelBuilder.Entity("Targetcom.Models.SharedProfilePostage", b =>
                {
                    b.HasOne("Targetcom.Models.Profile", "Profile")
                        .WithMany("SharedProfilePostages")
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Targetcom.Models.ProfilePostage", "Postage")
                        .WithMany("SharedProfiles")
                        .HasForeignKey("ProfilePostageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Postage");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("Targetcom.Models.Game", b =>
                {
                    b.Navigation("ProfileGames");
                });

            modelBuilder.Entity("Targetcom.Models.ProfilePostage", b =>
                {
                    b.Navigation("LikedProfiles");

                    b.Navigation("ProfilePostageComments");

                    b.Navigation("SharedProfiles");
                });

            modelBuilder.Entity("Targetcom.Models.Profile", b =>
                {
                    b.Navigation("Friendships");

                    b.Navigation("LikedProfilePostages");

                    b.Navigation("ProfileGames");

                    b.Navigation("ProfilePostageComments");

                    b.Navigation("ProfilePostages");

                    b.Navigation("SharedProfilePostages");

                    b.Navigation("WritterPostages");
                });
#pragma warning restore 612, 618
        }
    }
}
