using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Status { BMOVING, ISOVER, B}

public class FirstController : MonoBehaviour, ISceneController, IUserAction {
    public CCActionManager actionManager;
    public bool someObjectHandling;

    public CoastController fromCoast;
	public CoastController toCoast;
	public BoatController boat;
	private ICharacterController[] characters;
    readonly Vector3 riverPosition = new Vector3(0, 0.5F, 0);
    UserUI userUI;

    //for the boat
    private Vector3 fromPosition = new Vector3(4, 1, 0);
    private Vector3 toPosition = new Vector3(-4, 1, 0);

    private int FromPriestsCount()
    {
        int fromPriest = 0;

        int[] fromCount = fromCoast.GetCharacterNum();
        fromPriest += fromCount[0];

        int[] boatCount = boat.GetCharacterNum();
        if (boat.Get_to_or_from() == 1)
            fromPriest += boatCount[0];

        return fromPriest;
    }

    private int FromDevilsCount()
    {
        int fromDevil = 0;

        int[] fromCount = fromCoast.GetCharacterNum();
        fromDevil += fromCount[1];

        int[] boatCount = boat.GetCharacterNum();
        if (boat.Get_to_or_from() == 1)
            fromDevil += boatCount[1];

        return fromDevil;
    }

    void Awake() {
		SSDirector director = SSDirector.GetInstance();
        director.SetFPS(60);
		director.currentSceneController = this;
		userUI = gameObject.AddComponent<UserUI>() as UserUI;
		characters = new ICharacterController[6];
		LoadResources();
	}

    void Start()
    {
        actionManager = gameObject.AddComponent<CCActionManager>();
    }

    public void LoadResources() {
        someObjectHandling = false;

		GameObject river = Instantiate(Resources.Load("Perfabs/River", typeof(GameObject)), riverPosition, Quaternion.identity, null) as GameObject;
		river.name = "river";
		fromCoast = new CoastController("from");
		toCoast = new CoastController("to");
		boat = new BoatController();
		LoadCharacter();
	}

	private void LoadCharacter() {
		for (int i = 0; i < 3; i++) {
            ICharacterController character = new ICharacterController("priest");
            character.SetName("priest" + i);
            character.SetPosition(fromCoast.GetEmptyPosition());
            character.GetOnCoast(fromCoast);
			fromCoast.GetOnCoast(character);
			characters[i] = character;
		}

		for (int i = 0; i < 3; i++) {
            ICharacterController character = new ICharacterController("devil");
            character.SetName("devil" + i);
            character.SetPosition(fromCoast.GetEmptyPosition());
            character.GetOnCoast(fromCoast);
			fromCoast.GetOnCoast(character);
			characters[i + 3] = character;
		}
	}

	public void MoveBoat(GameObject clickedObject) {
        if (boat.IsEmpty())
        {
            someObjectHandling = false;
            return;
        }
        
        if (boat.toOrFrom == -1)
        {
            actionManager.MoveAction(clickedObject, fromPosition);
            boat.toOrFrom = 1;
        }
        else
        {
            actionManager.MoveAction(clickedObject, toPosition);
            boat.toOrFrom = -1;
        }    
        //boat.Move();
        userUI.status = CheckGameOver();
	}

	public void CharacterIsClicked(ICharacterController characterController, GameObject clickedObject) {
		if (characterController.IsOnBoat()) {
			CoastController whichCoast;
			if (boat.Get_to_or_from() == -1) // to -1; from 1
				whichCoast = toCoast;
			else
				whichCoast = fromCoast;
			boat.GetOffBoat(characterController.GetName());
            actionManager.MoveAction(clickedObject, whichCoast.GetEmptyPosition());
            //characterController.MoveToPosition (whichCoast.GetEmptyPosition());
            characterController.GetOnCoast(whichCoast);
			whichCoast.GetOnCoast(characterController);
		}
        else {
			CoastController whichCoast = characterController.GetCoastController();
			if (boat.GetEmptyIndex() == -1)
				return;
			if (whichCoast.GetToOrFrom() != boat.Get_to_or_from())
				return;
			whichCoast.GetOffCoast(characterController.GetName());
            actionManager.MoveAction(clickedObject, boat.GetEmptyPosition());
            //characterController.MoveToPosition (boat.GetEmptyPosition());
            characterController.GetOnBoat(boat);
			boat.GetOnBoat(characterController);
		}
		userUI.status = CheckGameOver();
	}

	int CheckGameOver() {	// playing 0, lose 1, win 2
		int fromPriest = 0;
		int fromDevil = 0;
		int toPriest = 0;
		int toDevil = 0;

		int[] fromCount = fromCoast.GetCharacterNum();
		fromPriest += fromCount[0];
		fromDevil += fromCount[1];

		int[] toCount = toCoast.GetCharacterNum();
		toPriest += toCount[0];
		toDevil += toCount[1];

		if (toPriest + toDevil == 6)
			return 2;
		int[] boatCount = boat.GetCharacterNum ();
		if (boat.Get_to_or_from () == -1) {
			toPriest += boatCount[0];
			toDevil += boatCount[1];
		}
        else {
			fromPriest += boatCount[0];
			fromDevil += boatCount[1];
		}
		if (fromPriest < fromDevil && fromPriest > 0)
			return 1;
		if (toPriest < toDevil && toPriest > 0)
			return 1;
		return 0;
	}

	public void ReStart() {
        if (boat.toOrFrom == -1)
        {
            actionManager.MoveAction(boat.boat, fromPosition);
            boat.toOrFrom = 1;
        }
        boat.Reset();
        fromCoast.Reset();
        toCoast.Reset();
        for (int i = 0; i < characters.Length; i++)
			characters [i].Reset();
    }


    /********************************** AI NEXT ********************************************/
    // 获取当前状态
    private AINext.Vertex GetCurState()
    {
        if (boat.Get_to_or_from() == 1 && FromPriestsCount() == 3 && FromDevilsCount() == 3)
        {
            return AINext.Vertex.P3D3B;     // 0
        }
        else if (boat.Get_to_or_from() == -1 && FromPriestsCount() == 2 && FromDevilsCount() == 2)
        {
            return AINext.Vertex.P2D2;      // 1
        }
        else if (boat.Get_to_or_from() == -1 && FromPriestsCount() == 3 && FromDevilsCount() == 2)
        {
            return AINext.Vertex.P3D2;      // 2
        }
        else if (boat.Get_to_or_from() == -1 && FromPriestsCount() == 3 && FromDevilsCount() == 1)
        {
            return AINext.Vertex.P3D1;      // 3
        }
        else if (boat.Get_to_or_from() == 1 && FromPriestsCount() == 3 && FromDevilsCount() == 2)
        {
            return AINext.Vertex.P3D2B;     // 4
        }
        else if (boat.Get_to_or_from() == 1 && FromPriestsCount() == 3 && FromDevilsCount() == 1)
        {
            return AINext.Vertex.P3D1B;     // 5
        }
        else if (boat.Get_to_or_from() == -1 && FromPriestsCount() == 3 && FromDevilsCount() == 0)
        {
            return AINext.Vertex.P3D0;      // 6
        }
        else if (boat.Get_to_or_from() == -1 && FromPriestsCount() == 1 && FromDevilsCount() == 1)
        {
            return AINext.Vertex.P1D1;      // 7
        }
        else if (boat.Get_to_or_from() == 1 && FromPriestsCount() == 2 && FromDevilsCount() == 2)
        {
            return AINext.Vertex.P2D2B;     // 8
        }
        else if (boat.Get_to_or_from() == -1 && FromPriestsCount() == 0 && FromDevilsCount() == 2)
        {
            return AINext.Vertex.P0D2;      // 9
        }
        else if (boat.Get_to_or_from() == 1 && FromPriestsCount() == 0 && FromDevilsCount() == 3)
        {
            return AINext.Vertex.P0D3B;     // 10
        }
        else if (boat.Get_to_or_from() == 1 && FromPriestsCount() == 2 && FromDevilsCount() == 1)
        {
            return AINext.Vertex.P2D1B;     // 11
        }
        else if (boat.Get_to_or_from() == -1 && FromPriestsCount() == 0 && FromDevilsCount() == 1)
        {
            return AINext.Vertex.P0D1;      // 12
        }
        else if (boat.Get_to_or_from() == 1 && FromPriestsCount() == 1 && FromDevilsCount() == 1)
        {
            return AINext.Vertex.P1D1B;     // 13
        }
        else if (boat.Get_to_or_from() == 1 && FromPriestsCount() == 0 && FromDevilsCount() == 2)
        {
            return AINext.Vertex.P0D2B;     // 14
        }    
        return AINext.Vertex.P0D0;
    }

    // 上船。type为0牧师上船；type为1恶魔上船
    public void GetOnBoat(int type)
    {
        if (boat.toOrFrom == 1)
        {
            for (int i = 0; i < fromCoast.passengerPlaner.Length; i++)
            {
                if (fromCoast.passengerPlaner[i] != null && fromCoast.passengerPlaner[i].MyGetType() == type)
                {
                    CharacterIsClicked(fromCoast.passengerPlaner[i], fromCoast.passengerPlaner[i].character);
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < toCoast.passengerPlaner.Length; i++)
            {
                if (toCoast.passengerPlaner[i] != null && toCoast.passengerPlaner[i].MyGetType() == type)
                {
                    CharacterIsClicked(toCoast.passengerPlaner[i], toCoast.passengerPlaner[i].character);
                    break;
                }
            }
        }
    }

    // 下船
    public void GetOffBoat()
    {
        if (boat.passenger[0] != null)
            CharacterIsClicked(boat.passenger[0], boat.passenger[0].character);
        if (boat.passenger[1] != null)
            CharacterIsClicked(boat.passenger[1], boat.passenger[1].character);
    }

    public void InvokeMoveBoat()
    {
        MoveBoat(boat.boat);
    }
    
    AINext mynext = new AINext();

    // 下一步
    public void NextStep()
    {
        if (userUI.status == 1 || userUI.status == 2)
            return;
        GetOffBoat();
        AINext.Edge next = mynext.GetNext(GetCurState());
        if (next == AINext.Edge.D)
            GetOnBoat(1);
        else if (next == AINext.Edge.DD)
        {
            GetOnBoat(1);
            GetOnBoat(1);
        }
        else if (next == AINext.Edge.P)
        {
            GetOnBoat(0);
        }
        else if (next == AINext.Edge.PD)
        {
            GetOnBoat(0);
            GetOnBoat(1);
        }
        else if (next == AINext.Edge.PP)
        {
            GetOnBoat(0);
            GetOnBoat(0);
        }
        Invoke("InvokeMoveBoat", 0.1f);
        Invoke("GetOffBoat", 0.1f);
    }
    
}