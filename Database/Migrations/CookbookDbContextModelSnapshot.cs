﻿// <auto-generated />
using System;
using KP.Cookbook.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KP.Cookbook.Database.Migrations
{
    [DbContext(typeof(CookbookDbContext))]
    partial class CookbookDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("KP.Cookbook.Domain.Models.Ingredient", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("KP.Cookbook.Domain.Models.Recipe", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CookingDuration")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("SourceId")
                        .HasColumnType("bigint");

                    b.Property<string>("Synopsis")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tags")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ThumbnailBase64")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SourceId");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("KP.Cookbook.Domain.Models.Source", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SiteUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Sources");
                });

            modelBuilder.Entity("KP.Cookbook.Domain.Models.Step", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("ImageBase64")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<long?>("RecipeId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.ToTable("Step");
                });

            modelBuilder.Entity("KP.Cookbook.Domain.Models.Recipe", b =>
                {
                    b.HasOne("KP.Cookbook.Domain.Models.Source", "Source")
                        .WithMany()
                        .HasForeignKey("SourceId");

                    b.OwnsMany("KP.Cookbook.Domain.Models.IngredientDetails", "IngredientDetails", b1 =>
                        {
                            b1.Property<long>("RecipeId")
                                .HasColumnType("bigint");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<decimal>("Amount")
                                .HasPrecision(8, 2)
                                .HasColumnType("decimal(8,2)");

                            b1.Property<long?>("IngredientId")
                                .HasColumnType("bigint");

                            b1.Property<int>("Measure")
                                .HasColumnType("int");

                            b1.Property<bool>("Optional")
                                .HasColumnType("bit");

                            b1.HasKey("RecipeId", "Id");

                            b1.HasIndex("IngredientId");

                            b1.ToTable("Recipes_IngredientDetails");

                            b1.HasOne("KP.Cookbook.Domain.Models.Ingredient", "Ingredient")
                                .WithMany()
                                .HasForeignKey("IngredientId");

                            b1.WithOwner()
                                .HasForeignKey("RecipeId");

                            b1.Navigation("Ingredient");
                        });

                    b.Navigation("IngredientDetails");

                    b.Navigation("Source");
                });

            modelBuilder.Entity("KP.Cookbook.Domain.Models.Step", b =>
                {
                    b.HasOne("KP.Cookbook.Domain.Models.Recipe", null)
                        .WithMany("Steps")
                        .HasForeignKey("RecipeId");

                    b.OwnsMany("KP.Cookbook.Domain.Models.IngredientDetails", "IngredientDetails", b1 =>
                        {
                            b1.Property<long>("StepId")
                                .HasColumnType("bigint");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<decimal>("Amount")
                                .HasPrecision(8, 2)
                                .HasColumnType("decimal(8,2)");

                            b1.Property<long?>("IngredientId")
                                .HasColumnType("bigint");

                            b1.Property<int>("Measure")
                                .HasColumnType("int");

                            b1.Property<bool>("Optional")
                                .HasColumnType("bit");

                            b1.HasKey("StepId", "Id");

                            b1.HasIndex("IngredientId");

                            b1.ToTable("Step_IngredientDetails");

                            b1.HasOne("KP.Cookbook.Domain.Models.Ingredient", "Ingredient")
                                .WithMany()
                                .HasForeignKey("IngredientId");

                            b1.WithOwner()
                                .HasForeignKey("StepId");

                            b1.Navigation("Ingredient");
                        });

                    b.Navigation("IngredientDetails");
                });

            modelBuilder.Entity("KP.Cookbook.Domain.Models.Recipe", b =>
                {
                    b.Navigation("Steps");
                });
#pragma warning restore 612, 618
        }
    }
}
