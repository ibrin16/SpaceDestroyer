using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUICanvas : MonoBehaviour
{

    public static GameUICanvas instance;

    void Awake()
    {
        instance = this; 
    }
}
