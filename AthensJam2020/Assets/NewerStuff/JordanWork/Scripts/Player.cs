using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    //See if player started moving
    public bool startedMoving = false;
    public bool offGround = false;
    //Rotation Speed
    public float rotationSpeed = 10f;

    //Velocity
    public float raiseSpeed = 10f;

    //Angle speed
    public float angleSpeed = 10f;

    public float maxAngle = 15;

    //Height Limit
    public float heightLimit = 2f;

    //Make Vector3 for storing Euler angles
    Vector3 currentEulerAngles;
    Quaternion currentRotation;

    //Check the water levels of puddle
    public bool hasWater = true;

    //Get Camera
    public Camera mainCam;

    //Get Slider
    public Slider waterLevel;
    public float lossAmount = 5f;
    public float waterGain = .01f;

    //Is hit
    bool isHit = false;
    private bool isReady = false;
    
    //Lives
    public int lives = 3;

    //Rigidbody
    Rigidbody2D rb;

    //Input Actions
    PlayerInputActions inputAction;

    //Move
    Vector2 movementInput;

    //Audio
    AudioSource audioSource;

    private SpriteRenderer _SpriteRenderer;

    private BoxCollider2D _BoxCollider2D;

    public GameObject groundSound;
    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>() as Rigidbody2D;
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        _BoxCollider2D = GetComponent<BoxCollider2D>();
        inputAction = new PlayerInputActions();
        inputAction.PlayerControls.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        lives = 3;
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private IEnumerator Start() {
        float gravity = rb.gravityScale;
        rb.gravityScale = 0f;
        yield return new WaitForSeconds(2f);
        
        rb.gravityScale = gravity;
        isReady = true;
    }

    private void Update()
    {
        if(lives > 0 && isReady)
        {
            if (startedMoving)
            {
                audioSource.Play();
                startedMoving = false;
                offGround = true;
            }
            RotateJetpack();
            RaiseJetpack();
        }
        else
        {
            //play burning animation
            audioSource.Stop();
            offGround = false;
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
            if(!audioSource.isPlaying)
            {
                startedMoving = true;
            }
            
            waterLevel.value -= lossAmount * Time.deltaTime;
        }
    }

    private void RotateJetpack()
    {
        Debug.Log(movementInput.x);

        if(movementInput.x != 0f)
        {
            currentEulerAngles += new Vector3(0, 0, movementInput.x * rotationSpeed);

            if(currentEulerAngles.z > maxAngle)
            {
                currentEulerAngles.z = maxAngle;
            }
            else if (currentEulerAngles.z < -maxAngle)
            {
                currentEulerAngles.z = -maxAngle;
            }

            if (currentEulerAngles.z <= maxAngle && currentEulerAngles.z >= -maxAngle)
            {
                //moving the value of the Vector3 into Quanternion.eulerAngle format
                currentRotation.eulerAngles = currentEulerAngles;

                //apply the Quaternion.eulerAngles change to the gameObject
                transform.rotation = currentRotation;
            }
        }
        else {
            currentEulerAngles = Vector3.zero;

            //moving the value of the Vector3 into Quanternion.eulerAngle format
            currentRotation.eulerAngles = currentEulerAngles;

            //apply the Quaternion.eulerAngles change to the gameObject
            transform.rotation = currentRotation;
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
                if (!puddle.isInfinite) {
                    puddle.water -= lossAmount * Time.deltaTime;
                    waterLevel.value += (lossAmount + (lossAmount / 2)) * Time.deltaTime;   
                }
            }
        }

        if(collision.tag == "Obstacle")
        {
            if(!isHit)
            {
                collision.GetComponent<AudioSource>().Play();
                lives--;
                StartCoroutine(Hit());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.tag);
        if (other.tag == "Seagull") {
            if (!isHit)
            {
                other.GetComponent<AudioSource>().Play();
                lives--;
                waterLevel.value = .5f;
                rb.velocity = new Vector2(currentEulerAngles.z * raiseSpeed * angleSpeed * -1, raiseSpeed + raiseSpeed);
                StartCoroutine(Hit());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.tag == "Ground" || other.collider.tag == "Seagull")
        {
            if (!isHit && offGround)
            {
                groundSound.GetComponent<AudioSource>().Play();
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
        Physics2D.IgnoreLayerCollision(9, 10, true);
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
        Physics2D.IgnoreLayerCollision(9, 10, false);
        isHit = false;
    }

}
