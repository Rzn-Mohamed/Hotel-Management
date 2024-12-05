﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Hotel_Management.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241205142343_ghita1")]
    partial class ghita1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Hotel_Management.Models.Client", b =>
                {
                    b.HasBaseType("User");

                    b.Property<bool>("isClient")
                        .HasColumnType("bit");

                    b.ToTable("Clients", (string)null);
                });

            modelBuilder.Entity("Hotel_Management.Models.Employee", b =>
                {
                    b.HasBaseType("User");

                    b.Property<bool>("isEmployee")
                        .HasColumnType("bit");

                    b.ToTable("Employees", (string)null);
                });

            modelBuilder.Entity("Hotel_Management.Models.Manager", b =>
                {
                    b.HasBaseType("User");

                    b.Property<bool>("isManager")
                        .HasColumnType("bit");

                    b.ToTable("Managers", (string)null);
                });

            modelBuilder.Entity("Hotel_Management.Models.Client", b =>
                {
                    b.HasOne("User", null)
                        .WithOne()
                        .HasForeignKey("Hotel_Management.Models.Client", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Hotel_Management.Models.Employee", b =>
                {
                    b.HasOne("User", null)
                        .WithOne()
                        .HasForeignKey("Hotel_Management.Models.Employee", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Hotel_Management.Models.Manager", b =>
                {
                    b.HasOne("User", null)
                        .WithOne()
                        .HasForeignKey("Hotel_Management.Models.Manager", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
