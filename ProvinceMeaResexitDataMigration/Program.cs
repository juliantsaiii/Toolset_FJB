using CommonLibrary;
using System;

namespace ProvinceMeaResexitDataMigration
{
    /// <summary>
    /// 省里限制出境流数据迁移
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("开始迁移数据");
            LogHelper.DoNormalLog("开始迁移数据");

            var needMigrateList = Tool.GetNeedMigrate();
            if (needMigrateList == null || needMigrateList.Count == 0)
            {
                Console.WriteLine("本次没有需要迁移的数据");
                LogHelper.DoNormalLog("本次没有需要迁移的数据");
            }
            else
            {
                Console.WriteLine("本次一共需要迁移数据:" + needMigrateList.Count + "条数据");
                LogHelper.DoNormalLog("本次一共需要迁移数据:" + needMigrateList.Count + "条数据");

                Console.WriteLine("开始迁移");
                LogHelper.DoNormalLog("开始迁移");

                Tool.DataMigration(needMigrateList);

                Console.WriteLine("结束迁移");
                LogHelper.DoNormalLog("结束迁移");
            }
            
            Console.ReadKey();
        }

    }

}
