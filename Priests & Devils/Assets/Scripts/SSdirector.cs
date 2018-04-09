using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum State { WIN, LOSE, PAUSE, CONTINUE, START };
public class SSDirector : System.Object
{
    //计时器
    public int totalSeconds = 100;
    public int leaveSeconds;
    public bool onCountDown = false;
    public string countDownTitle = "Start";

    public State state { get; set; }
    public static SSDirector _instance;
    public ISceneController currentScenceController { get; set; }
    public static SSDirector getInstance()
    {
        if (_instance == null)
        {
            _instance = new SSDirector();
        }
        return _instance;
    }
    public void setFPS(int fps)
    {
        Application.targetFrameRate = fps;
    }
    public IEnumerator DoCountDown()
    {
        while (leaveSeconds > 0)
        {
            yield return new WaitForSeconds(1f);
            leaveSeconds--;
        }
    }
}
