using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UFO;

public class PhysicsActionManager : MonoBehaviour
{
    private bool shooting;
    public bool isShooting()
    {
        return shooting;
    }

    private List<GameObject> disks = new List<GameObject>();    // 存储飞碟对象的链表
    private List<int> diskIds = new List<int>();                // 存储每一个飞碟id的链表
    Vector3 start;   //起点
    Vector3 target;   //要到达的目标  
    Vector3 speed;    //分解速度
    float countTime;
    Vector3 Gravity;
    private int level;


    private SceneController scene;

    void Awake()
    {
        scene = SceneController.getInstance();
        scene.setGameModel(this);
    }
    // 为当前飞碟设置属性
    public void setting(int scale, Color color, Vector3 emitPos, Vector3 emitDir, float speed, int num)
    {
        diskScale = scale;
        diskColor = color;
        float mainSpeed = speed;
        float time = Vector3.Distance(emitDirection, emitPosition) / mainSpeed;
        emitPosition = emitPos;
        emitDirection = emitDir;
        emitSpeed = new Vector3((emitDirection.x - emitPosition.x) / time,(emitDirection.y - emitPosition.y) / time + 5 * Gravity.y);
        Gravity = Vector3.zero;
        emitNumber = num;
    }

    public void prepareToPlayDisk()
    {
        if (!shooting) emitEnable = true;
    }
    // 发射飞碟，注意最后为飞碟添加了刚体属性，如此飞碟在飞出后还会在发射方向上受到随机大小的力的影响
    void playDisk()
    {
        for (int i = 0; i < emitNumber; ++i)
        {
            diskIds.Add(DiskFactory.getInstance().getDisk());
            disks.Add(DiskFactory.getInstance().getDiskObject(diskIds[i]));
            disks[i].transform.localScale *= diskScale;
            disks[i].GetComponent<Renderer>().material.color = diskColor;
            disks[i].transform.position = new Vector3(emitPosition.x, emitPosition.y + i, emitPosition.z);
            disks[i].SetActive(true);
            
        }
    }
    // 飞盘的回收，两个链表移除其信息
    void freeADisk(int i)
    {
        DiskFactory.getInstance().free(diskIds[i]);
        disks.RemoveAt(i);
        diskIds.RemoveAt(i);
    }

    void FixedUpdate()
    {
        if (emitEnable)
        {
            playDisk();
            emitEnable = false;
            shooting = true;
        }
    }
    // 游戏检测是否打中或者错失飞碟
    void Update()
    {
        float g = -10;
        Gravity.y = g * (countTime += Time.fixedDeltaTime);
        this.transform.position += (emitSpeed + Gravity) * Time.fixedDeltaTime;
        for (int i = 0; i < disks.Count; i++)
        {
            // score
            if (!disks[i].activeInHierarchy)
            {
                scene.getJudge().hitADisk();
                freeADisk(i);
            }
            // miss
            else if (disks[i].transform.position.y < 0)
            {
                scene.getJudge().loseADisk();
                freeADisk(i);
            }
        }
        if (disks.Count == 0)
            shooting = false;
    }
}
