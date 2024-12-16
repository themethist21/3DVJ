using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public int score { get; private set;}

    private bool godMode = false;
    
    public UnityEvent toggleGodMode;

    public UnityEvent terrainSpawn;

    private float levelSpawnTimer;

    private bool levelSpawned = false;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        levelSpawnTimer = 5.0f;
    }

    private void Update()
    {
        levelSpawnTimer -= Time.deltaTime;

        if (levelSpawnTimer < 0 && !levelSpawned)
        {
            levelSpawned = true;
            terrainSpawn.Invoke();
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            godMode = !godMode;
            foreach (var obj in GameObject.FindGameObjectsWithTag("JumpTrigger"))
            {
                obj.GetComponent<BoxCollider>().enabled = !obj.GetComponent<BoxCollider>().enabled;
            }
        }
    }

    public void IncrementScore()
    {
        ++score;
    }
}
