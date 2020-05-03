using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDifficulty : MonoBehaviour
{
    private static GameDifficulty instance = null;
    public static GameDifficulty Instance => instance;

    public string Difficulty { get; set; }

    // Implemented the Singleton pattern so GameDifficulty can be referenced from a different scene
    private void Awake()
    {
        // if instance is not yet set, set it and make it persistent between scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // if instance is already set and this is not the same object, destroy it
            if (this != instance)
            {
                Destroy(gameObject);
            }
        }
    }

}
