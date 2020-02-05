using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using Z.EntityFramework.Classic;

namespace ConsoleApp31ConcatTypes
{
    public class TestContext : DbContext
    {

        public static string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EFClassic;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public TestContext(): base(ConnectionString)
        {
            this.Database.Log = Console.Write;
        }

        public virtual DbSet<TestTypesEntity> TestTypes { get; set; }

        //// https://stackoverflow.com/questions/46212704/how-do-i-write-ef-functions-extension-method
        //public static bool Like(string matchExpression, string pattern) => EF.Functions.Like(matchExpression, pattern);
    }
}
