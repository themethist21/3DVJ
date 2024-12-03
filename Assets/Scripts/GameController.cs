using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public int score { get; private set;}
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    public void IncrementScore()
    {
        ++score;
    }
}
