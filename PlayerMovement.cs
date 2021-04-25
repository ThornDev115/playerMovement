using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //stores the rigidbody class in the unity engine
    public Rigidbody ridgidBody;

    public bool grounded = true;
    
    public Rigidbody drag;
    
    public GameObject Player;

    public float playerSpeed = 50f;

    public bool crouch = false;

    public bool move = true;

    public float theta = -46f;

    void FixedUpdate()
    {
        Jump();

        Movement();

        Slide();
    }

    void Movement()
    {      
        if (Input.GetKey("w") && move)
        {
            if (crouch)
            {
                ridgidBody.AddRelativeForce(playerSpeed * Mathf.Cos((360 - theta) * Mathf.PI / 180), 0f, playerSpeed * Mathf.Sin((360 - theta) * Mathf.PI / 180), ForceMode.Acceleration);
            }
            else
            {
                ridgidBody.AddRelativeForce(Vector3.forward * playerSpeed, ForceMode.Acceleration);
            }
        }

        if (Input.GetKey("a") && move)
        {
            ridgidBody.AddRelativeForce(Vector3.left * playerSpeed, ForceMode.Acceleration);
        }
        
        if (Input.GetKey("s") && move)
        {
            if (crouch)
            {
                ridgidBody.AddRelativeForce(-playerSpeed * Mathf.Cos((360f - theta) * Mathf.PI / 180f), 0f, -playerSpeed * Mathf.Sin((360f - theta) * Mathf.PI / 180f), ForceMode.Acceleration);
            }
            else
            {
                ridgidBody.AddRelativeForce(Vector3.back * playerSpeed, ForceMode.Acceleration);
            }
        }

        if (Input.GetKey("d") && move)
        {
            ridgidBody.AddRelativeForce(Vector3.right * playerSpeed, ForceMode.Acceleration);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown("space") && grounded)
        {
            //sets position of player using a rigidbody
            ridgidBody.velocity = new Vector3(ridgidBody.velocity.x, ridgidBody.velocity.y + 10.0f, ridgidBody.velocity.z);

            //states you aren't grounded
            grounded = false;
        }

        //when you are jumping the drag on the rigidbody goes to 1.5 making you able to jump higher
        if (grounded == false)
        {
            drag.drag = 1.5f;
        }
        else
        {
            //sets drag back to 3 when not grounded
            drag.drag = 3;
        }
    }

    void Slide()
    {
        //Slide
        if (Input.GetKey("left ctrl"))
        {
            crouch = true;
            Player.transform.rotation = Quaternion.Euler(theta, Player.transform.rotation.eulerAngles.y, 0f);
            ridgidBody.AddRelativeForce(Vector3.forward, ForceMode.Acceleration);
            move = false;
            ridgidBody.constraints = RigidbodyConstraints.FreezePositionY;
        }

        else
        {
            crouch = false;
            Player.transform.rotation = Quaternion.Euler(0f, Player.transform.rotation.eulerAngles.y, 0f);
            move = true;
            ridgidBody.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //States that you are grounded when you are touching an object with the tag of "TheGround"
        bool groundCollider = collision.collider.gameObject.CompareTag("TheGround");

        if (groundCollider)
        {
            grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //States that you aren't grounded when you aren't touching an object with the tag of ground
        bool groundCollider = collision.collider.gameObject.CompareTag("TheGround");

        if (groundCollider)
        {
            grounded = false;
        }
    }
}
