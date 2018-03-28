using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tictactoe : MonoBehaviour
{
    //显示棋盘内容
    private string[] info = new string[9];
    //记录棋盘的状态，0没有按下，1X按下, 2O按下
    private int[] state = new int[9];
    //1是X的回合，0是O的回合
    private int turn = 0;
    private string result = "";
    private int i;
    public Texture buttonTexture;
    private int temp;
    private int temp1;
    void Start()
    {
        reset();
    }
    //初始化函数
    void reset()
    {
        for (i = 0; i < 9; i++)
        {
            info[i] = "";

            state[i] = 0;
        }
        turn = 0;
    }
    void xreset()
    {
        for (i = 0; i < 9; i++)
        {
            info[i] = "";

            state[i] = 0;
        }
        turn = 1;
    }
    void OnGUI()
    {
        int x, y, num;
        GUI.Label(new Rect(40, 320, 50, 50), result);
        if (GUI.Button(new Rect(200, 320, 100, 40), "O Start"))
        {
            reset();
            result = "";
        }
        if (GUI.Button(new Rect(100, 320, 100, 40), "X Start"))
        {
            xreset();
            result = "";
        }
        GUI.color = Color.yellow;
        GUI.backgroundColor = Color.white;
        if (check())
        {
            x = 20;
            y = 20;
            num = 0;
            //保留游戏结果的情况
            for (temp = 0; temp <= 2; temp++)
            {
                for (temp1 = 0; temp1 <= 2; temp1++)
                {
                    GUI.Button(new Rect(x, y, 80, 80), info[num]);

                    x += 80;
                    num++;
                }
                y += 80;
                x = 20;
            }

            if (GUI.Button(new Rect(200, 320, 100, 40), "O Start"))
            {
                reset();
                result = "";
            }
            if (GUI.Button(new Rect(100, 320, 100, 40), "X Start"))
            {
                xreset();
                result = "";
            }
            return;
        }

        //下棋
        x = 20;
        y = 20;
        num = 0;
        for (temp = 0; temp <= 2; temp++)
        {
            for (temp1 = 0; temp1 <= 2; temp1++)
            {
                if (GUI.Button(new Rect(x, y, 80, 80), info[num]))
                {
                    if (state[num] == 0 && turn == 0)
                    {
                        info[num] = "O";
                        state[num] = 2;

                    }
                    else if (state[num] == 0 && turn == 1)
                    {
                        info[num] = "X";
                        state[num] = 1;
                    }
                    if (turn == 0)
                    {
                        turn = 1;
                    }
                    else if (turn == 1)
                    {
                        turn = 0;
                    }
                }
                x += 80;
                num++;
            }
            y += 80;
            x = 20;
        }

    }
    //检查胜负
    bool check()
    {
        //检查横排
        int j = 0;
        for (j = 0; j <= 6; j = j + 3)
        {
            if (state[j] == state[j + 1] && state[j + 1] == state[j + 2])
            {
                if (state[j] == 1)
                {
                    result = "X wins";
                    return true;
                }
                else if (state[j] == 2)
                {
                    result = "O wins";
                    return true;
                }
            }
        }
        //检查竖排
        for (j = 0; j <= 2; j++)
        {
            if (state[j] == state[j + 3] && state[j + 3] == state[j + 6])
            {
                if (state[j] == 1)
                {
                    result = "X wins";
                    //print("x win");
                    return true;
                }
                else if (state[j] == 2)
                {
                    result = "O wins";
                    //print("O win");
                    return true;
                }
            }
        }
        //检查对角线
        if (state[0] == state[4] && state[4] == state[8])
        {
            if (state[0] == 1)
            {
                result = "X wins";
                return true;
            }
            else if (state[0] == 2)
            {
                result = "O wins";
                return true;
            }
        }
        if (state[2] == state[4] && state[4] == state[6])
        {
            if (state[2] == 1)
            {
                result = "X wins";
                return true;
            }
            else if (state[2] == 2)
            {
                result = "O wins";
                return true;
            }
        }
        //检查是否平局
        int flag = 0;
        for (j = 0; j < 9; j++)
        {
            if (state[j] == 0)
            {
                flag = 1;
                break;
            }

        }
        if (flag == 0)
        {
            result = "a draw";
            return true;
        }
        return false;
    }
}