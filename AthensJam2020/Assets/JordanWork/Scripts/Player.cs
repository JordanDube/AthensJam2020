using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Rotation Speed
    public float rotationSpeed = 10f;

    //Velocity
    public float raiseSpeed = 10f;

    //Angle speed
    public float angleSpeed = 10f;

    //Height Limit
    public float heightLimit = 2f;

    //Make Vector3 for storing Euler angles
    Vector3 currentEulerAngles;
    Quaternion currentRotation;

    //Check the water levels of puddle
    public bool hasWater = true;

    //Get Camera
    public Camera mainCam;

    //Rigidbody
    Rigidbody2D rb;

    //Input Actions
    PlayerInputActions inputAction;

    //Move
    Vector2 movementInput;
    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>() as Rigidbody2D;
        inputAction = new PlayerInputActions();
        inputAction.PlayerControls.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();

    }

    private void Update()
    {
        RotateJetpack();
        RaiseJetpack();
    }


    private void RaiseJetpack()
    {
       
        if(movementInput.y > 0 && hasWater)
        {
            if(transform.position.y < heightLimit)
            {
                rb.velocity = new Vector2(currentEulerAngles.z * raiseSpeed * angleSpeed * -1, raiseSpeed);
            }
            
        }
    }

    private void RotateJetpack()
    {
        if(movementInput.x != 0f)
        {

                //modifying the Vector3, based on input multiplied by speed and time
                currentEulerAngles += new Vector3(0, 0, movementInput.x * rotationSpeed);

            if(currentEulerAngles.z > 15)
            {
                currentEulerAngles.z = 15;
            }
            else if (currentEulerAngles.z < -15)
            {
                currentEulerAngles.z = -15;
            }

            if (currentEulerAngles.z <= 15 && currentEulerAngles.z >= -15)
            {
                //moving the value of the Vector3 into Quanternion.eulerAngle format
                currentRotation.eulerAngles = currentEulerAngles;

                //apply the Quaternion.eulerAngles change to the gameObject
                transform.rotation = currentRotation;
            }
                

            
        }
    }
    private void OnEnable()
    {
        inputAction.Enable();
    }
    private void OnDisable()
    {
        inputAction.Disable();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger entered");
        if(movementInput.y > 0 && collision.tag == "Edge")
        {
            float camX = mainCam.transform.position.x;
            mainCam.transform.position = new Vector3(camX + 1, 0, -10);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Trigger staying");
        if (movementInput.y > 0 && collision.tag == "Edge")
        {
            float camX = mainCam.transform.position.x;
            mainCam.transform.position = new Vector3(mainCam.transform.position.x + Time.deltaTime, 0, -10);
        }
    }

}
