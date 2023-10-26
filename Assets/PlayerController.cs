using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject sphere;
    GameObject sphereInstance;
    float dir;

    // Start is called before the first frame update
    void Start()
    {
        dir = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.Translate(-2f * Time.deltaTime, 0f, 0f);
            dir = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.Translate(2f * Time.deltaTime, 0f, 0f);
            dir = 1;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            sphereInstance = Instantiate(sphere) as GameObject;
            sphereInstance.GetComponent<SphereController>().dir = dir;
            sphereInstance.transform.position = gameObject.transform.position;
        }
    }
}
