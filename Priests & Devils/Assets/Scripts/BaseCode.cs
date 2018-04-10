using UnityEngine;
using System.Collections;
using Com.Mygame;

namespace Com.Mygame
{
    public enum State { BSTART, BSEMOVING, BESMOVING, BEND, WIN, LOSE };
    /* 
     * BSTART:  boat stops on start shore 
     * BEND:    boat stops on end shore 
     * BSEMOVING:   boat is moving from start shore to end shore 
     * BESMOVING:   boat is moving from end shore to start shore 
     * WIN:     win 
     * LOSE:    lose 
     */

    public interface IUserActions
    {
        void priestSOnB();
        void priestEOnB();
        void devilSOnB();
        void devilEOnB();
        void moveBoat();
        void offBoatL();
        void offBoatR();
        void restart();
    }

    public class GameSceneController : System.Object, IUserActions
    {

        private static GameSceneController _instance;
        private BaseCode _base_code;
        private GenGameObject _gen_game_obj;
        public State state = State.BSTART;

        public static GameSceneController GetInstance()
        {
            if (null == _instance)
            {
                _instance = new GameSceneController();
            }
            return _instance;
        }

        public BaseCode getBaseCode()
        {
            return _base_code;
        }

        internal void setBaseCode(BaseCode bc)
        {
            if (null == _base_code)
            {
                _base_code = bc;
            }
        }

        public GenGameObject getGenGameObject()
        {
            return _gen_game_obj;
        }

        internal void setGenGameObject(GenGameObject ggo)
        {
            if (null == _gen_game_obj)
            {
                _gen_game_obj = ggo;
            }
        }

        public void priestSOnB()
        {
            _gen_game_obj.priestStartOnBoat();
        }

        public void priestEOnB()
        {
            _gen_game_obj.priestEndOnBoat();
        }

        public void devilSOnB()
        {
            _gen_game_obj.devilStartOnBoat();
        }

        public void devilEOnB()
        {
            _gen_game_obj.devilEndOnBoat();
        }

        public void moveBoat()
        {
            _gen_game_obj.moveBoat();
        }

        public void offBoatL()
        {
            _gen_game_obj.getOffTheBoat(0);
        }

        public void offBoatR()
        {
            _gen_game_obj.getOffTheBoat(1);
        }

        public void restart()
        {
            Application.LoadLevel(Application.loadedLevelName);
            state = State.BSTART;
        }
    }
}

public class BaseCode : MonoBehaviour
{

    public string gameName;
    public string gameRule;

    void Start()
    {
        GameSceneController my = GameSceneController.GetInstance();
        my.setBaseCode(this);
        gameName = "Priests and Devils";
        gameRule = "Priests and Devils is a puzzle game in which you will help the Priests and Devils to cross the river within the time limit. There are 3 priests and 3 devils at one side of the river. They all want to get to the other side of this river, but there is only one boat and this boat can only carry two persons each time. And there must be one person steering the boat from one side to the other side. In the flash game, you can click on them to move them and click the go button to move the boat to the other direction. If the priests are out numbered by the devils on either side of the river, they get killed and the game is over. You can try it in many ways. Keep all priests alive! Good luck!             Sphere -- Priest    Cube -- Devil";
    }
}