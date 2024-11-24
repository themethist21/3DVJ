using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector3 initPos;
    public float playerSpeed = 2.0f;
    private Vector3 moveDirection = Vector3.right;
 
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * playerSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "LeftTurn":
                transform.Rotate(new Vector3(0, -90, 0));
                break;
            case "RightTurn":
                transform.Rotate(new Vector3(0, 90, 0));
                break;
            case "LevelFinish":
                transform.position = initPos;
                break;
            default:
                break;
        }
    }
}
