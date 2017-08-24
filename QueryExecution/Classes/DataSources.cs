using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Data.Sqlite;

namespace QueryExecution.Classes
{
    public class DataSourceTypes
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int id { get; set; }
        public string DataSourceType { get; set; }

        public string imagelocation { get; set; }

    }
    public class DataConnections
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int id { get; set; }
        public string DataSourceType { get; set; }

        public string ConnectionString { get; set; }

        public string Name { get; set; }

    }
    public class DataTables
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }
        public int DataConnectionId { get; set; }
        public string TableName { get; set; }
        public int Selected { get; set; }
    }
    public class datasources
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }
        public string type { get; set; }
        public string image_uri { get; set; }
        public string name { get; set; }
    }
    public class QueryContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=DataSources.db");
        }

        public QueryContext() : base()
        {
             this.Database.EnsureCreated();
          }
        public DbSet<DataSourceTypes> datasources { get; set; }
        public DbSet<DataConnections> dataconnections { get; set; }

        public DbSet<DataTables> datatables { get; set; }


    }
}
