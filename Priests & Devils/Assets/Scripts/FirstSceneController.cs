using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 场记的接口，每个场记都要实现
 * 加载场景资源、场景的暂停、恢复和重新布置
 */
public interface ISceneController
{
    void LoadResources();
    void Pause();
    void Resume();
    void Restart();
}

public class FirstSceneController : MonoBehaviour, IUserAction, ISceneController
{

    public FirstSSActionManager actionManager;

    List<GameObject> LeftObjList = new List<GameObject>();//存储在左岸的对象
    List<GameObject> RightObjList = new List<GameObject>();//存储在右岸的对象
    GameObject[] boat = new GameObject[2];

    GameObject boat_obj, leftShore_obj, rightShore_obj;

    Vector3 LeftShorePos = new Vector3(-12, 0, 0);
    Vector3 RightShorePos = new Vector3(12, 0, 0);
    Vector3 BoatLeftPos = new Vector3(-4, 0, 0);
    Vector3 BoatRightPos = new Vector3(4, 0, 0);

    void Awake()
    {
        SSDirector director = SSDirector.getInstance();
        director.setFPS(60);
        director.currentScenceController = this;
        director.currentScenceController.LoadResources();
        director.leaveSeconds = director.totalSeconds;

    }

    void Start()
    {

        SSDirector.getInstance().state = State.PAUSE;
        SSDirector.getInstance().countDownTitle = "Start";
        actionManager = GetComponent<FirstSSActionManager>() as FirstSSActionManager;
    }

    void Update()
    {
        Judge();
    }

    public void LoadResources()
    {
        GameObject priest_obj, devil_obj;
        Camera.main.transform.position = new Vector3(0, 0, -20);

        //加载对象
        leftShore_obj = Instantiate(Resources.Load("Prefabs/Shore"), LeftShorePos, Quaternion.identity) as GameObject;
        rightShore_obj = Instantiate(Resources.Load("Prefabs/Shore"), RightShorePos, Quaternion.identity) as GameObject;
        leftShore_obj.name = "left_shore";
        rightShore_obj.name = "right_shore";

        boat_obj = Instantiate(Resources.Load("Prefabs/Boat"), BoatLeftPos, Quaternion.identity) as GameObject;
        boat_obj.name = "boat";
        boat_obj.transform.parent = leftShore_obj.transform;
        //在左岸加载牧师恶魔并编号
        for (int i = 0; i < 3; ++i)
        {
            priest_obj = Instantiate(Resources.Load("Prefabs/Priest")) as GameObject;
            priest_obj.name = i.ToString();//牧师编号0 1 2
            priest_obj.transform.position = new Vector3(-16f + 1.5f * Convert.ToInt32(priest_obj.name), 2.7f, 0);
            priest_obj.transform.parent = leftShore_obj.transform;
            LeftObjList.Add(priest_obj);

            devil_obj = Instantiate(Resources.Load("Prefabs/Devil")) as GameObject;
            devil_obj.name = (i + 3).ToString();//魔鬼编号3 4 5
            devil_obj.transform.position = new Vector3(-16f + 1.5f * Convert.ToInt32(devil_obj.name), 2.7f, 0);
            devil_obj.transform.parent = leftShore_obj.transform;
            LeftObjList.Add(devil_obj);
        }
        Instantiate(Resources.Load("Prefabs/Light"));
    }

    /**
     * 实现用户动作接口中的点击对象函数
     * 利用射线获取对象
     *   对象名字 0—2代表牧师
     *            3—5代表魔鬼
     *            boat代表船
     * 其余情况不处理
     */
    public void clickOne()
    {
        GameObject gameObj = null;

        if (Input.GetMouseButtonDown(0) &&
            (SSDirector.getInstance().state == State.START || SSDirector.getInstance().state == State.CONTINUE))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                gameObj = hit.transform.gameObject;
            }
        }

        if (gameObj == null) return;
        else if (gameObj.name == "0" || gameObj.name == "1" || gameObj.name == "2"
            || gameObj.name == "3" || gameObj.name == "4" || gameObj.name == "5")
        {
            MovePeople(gameObj);
        }
        else if (gameObj.name == "boat")
        {
            MoveBoat();
        }
    }

    /**
     * 人的移动分四种情况
     * 上船：左岸上左船，右岸上右船
     * 上岸：左船上左岸、右船上右岸
     */
    void MovePeople(GameObject people)
    {
        int shoreNum, seatNum;//0为左，1为右，作为参数传给动作管理员

        if (people.transform.parent == boat_obj.transform.parent && (boat[0] == null || boat[1] == null))//物体和船都在同一个岸且船有空位才能上船
        {
            seatNum = boat[0] == null ? 0 : 1;
            if (people.transform.parent == leftShore_obj.transform)
            {
                shoreNum = 0;
                for (int i = 0; i < LeftObjList.Count; i++)
                {
                    if (people.name == LeftObjList[i].name)
                    {
                        actionManager.getOnBoat(people, shoreNum, seatNum);//让动作管理员执行上船动作
                        LeftObjList.Remove(LeftObjList[i]);
                    }
                }
            }
            else
            {
                shoreNum = 1;
                for (int i = 0; i < RightObjList.Count; i++)
                {
                    if (people.name == RightObjList[i].name)
                    {
                        actionManager.getOnBoat(people, shoreNum, seatNum);
                        RightObjList.Remove(RightObjList[i]);
                    }
                }
            }
            boat[seatNum] = people;
            people.transform.parent = boat_obj.transform;
        }
        else if (people.transform.parent == boat_obj.transform)//若物体在船上就选择上岸
        {
            shoreNum = boat_obj.transform.parent == leftShore_obj.transform ? 0 : 1;
            seatNum = (boat[0] != null && boat[0].name == people.name) ? 0 : 1;

            actionManager.getOffBoat(people, shoreNum);//动作管理员去执行上岸动作

            boat[seatNum] = null;
            if (shoreNum == 0)
            {
                people.transform.parent = leftShore_obj.transform;
                LeftObjList.Add(people);
            }
            else
            {
                people.transform.parent = rightShore_obj.transform;
                RightObjList.Add(people);
            }
        }
    }

    /**
     * 若船上有人，则船可驶向对岸
     * 让动作管理器去负责移动小船
     */
    void MoveBoat()
    {
        if (boat[0] != null || boat[1] != null)
        {
            actionManager.moveBoat(boat_obj);

            boat_obj.transform.parent = boat_obj.transform.parent == leftShore_obj.transform ?
                rightShore_obj.transform : leftShore_obj.transform;
        }
    }

    /**
     * 判断游戏输赢状态并告知导演
     * 计算分三步，先统计左岸的牧师、恶魔
     *             然后统计右岸的牧师、恶魔
     *             再判断船在左岸还是右岸，把船上恶魔与牧师加到对应岸的数量上
     *  若某一岸恶魔 > 牧师，则游戏失败
     *  若全到右岸，则胜利
     *  否则游戏继续
     */
    public void Judge()
    {
        int left_d = 0, left_p = 0, right_d = 0, right_p = 0;

        foreach (GameObject element in LeftObjList)
        {
            if (element.tag == "Priest") left_p++;
            if (element.tag == "Devil") left_d++;
        }

        foreach (GameObject element in RightObjList)
        {
            if (element.tag == "Priest") right_p++;
            if (element.tag == "Devil") right_d++;
        }

        for (int i = 0; i < 2; i++)
        {
            if (boat[i] != null && boat_obj.transform.parent == leftShore_obj.transform)//船在左岸
            {
                if (boat[i].tag == "Priest") left_p++;
                else left_d++;
            }
            if (boat[i] != null && boat_obj.transform.parent == rightShore_obj.transform)//船在右岸
            {
                if (boat[i].tag == "Priest") right_p++;
                else right_d++;
            }
        }

        if ((left_d > left_p && left_p != 0) || (right_d > right_p && right_p != 0) || SSDirector.getInstance().leaveSeconds == 0)
        {
            SSDirector.getInstance().state = State.LOSE;//恶魔多于牧师，lose
        }
        else if (right_d == right_p && right_d == 3)//全过河，win
        {
            SSDirector.getInstance().state = State.WIN;
        }
    }

    public void Pause()
    {
        SSDirector.getInstance().state = State.PAUSE;
    }

    public void Resume()
    {
        SSDirector.getInstance().state = State.CONTINUE;
    }

    public void Restart()
    {
        Application.LoadLevel(Application.loadedLevelName);
        SSDirector.getInstance().state = State.START;
    }
}