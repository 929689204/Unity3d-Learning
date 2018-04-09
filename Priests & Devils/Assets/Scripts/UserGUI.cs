using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/**
 * 玩家动作接口，鼠标点击是唯一动作
 */
public interface IUserAction
{
    void clickOne();
}

public class UserGUI : MonoBehaviour
{
    private IUserAction action;
    float width, height;

    void Start()
    {
        action = SSDirector.getInstance().currentScenceController as IUserAction;
    }

    float castw(float scale)
    {
        return (Screen.width - width) / scale;
    }

    float casth(float scale)
    {
        return (Screen.height - height) / scale;
    }

    void OnGUI()
    {
        width = Screen.width / 12;
        height = Screen.height / 12;

        GUI.Label(new Rect(castw(2f) + 20, casth(6f) - 20, 50, 50), SSDirector.getInstance().leaveSeconds.ToString());// 倒计时秒数 //  


        if (SSDirector.getInstance().state != State.WIN && SSDirector.getInstance().state != State.LOSE
            && GUI.Button(new Rect(10, 10, 80, 30), SSDirector.getInstance().countDownTitle))
        {
            if (SSDirector.getInstance().countDownTitle == "Start")
            {

                SSDirector.getInstance().currentScenceController.Resume();
                SSDirector.getInstance().countDownTitle = "Pause";
                SSDirector.getInstance().onCountDown = true;
                StartCoroutine(SSDirector.getInstance().DoCountDown());
            }
            else
            {
                SSDirector.getInstance().currentScenceController.Pause();
                SSDirector.getInstance().countDownTitle = "Start";
                SSDirector.getInstance().onCountDown = false;
                StopAllCoroutines();
            }
        }

        if (SSDirector.getInstance().state == State.WIN)//胜利
        {
            StopAllCoroutines();
            if (GUI.Button(new Rect(castw(2f), casth(6f), Screen.width / 8, height), "Win!"))
            {
                SSDirector.getInstance().currentScenceController.Restart();
            }
        }
        else if (SSDirector.getInstance().state == State.LOSE)//失败
        {
            StopAllCoroutines();
            if (GUI.Button(new Rect(castw(2f), casth(6f), Screen.width / 7, height), "Lose!"))
            {
                SSDirector.getInstance().currentScenceController.Restart();
            }
        }
    }

    /**
     * 检测用户的点击动作
     */
    void Update()
    {
        action.clickOne();
    }

}
