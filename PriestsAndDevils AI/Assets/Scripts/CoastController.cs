using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoastController
{
    readonly GameObject coast;
    readonly Vector3 from_pos = new Vector3(9, 1, 0);
    readonly Vector3 to_pos = new Vector3(-9, 1, 0);
    readonly Vector3[] positions;
    readonly int toOrFrom;  // to -1, from 1
    public ICharacterController[] passengerPlaner;

    public CoastController(string toOrFrom_str)
    {
        positions = new Vector3[] {new Vector3(6.5F,2.4F,0), new Vector3(7.5F,2.4F,0), new Vector3(8.5F,2.4F,0),
                new Vector3(9.5F,2.4F,0), new Vector3(10.5F,2.4F,0), new Vector3(11.5F,2.4F,0)};

        passengerPlaner = new ICharacterController[6];

        if (toOrFrom_str == "from")
        {
            coast = Object.Instantiate(Resources.Load("Perfabs/Coast", typeof(GameObject)), from_pos, Quaternion.identity, null) as GameObject;
            coast.name = "from";
            toOrFrom = 1;
        }
        else
        {
            coast = Object.Instantiate(Resources.Load("Perfabs/Coast", typeof(GameObject)), to_pos, Quaternion.identity, null) as GameObject;
            coast.name = "to";
            toOrFrom = -1;
        }
    }

    public int GetEmptyIndex()
    {
        for (int i = 0; i < passengerPlaner.Length; i++)
            if (passengerPlaner[i] == null)
                return i;
        return -1;
    }

    public Vector3 GetEmptyPosition()
    {
        Vector3 pos = positions[GetEmptyIndex()];
        pos.x *= toOrFrom;
        return pos;
    }

    public void GetOnCoast(ICharacterController characterCtrl)
    {
        int index = GetEmptyIndex();
        passengerPlaner[index] = characterCtrl;
    }

    public ICharacterController GetOffCoast(string passenger_name)
    {   // 0->priest, 1->devil
        for (int i = 0; i < passengerPlaner.Length; i++)
        {
            if (passengerPlaner[i] != null && passengerPlaner[i].GetName() == passenger_name)
            {
                ICharacterController charactorCtrl = passengerPlaner[i];
                passengerPlaner[i] = null;
                return charactorCtrl;
            }
        }
        return null;
    }

    public int GetToOrFrom()
    {
        return toOrFrom;
    }

    public int[] GetCharacterNum()
    {
        int[] count = { 0, 0 };
        for (int i = 0; i < passengerPlaner.Length; i++)
        {
            if (passengerPlaner[i] == null)
                continue;
            if (passengerPlaner[i].MyGetType() == 0) // priest 0, devil 1
                count[0]++;
            else
                count[1]++;
        }
        return count;
    }

    public void Reset()
    {
        passengerPlaner = new ICharacterController[6];
    }
}