using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;


public enum StageStates
{
    Despawned,
    Spawning,
    Spawned,
    Despawn,
    Cleanup
}

public class GameController : MonoBehaviour
{

    // Constant values
    private const float STAGESPAWNTIME = 1.0f;
    private const float OBSSPAWNTIME = 1.4f;
    private const float STAGEDESPAWNTIME = 1.4f;
    private const float OBSDESPAWNTIME = 0.4f;

    public int score { get; private set;}

    private bool godMode = false;

    //Events
    public UnityEvent<bool> terrainSpawn;
    public UnityEvent<bool> obstacleSpawn;
    public UnityEvent<bool> playerRun;

    //Timers
    private float stageSpawnTimer;
    private float obstacleSpawnTimer;

    public int stageCount = 1;
    private List<GameObject> stages;
    private int currentStage;
    private StageStates currentStageState;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        currentStage = 0;
        stageSpawnTimer = STAGESPAWNTIME;
        currentStageState = StageStates.Despawned;

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

        List<GameObject> coins = new List<GameObject>();
        GameObject.FindGameObjectsWithTag("Coin", coins);
        foreach (GameObject obj in coins)
        {
            obstacleSpawn.AddListener(obj.GetComponent<Coin>().SetVisible);
        }

        getStages();
        for (int i = 0; i < stageCount; i++)
        {
            ShowStage(i,false);
        }
    }

    private void Update()
    {
        stageSpawnTimer -= Time.deltaTime;
        obstacleSpawnTimer -= Time.deltaTime;    

        if (Input.GetKeyUp(KeyCode.Q))
        {
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


        switch (currentStageState)
        {
            case StageStates.Despawned:
                if (stageSpawnTimer < 0)
                {
                    ShowStage(currentStage, true);
                    currentStageState = StageStates.Spawning;
                    terrainSpawn.Invoke(true);
                    obstacleSpawnTimer = OBSSPAWNTIME;
                }
                break;
            case StageStates.Spawning:
                if (obstacleSpawnTimer < 0)
                {
                    obstacleSpawn.Invoke(true);
                    currentStageState = StageStates.Spawned;
                    playerRun.Invoke(true);
                }
                break;
            case StageStates.Spawned:
         
                break;
            case StageStates.Despawn:
                if (obstacleSpawnTimer < 0)
                {
                    terrainSpawn.Invoke(false);
                    stageSpawnTimer = STAGEDESPAWNTIME;
                    currentStageState = StageStates.Cleanup;
                }
                break;
            case StageStates.Cleanup:
                if (stageSpawnTimer < 0)
                {
                    ShowStage(currentStage, false);
                    if (currentStage + 1 < stageCount) currentStage++;
                    currentStageState = StageStates.Despawned;
                }
                break;
        }

    }

    private void getStages()
    {
        stages = new List<GameObject>();
        for (int i = 1; i < stageCount +1 ; i++)
        {
            stages.Add(GameObject.FindGameObjectWithTag("Stage" + i.ToString()));
        }
    }

    private void ShowStage(int level, bool b)
    {
        stages[level].SetActive(b);
        if (godMode)
        {
            foreach (var obj in GameObject.FindGameObjectsWithTag("JumpTrigger"))
            {
                obj.GetComponent<BoxCollider>().enabled = true;
            }
        }
    }

    public void IncrementScore()
    {
        ++score;
    }

    public void PlayerEndStage()
    {
        playerRun.Invoke(false);
        currentStageState = StageStates.Despawn;
        obstacleSpawn.Invoke(false);
        obstacleSpawnTimer = OBSDESPAWNTIME;
    }
}
