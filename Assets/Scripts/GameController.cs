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

    public UnityEvent<bool> terrainSpawn;
    public UnityEvent<bool> obstacleSpawn;

    private float levelSpawnTimer;

    private bool levelSpawned = false;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        levelSpawnTimer = 0.5f;

        List<GameObject> terrain = new List<GameObject>();
        GameObject.FindGameObjectsWithTag("Terrain", terrain);
        foreach (GameObject obj in terrain)
        {
            terrainSpawn.AddListener(obj.GetComponent<Terrain>().SetVisible);
        }
        List<GameObject> obstacles = new List<GameObject>();
        GameObject.FindGameObjectsWithTag("Spikes", obstacles);
        foreach (GameObject obj in obstacles)
        {
            obstacleSpawn.AddListener(obj.GetComponent<Obstacles>().SetVisible);
        }
    }

    private void Update()
    {
        levelSpawnTimer -= Time.deltaTime;

        if (levelSpawnTimer < 0 && !levelSpawned)
        {
            levelSpawned = true;
            terrainSpawn.Invoke(true);
            obstacleSpawn.Invoke(true);
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            levelSpawned = true;
            terrainSpawn.Invoke(true);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            terrainSpawn.Invoke(false);
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            obstacleSpawn.Invoke(true);
        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            obstacleSpawn.Invoke(false);
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
