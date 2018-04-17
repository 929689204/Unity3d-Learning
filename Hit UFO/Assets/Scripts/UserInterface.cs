using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UFO;

public class UserInterface : MonoBehaviour
{
    public Text scoreText;
    public Text roundText;

    private int round;  // 当前的关卡数
    public GameObject bullet;
    public ParticleSystem explosion;
    public float fireRate = 0.25f;
    public float speed = 500f;
    private float nextFireTime = 0.5f;    // 距离下一次发射飞碟的时间
    // 实现接口
    private IUserInterface userInt;
    private IQueryStatus queryInt;

    void Start()
    {
        bullet = Instantiate(bullet) as GameObject;
        explosion = Instantiate(explosion) as ParticleSystem;
        userInt = SceneController.getInstance() as IUserInterface;
        queryInt = SceneController.getInstance() as IQueryStatus;
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
            userInt.playDisk();
        if (queryInt.isShooting() && Input.GetMouseButtonDown(0) && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            // 给子弹添加了刚体属性
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
            bullet.transform.position = transform.position;
            bullet.GetComponent<Rigidbody>().AddForce(ray.direction * speed, ForceMode.Impulse);
            // 利用Ray射线结构判断撞击子弹撞击飞碟
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "Disk")
            {
                // 粒子爆炸效果
                explosion.transform.position = hit.collider.gameObject.transform.position;
                explosion.GetComponent<Renderer>().material.color = hit.collider.gameObject.GetComponent<Renderer>().material.color;
                explosion.Play();
                hit.collider.gameObject.SetActive(false);// 设置状态回收
            }
        }
        roundText.text = "Round: " + queryInt.getRound().ToString();
        scoreText.text = "Score: " + queryInt.getPoint().ToString();
        if (round != queryInt.getRound()) round = queryInt.getRound();
    }
}