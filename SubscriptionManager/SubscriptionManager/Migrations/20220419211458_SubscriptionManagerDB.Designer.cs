// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SubscriptionManager.Data;

namespace SubscriptionManager.Migrations
{
    [DbContext(typeof(SubscriptionManagerContext))]
    [Migration("20220419211458_SubscriptionManagerDB")]
    partial class SubscriptionManagerDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.15")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SubscriptionManager.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Category");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "This category is used for music services",
                            Title = "Music"
                        },
                        new
                        {
                            Id = 2,
                            Description = "This category is used for movies services",
                            Title = "Movies"
                        },
                        new
                        {
                            Id = 3,
                            Description = "This category is used for job services",
                            Title = "Job"
                        },
                        new
                        {
                            Id = 4,
                            Description = "This category is used for different hobby services",
                            Title = "Hobby"
                        },
                        new
                        {
                            Id = 5,
                            Description = "This category is used for sport activities",
                            Title = "Sport"
                        });
                });

            modelBuilder.Entity("SubscriptionManager.Models.Subscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<int?>("_CategoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("_CategoryId");

                    b.ToTable("Subscription");
                });

            modelBuilder.Entity("SubscriptionManager.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("SubscriptionManager.Models.Subscription", b =>
                {
                    b.HasOne("SubscriptionManager.Models.User", null)
                        .WithMany("Subs")
                        .HasForeignKey("UserId");

                    b.HasOne("SubscriptionManager.Models.Category", "_Category")
                        .WithMany()
                        .HasForeignKey("_CategoryId");

                    b.Navigation("_Category");
                });

            modelBuilder.Entity("SubscriptionManager.Models.User", b =>
                {
                    b.Navigation("Subs");
                });
#pragma warning restore 612, 618
        }
    }
}
