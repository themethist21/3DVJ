using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float playerSpeed = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-24.0f, 1.0f, 6.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * playerSpeed * Time.deltaTime);
        if (transform.position.x > 6.0f)
        {
            transform.position = new Vector3(-24.0f, 1.0f, 6.0f);
        }
    }
}
