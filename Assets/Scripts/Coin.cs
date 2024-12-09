using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    public UnityEvent collectCoin;
    private Transform model;

    private float offset;
    private float _angularVel = 2.0f;
    private float _degreesPerSec = 40.0f;

    private void Start()
    {
        collectCoin.AddListener(GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().IncrementScore);
        collectCoin.AddListener(GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>().UpdateScore);

        model = this.gameObject.transform.GetChild(0);
        offset = Random.Range(1, 10);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            collectCoin.Invoke();
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        model.SetPositionAndRotation(new Vector3(model.position.x, model.position.y + 0.0005f*Mathf.Sin(offset + _angularVel * Time.time), model.position.z) ,model.rotation);
        model.Rotate(Vector3.up* _degreesPerSec * Time.deltaTime);
    }
}
