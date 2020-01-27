using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Declare any public variables that you want to be able 
    // to access throughout your scene
    public Player player;
    public static GameManager Instance { get; private set; } // static singleton
    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        // Cache references to all desired variables
        player = FindObjectOfType<Player>();
    }
}