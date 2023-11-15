using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentScale : MonoBehaviour
{
    // Start is called before the first frame update
    Transform parentTransform;
    void Start()
    {
        parentTransform = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if (parentTransform.localScale.x < 0)
            transform.localScale = new Vector3(-1f, 1f, 1f);
        else
            transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
