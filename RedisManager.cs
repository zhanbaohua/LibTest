﻿using ServiceStack.Redis;
using System.Linq;

namespace RedisProject.Redis
{
    public class RedisManager
    {
        /// <summary>
        /// redis配置文件信息
        /// </summary>
        private static RedisConfig RedisConfig = RedisConfig.GetConfig();

        private static PooledRedisClientManager prcm;

        /// <summary>
        /// 静态构造方法，初始化链接池管理对象
        /// </summary>
        static RedisManager()
        {
            CreateManager();
        }

        /// <summary>
        /// 创建链接池管理对象
        /// </summary>
        private static void CreateManager()
        {
            string[] WriteServerConStr = SplitString(RedisConfig.WriteServerConStr, ",");
            string[] ReadServerConStr = SplitString(RedisConfig.ReadServerConStr, ",");
            prcm = new PooledRedisClientManager(ReadServerConStr, WriteServerConStr,
                             new RedisClientManagerConfig
                             {
                                 MaxWritePoolSize = 5,
                                 MaxReadPoolSize = 5,
                                 AutoStart = true
                             });
        }

        private static string[] SplitString(string strSource, string split)
        {
            return strSource.Split(split.ToArray());
        }
        /// <summary>
        /// 客户端缓存操作对象
        /// </summary>
        public static IRedisClient GetClient()
        {
            if (prcm == null)
                CreateManager();
            return prcm.GetClient();
        }

    }
}
