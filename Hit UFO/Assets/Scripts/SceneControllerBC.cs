using UnityEngine;
using System.Collections;
using UFO;

namespace UFO
{
    public interface IUserInterface{
        void playDisk();
    }
    // 查询当前游戏进行的状态，获取关卡、分数等信息
    public interface IQueryStatus
    {
        bool isShooting();
        int getRound();
        int getPoint();
    }
    // 切换关卡，设置分数的接口
    public interface IJudgeEvent
    {
        void nextRound();
        void setPoint(int point);
    }
    // 场记控制
    public class SceneController : System.Object, IQueryStatus, IUserInterface, IJudgeEvent
    {
        private static SceneController _instance;
        private SceneControllerBC _baseCode;
        private PhysicsActionManager _gameModel;
        private CountManager _judge;

        private int _round;
        private int _point;

        public static SceneController getInstance()
        {
            if (_instance == null)
                _instance = new SceneController();
            return _instance;
        }

        public void setGameModel(PhysicsActionManager obj)
        {
            _gameModel = obj;
        }
        internal PhysicsActionManager getGameModel()
        {
            return _gameModel;
        }

        public void setJudge(CountManager obj)
        {
            _judge = obj;
        }
        internal CountManager getJudge()
        {
            return _judge;
        }

        public void setSceneControllerBC(SceneControllerBC obj)
        {
            _baseCode = obj;
        }
        internal SceneControllerBC getSceneControllerBC()
        {
            return _baseCode;
        }

        public void playDisk()
        {
            _gameModel.prepareToPlayDisk();
        }

        public bool isShooting()
        {
            return _gameModel.isShooting();
        }
        public int getRound()
        {
            return _round;
        }
        public int getPoint()
        {
            return _point;
        }

        public void setPoint(int point)
        {
            _point = point;
        }
        public void nextRound()
        {
            _point = 0;
            _round++;
            int randisk;
            randisk = Random.Range(1, 4); // 随机1,2,3，小数忽略
            _baseCode.loadRoundData(randisk);
        }
    }
}

public class SceneControllerBC : MonoBehaviour
{
    // 这里定义了飞碟的一些属性
    private Color color;
    private Vector3 emitPos;
    private Vector3 emitDir;
    private float speed;

    void Awake() { SceneController.getInstance().setSceneControllerBC(this); }

    public void loadRoundData(int disk)
    {
        // 随机数模拟飞碟飞出的位置以及方向
        float rdm_posX = Random.Range(-2f, 2f);
        float rdm_posY = Random.Range(0f, 0.5f);
        float rdm_posZ = Random.Range(-6f, -4f);

        float rdm_dirX = Random.Range(-25f, 25f);
        float rdm_dirY = Random.Range(30f, 40f);
        float rdm_dirZ = Random.Range(65f, 70f);
        if (disk == 1)
        {
            color = Color.green;
            emitPos = new Vector3(rdm_posX, rdm_posY, rdm_posZ);
            emitDir = new Vector3(rdm_dirX, rdm_dirY, rdm_dirZ);
            speed = 5;
            SceneController.getInstance().getGameModel().setting(1, color, emitPos, emitDir.normalized, speed, 1);
        }
        else if (disk == 2)
        {
            color = Color.red;
            emitPos = new Vector3(rdm_posX, rdm_posY, rdm_posZ);
            emitDir = new Vector3(rdm_dirX, rdm_dirY, rdm_dirZ);
            speed = 4;
            SceneController.getInstance().getGameModel().setting(1, color, emitPos, emitDir.normalized, speed, 2);
        }
        else if (disk ==3 )
        {
            color = Color.blue;
            emitPos = new Vector3(rdm_posX, rdm_posY, rdm_posZ);
            emitDir = new Vector3(rdm_dirX, rdm_dirY, rdm_dirZ);
            speed = 3;
            SceneController.getInstance().getGameModel().setting(1, color, emitPos, emitDir.normalized, speed, 3);
        }
    }
}