using UnityEngine;
using System.Collections;
using UFO;

public class CountManager : MonoBehaviour
{
    // 设置得分和失分和通关分数
    public int oneDiskHit = 10;
    public int oneDiskLose = 5;
    public int nextroundscore = 40;

    private SceneController scene;

    void Awake()
    {
        scene = SceneController.getInstance();
        scene.setJudge(this);
    }
    // 默认从第一关开始
    void Start()
    {
        scene.nextRound();
    }
    // 打中，加分；够分通关
    public void hitADisk()
    {
        scene.setPoint(scene.getPoint() + oneDiskHit);
        if (scene.getPoint() >= nextroundscore)
            scene.nextRound();
    }
    // 飞碟坠落，扣分
    public void loseADisk()
    {
        scene.setPoint(scene.getPoint() - oneDiskLose);
    }
}
