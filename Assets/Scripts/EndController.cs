using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndController : MonoBehaviour
{
    public GameObject cube;
    Material cubeMaterial;
    float timer;
    float time = 2f;
    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        cubeMaterial = cube.GetComponent<MeshRenderer>().material;
        isActive = false;

        NetworkButtons.GameOverEvent.AddListener(() =>
        {
            isActive = true;
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            timer += Time.deltaTime;
            cubeMaterial.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, timer / time));

            if (timer > time)
            {
                timer = 0;
                SceneManager.LoadScene(1);
            }
        }
    }
}
