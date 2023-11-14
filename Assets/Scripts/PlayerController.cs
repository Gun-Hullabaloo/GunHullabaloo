using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public GameObject spherePrefab;
    public Material playerMaterialBlue, playerMaterialRed;
    GameObject sphereInstance;
    Rigidbody playerRb;
    float dir;
    bool isJumping;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            this.enabled = false;
        }

        gameObject.GetComponent<MeshRenderer>().material = IsOwner ? playerMaterialBlue : playerMaterialRed;
        gameObject.transform.position = new Vector3(0f, 20f, 0f);
    }

    [ClientRpc]
    void ShootClientRPC(Vector3 pos, float dir)
    {
        sphereInstance = Instantiate(spherePrefab);
        sphereInstance.GetComponent<SphereController>().dir = dir;
        sphereInstance.transform.position = pos;
    }
    [ServerRpc]
    public void ShootServerRPC(Vector3 pos, float dir)
    {
        ShootClientRPC(pos, dir);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRb = gameObject.GetComponent<Rigidbody>();
        dir = 1;
        isJumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.Translate(-4f * Time.deltaTime, 0f, 0f);
            gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
            dir = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.Translate(4f * Time.deltaTime, 0f, 0f);
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            dir = 1;
        }

        if (Input.GetKey(KeyCode.W) && !isJumping)
        {
            playerRb.velocity = new Vector3(playerRb.velocity.x, 10f, playerRb.velocity.z);
        }

        // shoot
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 pos = new Vector3(transform.position.x + dir, transform.position.y, transform.position.z);
            ShootServerRPC(pos, dir);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Floor")
        {
            isJumping = false;
        }
        else if (other.gameObject.tag == "DeadZone")
        {
            gameObject.transform.position = new Vector3(0f, 20f, 0f);
            isJumping = false;  // prevent fall through
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
