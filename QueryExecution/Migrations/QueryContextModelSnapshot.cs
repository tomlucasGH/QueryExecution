using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using QueryExecution.Classes;

namespace QueryExecution.Migrations
{
    [DbContext(typeof(QueryContext))]
    partial class QueryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("QueryExecution.Classes.DataConnections", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConnectionString");

                    b.Property<string>("DataSourceType");

                    b.Property<string>("Name");

                    b.HasKey("id");

                    b.ToTable("dataconnections");
                });

            modelBuilder.Entity("QueryExecution.Classes.DataSourceTypes", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DataSourceType");

                    b.Property<string>("imagelocation");

                    b.HasKey("id");

                    b.ToTable("datasources");
                });
        }
    }
}
