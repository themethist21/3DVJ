using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    public UnityEvent collectCoin;

    private void Start()
    {
        collectCoin.AddListener(GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().IncrementScore);
        collectCoin.AddListener(GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>().UpdateScore);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            collectCoin.Invoke();
            Destroy(gameObject);
        }
    }
}
