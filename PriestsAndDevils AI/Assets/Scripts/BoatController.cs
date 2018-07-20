using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController
{
    public CCActionManager actionManager;

    public GameObject boat;
    //readonly Move moveableScript;
    readonly Vector3 fromPosition = new Vector3(4, 1, 0);
    //readonly Vector3 toPosition = new Vector3(-4, 1, 0);
    readonly Vector3[] fromPositions;
    readonly Vector3[] toPositions;
    public int toOrFrom; // to -1, from 1
    public ICharacterController[] passenger = new ICharacterController[2];

    public BoatController()
    {
        toOrFrom = 1;

        fromPositions = new Vector3[] { new Vector3(3.5F, 1.5F, 0), new Vector3(4.5F, 1.5F, 0) };
        toPositions = new Vector3[] { new Vector3(-4.5F, 1.5F, 0), new Vector3(-3.5F, 1.5F, 0) };

        boat = Object.Instantiate(Resources.Load("Perfabs/Boat", typeof(GameObject)), fromPosition, Quaternion.identity, null) as GameObject;
        boat.name = "boat";

        //moveableScript = boat.AddComponent(typeof(Move)) as Move;
        boat.AddComponent(typeof(ClickUI));
    }

    /*
    public void Move()
    {
        if (toOrFrom == -1)
        {
            moveableScript.SetDestination(fromPosition);
            toOrFrom = 1;
        }
        else
        {
            moveableScript.SetDestination(toPosition);
            toOrFrom = -1;
        }
    }*/

    public int GetEmptyIndex()
    {
        for (int i = 0; i < passenger.Length; i++)
            if (passenger[i] == null)
                return i;
        return -1;
    }

    public bool IsEmpty()
    {
        for (int i = 0; i < passenger.Length; i++)
            if (passenger[i] != null)
                return false;
        return true;
    }

    public Vector3 GetEmptyPosition()
    {
        Vector3 pos;
        int emptyIndex = GetEmptyIndex();
        if (toOrFrom == -1)
            pos = toPositions[emptyIndex];
        else
            pos = fromPositions[emptyIndex];
        return pos;
    }

    public void GetOnBoat(ICharacterController characterCtrl)
    {
        int index = GetEmptyIndex();
        passenger[index] = characterCtrl;
    }

    public ICharacterController GetOffBoat(string passenger_name)
    {
        for (int i = 0; i < passenger.Length; i++)
        {
            if (passenger[i] != null && passenger[i].GetName() == passenger_name)
            {
                ICharacterController charactorCtrl = passenger[i];
                passenger[i] = null;
                return charactorCtrl;
            }
        }
        return null;
    }

    public GameObject GetGameobj()
    {
        return boat;
    }

    public int Get_to_or_from()
    { // to->-1; from->1
        return toOrFrom;
    }

    public int[] GetCharacterNum()
    {
        int[] count = { 0, 0 };
        for (int i = 0; i < passenger.Length; i++)
        {
            if (passenger[i] == null)
                continue;
            if (passenger[i].MyGetType() == 0)  // 0 priest, 1 devil
                count[0]++;
            else
                count[1]++;
        }
        return count;
    }

    public void Reset()
    {
        //moveableScript.Reset();
        passenger = new ICharacterController[2];
    }
}