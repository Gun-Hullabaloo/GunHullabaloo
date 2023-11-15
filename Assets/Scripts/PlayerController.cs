using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public GameObject spherePrefab;
    GameObject sphereInstance;
    Rigidbody playerRb;
    CapsuleCollider playerCollider;
    GameObject endController;
    GameObject LifeCube;
    LifeContainer Life;

    float dir;
    bool isJumping;
    bool isOwner = true;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            isOwner = false;
            this.enabled = false;
        }

        Life = gameObject.GetComponent<LifeContainer>();
        LifeCube = transform.GetChild(2).gameObject;
        playerRb = gameObject.GetComponent<Rigidbody>();
        playerCollider = gameObject.GetComponent<CapsuleCollider>();

        NetworkButtons.GameOverEvent.AddListener(onGameOver);
    }

    [ClientRpc]
    void ShootClientRPC(Vector3 pos, float dir)
    {
        sphereInstance = Instantiate(spherePrefab);
        sphereInstance.GetComponent<SphereController>().dir = dir;
        sphereInstance.transform.position = pos;
    }
    [ServerRpc]
    void ShootServerRPC(Vector3 pos, float dir)
    {
        ShootClientRPC(pos, dir);
    }
    void Start()
    {
        Life.resetLife();
        gameObject.transform.position = new Vector3(0f, 20f, 0f);
        LifeCube.transform.GetChild(1).gameObject.SetActive(true);
        LifeCube.transform.GetChild(2).gameObject.SetActive(true);
        dir = 1;
        isJumping = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.Translate(-8f * Time.deltaTime, 0f, 0f);
            gameObject.transform.localScale = new Vector3(-4f, 4f, 4f);
            // LifeCube.transform.localScale = new Vector3(-1f, 1f, 1f);
            dir = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.Translate(8f * Time.deltaTime, 0f, 0f);
            gameObject.transform.localScale = new Vector3(4f, 4f, 4f);
            // LifeCube.transform.localScale = new Vector3(1f, 1f, 1f);
            dir = 1;
        }

        if (Input.GetKey(KeyCode.W) && !isJumping)
        {
            playerRb.velocity = new Vector3(playerRb.velocity.x, 50f, playerRb.velocity.z);
        }

        // shoot
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 pos = new Vector3(transform.position.x + 2.5f * dir, transform.position.y + 2f, transform.position.z);
            ShootServerRPC(pos, dir);
        }

        playerRb.velocity = new Vector3(playerRb.velocity.x, playerRb.velocity.y - 100f * Time.deltaTime, playerRb.velocity.z);
    }
    private void onGameOver()
    {
        Invoke("Start", 2f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Floor")
        {
            isJumping = false;
        }
        else if (other.gameObject.tag == "DeadZone")
        {
            Life.life--;
            if (Life.life <= 0)
            {
                Debug.Log("GameOver");
                NetworkButtons.GameOverEvent.Invoke();

            }
            else
            {
                Debug.Log("remaining life " + Life.life);
                LifeCube.transform.GetChild(Life.life).gameObject.SetActive(false);
                gameObject.transform.position = new Vector3(0f, 20f, 0f);
                isJumping = true;
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (!isOwner) return;
        if (other.gameObject.tag == "Floor")
        {
            isJumping = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isOwner) return;
        if (other.gameObject.tag == "Floor")
        {
            playerCollider.isTrigger = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!isOwner) return;
        if (other.gameObject.tag == "Floor")
        {
            playerCollider.isTrigger = false;
        }
    }
}
