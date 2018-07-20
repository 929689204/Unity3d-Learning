using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICharacterController
{
    public GameObject character;
    //readonly Move moveableScript;
    readonly ClickUI clickUI;
    readonly int characterType; // priest 0, devil 1
    bool isOnBoat;
    CoastController coastController;

    public ICharacterController(string which_character)
    {
        if (which_character == "priest")
        {
            character = Object.Instantiate(Resources.Load("Perfabs/Priest", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
            characterType = 0;
        }
        else
        {
            character = Object.Instantiate(Resources.Load("Perfabs/Devil", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
            characterType = 1;
        }
        //moveableScript = character.AddComponent(typeof(Move)) as Move;

        clickUI = character.AddComponent(typeof(ClickUI)) as ClickUI;
        clickUI.SetController(this);
    }

    public void SetName(string name)
    {
        character.name = name;
    }

    public void SetPosition(Vector3 pos)
    {
        character.transform.position = pos;
    }
    /*
    public void MoveToPosition(Vector3 destination)
    {
        moveableScript.SetDestination(destination);
    }*/

    public int MyGetType()
    {
        return characterType;
    }

    public string GetName()
    {
        return character.name;
    }

    public void GetOnBoat(BoatController boatCtrl)
    {
        coastController = null;
        character.transform.parent = boatCtrl.GetGameobj().transform;
        isOnBoat = true;
    }

    public void GetOnCoast(CoastController coastCtrl)
    {
        coastController = coastCtrl;
        character.transform.parent = null;
        isOnBoat = false;
    }

    public bool IsOnBoat()
    {
        return isOnBoat;
    }

    public CoastController GetCoastController()
    {
        return coastController;
    }

    public void Reset()
    {
        //moveableScript.Reset();
        coastController = (SSDirector.GetInstance().currentSceneController as FirstController).fromCoast;
        GetOnCoast(coastController);
        SetPosition(coastController.GetEmptyPosition());
        coastController.GetOnCoast(this);
    }
}