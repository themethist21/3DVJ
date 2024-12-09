using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStates
{
    Grounded,
    Jump,
    Fall
}
public class PlayerMovement : MonoBehaviour
{

    public PlayerData Data;
    //Public components
    public Rigidbody rb;


    //Public movement variables
    public Vector3 initPos;
    public float gravityScale = 1.0f;

    //private movement variables
    private const float jumpInputBufferTime = 0.2f;

    //Timers
    private float lastJumpInputTimer = 0.0f;

    PlayerStates state = PlayerStates.Grounded;


    private Vector3 moveDirection = Vector3.right;

    private void OnEnable()
    {
        rb.useGravity = false;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {
        Vector3 gravity = Data.gravityStrength * gravityScale * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

    // Update is called once per frame
    void Update()
    {
        #region TIMERS
        lastJumpInputTimer -= Time.deltaTime;
        #endregion

        #region INPUT
        if (Input.GetKeyDown(KeyCode.Space))
        {
            lastJumpInputTimer = jumpInputBufferTime;
        }
        #endregion

        if (state == PlayerStates.Grounded)
        {
            transform.Translate(Data.playerSpeed * Time.deltaTime * moveDirection);

            if (lastJumpInputTimer > 0.0f)
            {
                if (rb.velocity.y < 0) rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(Vector3.up * Data.jumpForce, ForceMode.Impulse);
                state = PlayerStates.Jump;
            }
        }
        else if (state == PlayerStates.Fall)
        {
            transform.Translate(Data.playerSpeed * Time.deltaTime * moveDirection);
            SetGravityScale(1.0f * Data.fallGravityMult);
        }
        else if (state == PlayerStates.Jump)
        {
            transform.Translate(Data.playerSpeed * Time.deltaTime * moveDirection);

            if (rb.velocity.y < 0) state = PlayerStates.Fall;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "LeftTurn":
                transform.Rotate(new Vector3(0, -90, 0));
                transform.SetPositionAndRotation(fixCoords(transform.position), transform.rotation);
                break;
            case "RightTurn":
                transform.Rotate(new Vector3(0, 90, 0));
                transform.SetPositionAndRotation(fixCoords(transform.position), transform.rotation);
                break;
            case "LevelFinish":
                transform.position = initPos;
                break;
            case "JumpTrigger":
                Jump();
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(state == PlayerStates.Fall) rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (state != PlayerStates.Jump) state = PlayerStates.Grounded;
        SetGravityScale(1.0f);
    }


    private void SetGravityScale(float scale)
    {
        gravityScale = scale;
    }

    private void Jump()
    {
        if (state == PlayerStates.Grounded)
        {
            if (rb.velocity.y < 0) rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * Data.jumpForce, ForceMode.Impulse);
            state = PlayerStates.Jump;
        }
    }

    Vector3 fixCoords(Vector3 coords)
    {
        return new Vector3(Mathf.RoundToInt(coords.x), coords.y, Mathf.RoundToInt(coords.z));
    }
}
