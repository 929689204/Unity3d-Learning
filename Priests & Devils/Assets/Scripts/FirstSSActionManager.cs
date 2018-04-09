using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISSActionCallback
{
    void actionDone(SSAction source);
}

/**
 * SSAction是动作的基类，保存要移动的游戏对象属性
 */
public class SSAction : ScriptableObject
{

    public bool enable = true;
    public bool destroy = false;

    public GameObject gameObject { get; set; }
    public Transform transform { get; set; }
    public ISSActionCallback callback { get; set; }

    public virtual void Start()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Update()
    {
        throw new System.NotImplementedException();
    }
}

/**
 * MoveToAction是SSAction的一个子类，代表平移的动作。 
 */
public class MoveToAction : SSAction
{
    public Vector3 target;
    public float speed;
    public static MoveToAction getAction(Vector3 target, float speed)
    {
        MoveToAction action = ScriptableObject.CreateInstance<MoveToAction>();
        action.target = target;
        action.speed = speed;
        return action;
    }

    public override void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
        if (this.transform.position == target)
        {
            this.destroy = true;
            this.callback.actionDone(this);
        }
    }

    public override void Start() { }

}

/**
 * SequenceAction是SSAction的另一个子类，它代表一系列组合动作。
 */
public class SequenceAction : SSAction, ISSActionCallback
{
    public List<SSAction> sequence;
    public int repeat = -1; //-1表示无限循环，0表示只执行一遍，repeat> 0 表示重复repeat遍
    public int currentAction = 0;//当前动作列表里，执行到的动作序号

    public static SequenceAction getAction(int repeat, int currentActionIndex, List<SSAction> sequence)
    {
        SequenceAction action = ScriptableObject.CreateInstance<SequenceAction>();
        action.sequence = sequence;
        action.repeat = repeat;
        action.currentAction = currentActionIndex;
        return action;
    }

    public override void Update()
    {
        if (sequence.Count == 0) return;
        if (currentAction < sequence.Count)
        {
            sequence[currentAction].Update();
        }
    }

    public void actionDone(SSAction source)
    {
        source.destroy = false;
        this.currentAction++;
        if (this.currentAction >= sequence.Count)
        {
            this.currentAction = 0;
            if (repeat > 0) repeat--;
            if (repeat == 0)
            {
                this.destroy = true;
                this.callback.actionDone(this);
            }
        }
    }
    public override void Start()
    {
        foreach (SSAction action in sequence)
        {
            action.gameObject = this.gameObject;
            action.transform = this.transform;
            action.callback = this;
            action.Start();
        }
    }
    void OnDestroy()
    {
        foreach (SSAction action in sequence)
        {
            DestroyObject(action);
        }
    }
}
/**
 * SSActionManager统筹上面三个动作类
 */
public class SSActionManager : MonoBehaviour
{
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    private List<SSAction> waitingToAdd = new List<SSAction>();
    private List<int> watingToDelete = new List<int>();

    protected void Update()
    {
        foreach (SSAction ac in waitingToAdd)
        {
            actions[ac.GetInstanceID()] = ac;
        }
        waitingToAdd.Clear();

        foreach (KeyValuePair<int, SSAction> kv in actions)
        {
            SSAction ac = kv.Value;
            if (ac.destroy)
            {
                watingToDelete.Add(ac.GetInstanceID());
            }
            else if (ac.enable)
            {
                ac.Update();
            }
        }

        foreach (int key in watingToDelete)
        {
            SSAction ac = actions[key];
            actions.Remove(key);
            DestroyObject(ac);
        }
        watingToDelete.Clear();
    }

    public void RunAction(GameObject gameObject, SSAction action, ISSActionCallback whoToNotify)
    {
        action.gameObject = gameObject;
        action.transform = gameObject.transform;
        action.callback = whoToNotify;
        waitingToAdd.Add(action);
        action.Start();
    }

}

/**
 * 专门设计动作并执行
 */
public class FirstSSActionManager : SSActionManager, ISSActionCallback
{
    public FirstSceneController scene;
    public MoveToAction action1, action2;
    public SequenceAction saction;
    float speed = 30f;

    /**
     * 为小船设置水平移动动作
     */
    public void moveBoat(GameObject boat)
    {
        action1 = MoveToAction.getAction((boat.transform.position == new Vector3(4, 0, 0) ? new Vector3(-4, 0, 0) : new Vector3(4, 0, 0)), speed);
        this.RunAction(boat, action1, this);
    }

    /**
     * 人的上船动作
     * 为人设置水平和垂直两个分解动作，然后组合起来执行
     */
    public void getOnBoat(GameObject people, int shore, int seat)
    {
        if (shore == 0 && seat == 0)
        {
            action1 = MoveToAction.getAction(new Vector3(-5f, 2.7f, 0), speed);//右移
            action2 = MoveToAction.getAction(new Vector3(-5f, 1.2f, 0), speed);//下移
        }
        else if (shore == 0 && seat == 1)
        {
            action1 = MoveToAction.getAction(new Vector3(-3f, 2.7f, 0), speed);
            action2 = MoveToAction.getAction(new Vector3(-3f, 1.2f, 0), speed);
        }
        else if (shore == 1 && seat == 0)
        {
            action1 = MoveToAction.getAction(new Vector3(3f, 2.7f, 0), speed);
            action2 = MoveToAction.getAction(new Vector3(3f, 1.2f, 0), speed);
        }
        else if (shore == 1 && seat == 1)
        {

            action1 = MoveToAction.getAction(new Vector3(5f, 2.7f, 0), speed);
            action2 = MoveToAction.getAction(new Vector3(5f, 1.2f, 0), speed);
        }

        SequenceAction saction = SequenceAction.getAction(0, 0, new List<SSAction> { action1, action2 });//组合动作
        this.RunAction(people, saction, this);
    }

    /**
     * 人的下船动作
     */
    public void getOffBoat(GameObject people, int shoreNum)
    {
        action1 = MoveToAction.getAction(new Vector3(people.transform.position.x, 2.7f, 0), speed);//上移

        if (shoreNum == 0) action2 = MoveToAction.getAction(new Vector3(-16f + 1.5f * Convert.ToInt32(people.name), 2.7f, 0), speed);//左移
        else action2 = MoveToAction.getAction(new Vector3(16f - 1.5f * Convert.ToInt32(people.name), 2.7f, 0), speed);//右移

        SequenceAction saction = SequenceAction.getAction(0, 0, new List<SSAction> { action1, action2 });//组合动作
        this.RunAction(people, saction, this);
    }

    protected void Start()
    {
        scene = (FirstSceneController)SSDirector.getInstance().currentScenceController;
        scene.actionManager = this;
    }

    protected new void Update()
    {
        base.Update();
    }

    public void actionDone(SSAction source)
    {
        Debug.Log("Done");
    }
}
