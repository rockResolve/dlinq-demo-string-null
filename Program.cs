using System;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp31ConcatTypes
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new TestContext();

            context.Database.Delete();
            context.Database.Create();

            if (!context.TestTypes.Any())
            {
                context.TestTypes.AddRange(Get9RowsData());
                context.SaveChanges();
            }
            
            Console.WriteLine(new string('-', 80));

            var xxx = context.TestTypes.Select("new ("
                + "String(null),"
                + "DateTimeOffset ? (null) AS DatetimeProp, "
                + "Decimal?(null) AS DecimalProp, "
                + "Int32?(null) AS Id,");

            var groupTotalQry = context.TestTypes
                .GroupBy("1", "it")

                .Select("new("
                + "1 AS __GrpLevelIdx,"

                + "String(null) AS StringProp,"
                + "\"Grand Totals\" AS TtTitle,"
                + "DateTimeOffset?(null) AS DatetimeProp,"

                + "Decimal?(null) AS DecimalProp, "
                + "Int32?(null) AS Id,"

                + "it.Count() AS __GrpCount)");

            //var group1ParentStringPropQry = context.TestTypes
            //    .GroupBy("new("
            //    + "StringProp"
            //    + ")", "it")

            //    .Select("new("
            //    + "1 AS __GrpLevelIdx,"

            //    + "it.Key.StringProp AS StringProp,"
            //    + "\"Level 1 Total\" AS TtTitle,"
            //    + "DateTimeOffset?(null) AS DatetimeProp,"

            //    + "Decimal?(null) AS DecimalProp, "
            //    + "Int32?(null) AS Id,"

            //    + "it.Count() AS __GrpCount)");

            //var group2ParentStringPropQry = context.TestTypes
            //    .GroupBy("new("
            //    + "StringProp,"
            //    + "DatetimeProp"
            //    + ")", "it")

            //    .Select("new("
            //    + "1 AS __GrpLevelIdx,"

            //    + "it.Key.StringProp AS StringProp,"
            //    + "\"Level 2 Total\" AS TtTitle,"
            //    + "it.Key.DatetimeProp AS DatetimeProp,"

            //    + "Decimal?(null) AS DecimalProp, "
            //    + "Int32?(null) AS Id,"

            //    + "it.Count() AS __GrpCount)");

            var itemsQry = context.TestTypes
                .Select("new("
                + "999 AS __GrpLevelIdx,"

                + "StringProp AS StringProp,"
                + "TtTitle,"
                + "DateTimeOffset?(DatetimeProp) AS DatetimeProp,"
                + "Decimal?(DecimalProp) AS DecimalProp,"
                + "Int32?(Id) AS Id,"

                + "0 AS __GrpCount)");

            Console.WriteLine(new string('-', 80));

            var heiracy = itemsQry
            //.Concat(group1ParentStringPropQry)
            //.Concat(group2ParentStringPropQry)
            .Concat(groupTotalQry)

            .OrderBy("StringProp, Id, __GrpLevelIdx")
            .ToDynamicList();

            //Row ids ordered <Total><Group P01> 6 7 8 <Group P02> 3 4 5 9 10 11

            Console.WriteLine(new string('-', 80));
            Console.WriteLine("");
            Console.WriteLine("Should sort 'Grand Totals line to the top. Requires String(null) to be calced as a null cast as a string, not a zero length string");
            Console.WriteLine("");

            foreach (dynamic x in heiracy)
            {
                Console.WriteLine($"StringProp, Id, GrpLevelIdx, TtTitle = {x.StringProp ?? "nULL"}, {x.Id ?? "-"}, {x.__GrpLevelIdx}, {x.TtTitle}");
            }

        }


        internal static List<TestTypesEntity> Get9RowsData()
        {
            return new List<TestTypesEntity>
            {
                new TestTypesEntity { TtId = 11, StringProp = null, IntProp = 1, DecimalProp = 9, DatetimeProp = DateTime.Now, TtTitle = "null group. Should be below Grand Totals" },
                new TestTypesEntity { TtId = 12, StringProp = null, IntProp = 1, DecimalProp = 9, DatetimeProp = DateTime.Now, TtTitle = "null group" },
                new TestTypesEntity { TtId = 13, StringProp = "", IntProp = 1, DecimalProp = 1, DatetimeProp = DateTime.Now, TtTitle = "zls group" },
                new TestTypesEntity { TtId = 14, StringProp = "", IntProp = 2, DecimalProp = 1, DatetimeProp = DateTime.Now, TtTitle = "zls group" },
                new TestTypesEntity { TtId = 15, StringProp = "Group 03", IntProp = 3, DecimalProp = 1, DatetimeProp = DateTime.Now, TtTitle = "group 3" },
                new TestTypesEntity { TtId = 16, StringProp = "Group 03", IntProp = 6, DecimalProp = 1, DatetimeProp = DateTime.Now, TtTitle = "group 3" },
            };
        }
    }
}
