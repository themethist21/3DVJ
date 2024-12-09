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

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    private void Update()
    {

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
