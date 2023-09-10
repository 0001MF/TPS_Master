using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Character Controller")]
    public CharacterController controller;

    [Header("Player Velocity and Jump")]
    Vector3 velocity;
    public float playerSpeed = 1.5f;
    public float turnTime = 0.1f;
    public float turnVelocity;
    public float playerSprint = 3f;

    [Header("Player Camera")]
    public Transform playerCamera;

    [Header("Player Animation")]
    public Animator anim;

    [Header("Jump")]
    public Transform surfaceCheck;
    bool onSurface;
    public float surfaceDistance = 0.5f;
    public LayerMask surfaceMask;
    public float Gravity = -9.81f;
    public float JumpForce = 1f;
    public float player_velocity;
    Rigidbody playerjump;
    bool Grounded;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        onSurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask);
        if (onSurface && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        velocity.y += Gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        playerMove();
        Sprint();
        Jump();
    }

    void playerMove()
    {
        float horizontalAxis = Input.GetAxisRaw("Horizontal");
        float verticalAxis = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontalAxis, 0f, verticalAxis);

        if(direction.magnitude >= 0.1f)
        {

            anim.SetBool("Walk", true);
            anim.SetBool("Run", false);
            anim.SetBool("Idle", false);
            anim.SetTrigger("Jump");

            float target = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float angle_2 = Mathf.SmoothDampAngle(transform.eulerAngles.y, target, ref turnVelocity, turnTime);
            transform.rotation = Quaternion.Euler(0f, angle_2, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, target, 0f) * Vector3.forward;
            controller.Move(moveDirection * playerSpeed * Time.deltaTime);
        }
        else
        {
            anim.SetBool("Idle", true);
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
            anim.SetTrigger("Jump");
        }
    }

    void Sprint()
    {
        if(Input.GetButton("Sprint") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            float horizontalAxis = Input.GetAxisRaw("Horizontal");
            float verticalAxis = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontalAxis, 0f, verticalAxis);

            if (direction.magnitude >= 0.1f)
            {

                anim.SetBool("Walk", false);
                anim.SetBool("Run", true);
                anim.SetBool("Idle", false);

                float target = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                float angle_2 = Mathf.SmoothDampAngle(transform.eulerAngles.y, target, ref turnVelocity, turnTime);
                transform.rotation = Quaternion.Euler(0f, angle_2, 0f);

                Vector3 moveDirection = Quaternion.Euler(0f, target, 0f) * Vector3.forward;
                controller.Move(moveDirection * playerSprint * Time.deltaTime);
            }
            else
            {
                anim.SetBool("Idle", false);
                anim.SetBool("Walk", false);
                //anim.SetBool("Run", false);
            }
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && onSurface)
        {
            anim.SetBool("Walk", false);
            anim.SetTrigger("Jump");
            velocity.y = Mathf.Sqrt(JumpForce * -2 * Gravity);
        }
        else
        {
            anim.ResetTrigger("Jump");
        }
    }

}
