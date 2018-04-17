using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFO;
namespace UFO
{
    public class DiskFactory : System.Object
    {
        private static DiskFactory _instance;
        private static List<GameObject> diskList; // 飞碟队列  
        public GameObject diskTemplate;

        public static DiskFactory getInstance()
        {
            if (_instance == null)
            {
                _instance = new DiskFactory();
                diskList = new List<GameObject>();
            }
            return _instance;
        }

        public int getDisk()
        {
            for (int i = 0; i < diskList.Count; ++i)
            {
                if (!diskList[i].activeInHierarchy)
                {
                    return i;   // 飞碟空闲  
                }
            }
            // 无空闲飞碟，则实例新的飞碟预设  
            diskList.Add(GameObject.Instantiate(diskTemplate) as GameObject);
            return diskList.Count - 1;
        }

        public GameObject getDiskObject(int id)
        {
            if (id > -1 && id < diskList.Count)
            {
                return diskList[id];
            }
            return null;
        }

        //回收
        public void free(int id)
        {
            if (id > -1 && id < diskList.Count)
            {
                // 重置飞碟速度  
                diskList[id].GetComponent<Rigidbody>().velocity = Vector3.zero;
                // 重置飞碟大小  
                diskList[id].transform.localScale = diskTemplate.transform.localScale;
                diskList[id].SetActive(false);
            }
        }
    }
}

public class DiskFactoryBC : MonoBehaviour
{
    public GameObject disk;

    private void Awake()
    {
        DiskFactory.getInstance().diskTemplate = disk;
    }
}

