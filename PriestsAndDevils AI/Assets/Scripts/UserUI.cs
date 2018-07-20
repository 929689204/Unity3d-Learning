using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserUI : MonoBehaviour {
	private IUserAction action;
	public int status = 0;

	void Start() {
		action = SSDirector.GetInstance().currentSceneController as IUserAction;
	}

	void OnGUI() {
		if (status == 1 || status == 2) {
			if (status == 1)
				GUI.Label(new Rect(Screen.width/2, Screen.height/2-120, 100, 50), "您输了!");
			else
				GUI.Label(new Rect(Screen.width/2, Screen.height/2-120, 100, 50), "您赢了!");
			if (GUI.Button(new Rect(Screen.width/2 - 30, Screen.height/2-100, 100, 60), "重新开始")) {
				action.ReStart();
				status = 0;
			}
		}
        if (GUI.Button(new Rect(Screen.width / 2 - 30, Screen.height / 2 - 140, 100, 20), "Next"))
        {
            action.NextStep();
        }
    }
}