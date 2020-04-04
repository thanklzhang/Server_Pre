using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Config
{
    public class ConfigData
    {
        public int SN;
    }



    public class ConfigManager
    {
        private static ConfigManager instance;

        public static ConfigManager Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = new ConfigManager();
                    //instance.LoadConfig();
                }
                return instance;
            }

        }
        //Dictionary<Type, List<ConfigData>> configDic = new Dictionary<Type, List<ConfigData>>();
        Dictionary<string, IList> configDic = new Dictionary<string, IList>();




        public void LoadConfig()
        {
            if (null == configDic || 0 == configDic.Count)
            {
                //这里是通过表来进行读取
                configDic = ConfigDataLoader.Instance.LoadFromFileByAutoClass();
            }


        }

        public T GetBySN<T>(int SN) where T : ConfigData
        {
            var data = (configDic[typeof(T).Name]).Cast<T>().Where(t => t.SN == SN);
            if (null == data || 0 == data.Count())
                throw new Exception("cant find the SN : " + SN + " the type : " + data.GetType().ToString());

            return (T)(data.First());
        }


        public List<T> GetAll<T>() where T : ConfigData
        {
            var data = (configDic[typeof(T).Name]);
            return data.Cast<T>().ToList();//Select(d => (T)d).ToList();
        }



    }

}

