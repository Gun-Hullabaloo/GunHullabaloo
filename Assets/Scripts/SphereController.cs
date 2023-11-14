using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    public float dir;
    // Transform playerTransform;
    // Rigidbody playerRigidbody;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(32f * dir * Time.deltaTime, 0f, 0f);
    }

    private void OnCollisionEnter(Collision other)
    {
        // playerTransform = other.gameObject.transform;
        // playerRigidbody = other.gameObject.GetComponent<Rigidbody>();

        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.Translate(dir, 0f, 0f);
            // playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x + dir * 10f, playerRigidbody.velocity.y, playerRigidbody.velocity.z);
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == "DeadZone")
        {
            Destroy(gameObject);
        }
    }
}
