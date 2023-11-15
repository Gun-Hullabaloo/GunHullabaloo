using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeContainer : MonoBehaviour
{
    public int life;

    void Awake()
    {
        resetLife();
    }
    public void resetLife()
    {
        life = 3;
    }
}


