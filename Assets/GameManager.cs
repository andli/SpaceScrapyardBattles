using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Declare any public variables that you want to be able 
    // to access throughout your scene
    public Player player;

    public List<ShipPartDisplay> connectionTargets;

    public static GameManager Instance { get; private set; } // static singleton
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

        }
        else
        {
            Destroy(gameObject);
            return;
        }
        // Cache references to all desired variables
        player = FindObjectOfType<Player>();

        this.connectionTargets = new List<ShipPartDisplay>();
    }

    private void OnDestroy()
    {
        Debug.Log("destroyed GameManager");

    }

    public void onGoButtonPressed() {
        SceneManager.LoadScene(2);
    }

    public void onBackButtonPressed()
    {
        SceneManager.LoadScene(1);
    }

    internal void AddConnectionTarget(ShipPartDisplay shipPartDisplay)
    {
        if (!connectionTargets.Contains(shipPartDisplay))
        {
            this.connectionTargets.Add(shipPartDisplay);

        }
    }

    internal void ClearConnectionTargets(List<ShipPartDisplay> partsToRemove)
    {
        foreach (ShipPartDisplay item in partsToRemove)
        {
            this.connectionTargets.Remove(item);
        }
    }

    internal void ClearAllConnectionTargets()
    {
        this.connectionTargets.Clear();
    }
}