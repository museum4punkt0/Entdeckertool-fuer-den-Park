using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepMenu : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
