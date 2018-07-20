using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickUI : MonoBehaviour {
    ICharacterController characterController;
    IUserAction action;

	void Start() {
		action = SSDirector.GetInstance().currentSceneController as IUserAction;
	}

    public void SetController(ICharacterController characterController) {
        this.characterController = characterController;
    }

    void OnMouseDown() {
		if (gameObject.name == "boat")
			action.MoveBoat(gameObject);
		else
			action.CharacterIsClicked(characterController, gameObject);
	}
}