using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject spherePrefab;
    GameObject sphereInstance;
    float dir;
    bool isJumping;

    // Start is called before the first frame update
    void Start()
    {
        dir = 1;
        isJumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.Translate(-4f * Time.deltaTime, 0f, 0f);
            dir = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.Translate(4f * Time.deltaTime, 0f, 0f);
            dir = 1;
        }

        if (Input.GetKey(KeyCode.W) && !isJumping)
        {
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 10f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            sphereInstance = Instantiate(spherePrefab) as GameObject;
            sphereInstance.GetComponent<SphereController>().dir = dir;
            sphereInstance.transform.position = gameObject.transform.position;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Floor")
        {
            isJumping = false;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Floor")
        {
            isJumping = true;
        }
    }
}
