using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartController : MonoBehaviour
{
    public GameObject cube;
    Material cubeMaterial;
    float timer;
    float time = 2f;

    // Start is called before the first frame update
    void Start()
    {
        cubeMaterial = cube.GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        cubeMaterial.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, timer / time));

        if (timer > time) Destroy(gameObject);
    }
}
