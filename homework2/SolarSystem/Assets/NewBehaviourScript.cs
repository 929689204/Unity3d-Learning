using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    public Transform Sun;
    public Transform Mercury;
    public Transform Venus;
    public Transform Earth;
    public Transform Mars;
    public Transform Jupiter;
    public Transform Saturn;
    public Transform Uranus;
    public Transform Neptune;


    // Use this for initialization
    void Start()
    {
        Sun.position = Vector3.zero;
        Mercury.position = new Vector3(6, 0, 0);
        Venus.position = new Vector3(9, 0, 0);
        Earth.position = new Vector3(12, 0, 0);
        Mars.position = new Vector3(15, 0, 0);
        Jupiter.position = new Vector3(18, 0, 0);
        Saturn.position = new Vector3(24, 0, 0);
        Uranus.position = new Vector3(30, 0, 0);
        Neptune.position = new Vector3(33, 0, 0);
    }
    // Update is called once per frame
    void Update()
    {
        Mercury.transform.RotateAround(Vector3.zero, new Vector3(0, 10, 1), 60 * Time.deltaTime);
        Venus.transform.RotateAround(Vector3.zero, new Vector3(0, 10, 4), 56 * Time.deltaTime);
        Earth.transform.RotateAround(Vector3.zero, new Vector3(0, 20, 3), 50 * Time.deltaTime);
        Mars.transform.RotateAround(Vector3.zero, new Vector3(0, 15, 5), 44 * Time.deltaTime);
        Jupiter.transform.RotateAround(Vector3.zero, new Vector3(0, 20, 3), 40 * Time.deltaTime);
        Saturn.transform.RotateAround(Vector3.zero, new Vector3(0, 30, 1), 32 * Time.deltaTime);
        Uranus.transform.RotateAround(Vector3.zero, new Vector3(0, 40, 4), 24 * Time.deltaTime);
        Neptune.transform.RotateAround(Vector3.zero, new Vector3(0, 50, 5), 20 * Time.deltaTime);


        Mercury.Rotate(Vector3.up * 30 * Time.deltaTime);
        Venus.Rotate(Vector3.up * 20 * Time.deltaTime);
        Earth.Rotate(Vector3.up * 50 * Time.deltaTime);
        Mars.Rotate(Vector3.up * 51 * Time.deltaTime);
        Jupiter.Rotate(Vector3.up * 80 * Time.deltaTime);
        Saturn.Rotate(Vector3.up * 75 * Time.deltaTime);
        Uranus.Rotate(Vector3.up * 70 * Time.deltaTime);
        Neptune.Rotate(Vector3.up * 60 * Time.deltaTime);
    }

}
