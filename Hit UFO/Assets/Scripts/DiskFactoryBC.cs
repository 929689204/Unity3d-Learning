using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UFO;

namespace UFO
{
    public class DiskFactory : System.Object
    {
        private static DiskFactory _instance;
        private static List<GameObject> diskList;
        public GameObject diskTemplate;
        // 飞碟工厂的实例化
        public static DiskFactory getInstance()
        {
            if (_instance == null)
            {
                _instance = new DiskFactory();
                diskList = new List<GameObject>();
            }
            return _instance;
        }
        // 获取一个空闲飞盘的id
        public int getDisk()
        {
            for (int i = 0; i < diskList.Count; ++i)
                if (!diskList[i].activeInHierarchy) return i;
            // 链表里没有空闲飞盘，则实例化创建一个预设的
            diskList.Add(GameObject.Instantiate(diskTemplate) as GameObject);
            return diskList.Count - 1;
        }

        public GameObject getDiskObject(int id)
        {
            return (id > -1 && id < diskList.Count) ? diskList[id] : null;
        }
        // 回收一个飞碟，重置其属性
        public void free(int id)
        {
            if (id > -1 && id < diskList.Count)
            {
                
                diskList[id].transform.localScale = diskTemplate.transform.localScale;
                diskList[id].SetActive(false);
            }
        }
    }
}

public class DiskFactoryBC : MonoBehaviour
{
    public GameObject disk;
    void Awake()
    {
        DiskFactory.getInstance().diskTemplate = disk;
    }
}
