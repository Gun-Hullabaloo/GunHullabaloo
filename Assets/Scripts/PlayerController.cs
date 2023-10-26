using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public GameObject spherePrefab;
    GameObject sphereInstance;
    float dir;
    bool isJumping;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) this.enabled = false;
    }

    [ClientRpc]
    void ShootClientRPC(Vector3 point, float dir)
    {
        sphereInstance = Instantiate(spherePrefab);
        sphereInstance.GetComponent<SphereController>().dir = dir;
        sphereInstance.transform.position = point;
    }
    [ServerRpc]
    public void ShootServerRPC(Vector3 point, float dir)
    {
        ShootClientRPC(point, dir);
    }

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

        // shoot
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootServerRPC(transform.position, dir);
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
