using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //See if player started moving
    bool startedMoving = false;

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
    public bool overWater = true;

    //Get Camera
    public Camera mainCam;

    //Get Slider
    public Slider waterLevel;
    public float lossAmount = 5f;
    public float waterGain = .01f;

    //Is hit
    bool isHit = false;

    //Lives
    public int lives = 3;

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
        lives = 3;

    }

    private void Update()
    {
        if(lives > 0)
        {
            RotateJetpack();
            RaiseJetpack();
        }
        else
        {
            //play burning animation
        }
    }


    private void RaiseJetpack()
    {
       
        if(movementInput.y > 0 && hasWater && waterLevel.value != 0)
        {
            if(transform.position.y < heightLimit)
            {
                rb.velocity = new Vector2(currentEulerAngles.z * raiseSpeed * angleSpeed * -1, raiseSpeed);
            }
            startedMoving = true;
            waterLevel.value -= lossAmount * Time.deltaTime; 
            
        }
    }

    private void RotateJetpack()
    {
        if(movementInput.x != 0f)
        {
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
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (movementInput.y > 0 && collision.tag == "Edge")
        {
            float camX = mainCam.transform.position.x;
            mainCam.transform.position = new Vector3(mainCam.transform.position.x + Time.deltaTime, 0, -10);
        }

        if(collision.tag == "Water")
        {
            Puddle puddle = collision.GetComponent<Puddle>();
            if(waterLevel.value < 1 && puddle.water > 0)
            {
                puddle.water -= lossAmount * Time.deltaTime;
                waterLevel.value += (lossAmount + (lossAmount / 2)) * Time.deltaTime;
            }
        }

        if(collision.tag == "Obstacle")
        {
            if(!isHit)
            {
                lives--;
                StartCoroutine(Hit());
            }
        }

        if (collision.tag == "Ground")
        {
            if (!isHit)
            {
                lives--;
                waterLevel.value = .5f;
                rb.velocity = new Vector2(currentEulerAngles.z * raiseSpeed * angleSpeed * -1, raiseSpeed + raiseSpeed);
                StartCoroutine(Hit());
            }

        }
    }

    IEnumerator Hit()
    {
        isHit = true;
        SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        yield return new WaitForSeconds(.2f);
        sprite.enabled = true;
        yield return new WaitForSeconds(.2f);
        sprite.enabled = false;
        yield return new WaitForSeconds(.2f);
        sprite.enabled = true;
        yield return new WaitForSeconds(.2f);
        sprite.enabled = false;
        yield return new WaitForSeconds(.2f);
        sprite.enabled = true;
        yield return new WaitForSeconds(.2f);
        isHit = false;
    }

}
