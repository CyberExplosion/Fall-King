using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.UIElements;
using System;

//TODO: add down as an option
//TODO: move up and left right not workings
//! Add a huge force to the opposite moving direction?
public class PlayerController : MonoBehaviour
{
    [Header("Horizontal Physics")]
    [Tooltip("Acceleration to the top speed")]
    [SerializeField] private float glidingAcceleration = 10f;
    [Tooltip("The player max left and right velocity")]
    [SerializeField] private float maxMoveMagnitude = 5f;
    [Tooltip("Time from when release key until the player come to complete stop")]
    [SerializeField] private float timeToStop = 1.5f;

    [Header("Vertical Physics")]
    [Tooltip("The player falling speed when user holding up key")]
    [SerializeField] private float hoverFallMagnitude = 3f;
    [Tooltip("The max falling speed the player can have through gravity (doesn't account for holding down key)")]
    [SerializeField] private float maxFallMagnitude = 5f;
    [Tooltip("Speed factor apply to downward speed when holding the down key")]
    [SerializeField] private float downKeySpeedFactor = 100f;

    [Header("Environment Interactions")]
    [Tooltip("The player max movement speed when going against wind")]
    [SerializeField] private float accelerationAgainstWind = 1.5f;

    //! movement
    private Rigidbody2D rigidBody;
    private float initialGravity;
    private float playerInputX, playerInputY;
    private float downSpeed;
    bool playerReleasedKey = true;

    //! Windy
    private bool fightAgainstWind = false;
    private float windForce;

    private PlayerInput playerInput;
    private InputAction playerAction;

    //Unsubscribe event when disable the script
    private void OnDisable()
    {
        UnSubCurrentActionCallback();
    }

    private void SubscribeActCallback()
    {
        playerAction.performed += MoveAction_performed;   //Subcribe to the event
        playerAction.canceled += MoveAction_canceled;
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        initialGravity = rigidBody.gravityScale;
        Assert.IsTrue(hoverFallMagnitude > 0);  //Cannot have negative hover force for later calculation nor too big either
                                                //this.virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        //Register input
        playerInput = GetComponent<PlayerInput>();
        playerAction = playerInput.actions["Move"];
        SubscribeActCallback();
    }

    //THIS CALL FROM DIFFERNT CLASS
    public void SwitchMap(string mapName)
    {
        UnSubCurrentActionCallback();
        playerInput.SwitchCurrentActionMap(mapName);
    }

    private void MoveAction_canceled(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValue<Vector2>() == Vector2.zero)
        {
            playerReleasedKey = true;
            playerInputX = 0;
            playerInputY = 0;
        }
    }

    private void MoveAction_performed(InputAction.CallbackContext ctx)
    {
        playerInputX = ctx.ReadValue<Vector2>().x;
        playerInputY = ctx.ReadValue<Vector2>().y;
        if (playerInputY < 0 )
        {
            FindObjectOfType<SoundManager>().PlaySoundEffect("Boost");
        }
        if (playerInputY > 0 )
        {
            FindObjectOfType<SoundManager>().PlaySoundEffect("Glide");
        }
    }

    public void ResetActionMap()
    {
        playerInput.SwitchCurrentActionMap("Player");
        SubscribeActCallback();
    }

    public void UnSubCurrentActionCallback()
    {
        playerAction.performed -= MoveAction_performed;
        playerAction.canceled -= MoveAction_canceled;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Wind"))
        {

            float windAngleDir = collision.gameObject.GetComponent<AreaEffector2D>().forceAngle;
            windForce = collision.gameObject.GetComponent<AreaEffector2D>().forceMagnitude;
            //maxMoveMagnitude = maxSpeedAgainstWind;
            //! Very scuff, but did it job
            if (windAngleDir >= 90 || windAngleDir <= 270)
            {
                if (playerInputX > 0)   //wind blowing left and player going right
                {
                    fightAgainstWind = true;
                }
            }
            else
            {
                // wind blowing right
                if (playerInputX < 0)
                {
                    fightAgainstWind = true;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Wind"))
        {
            fightAgainstWind = false;
        }
    }

    void FixedUpdate()
    {
        if (PauseManager.paused) return;

        // Player release moving key, add a force to the opposite moving direction
        if (playerReleasedKey)
        {
            //! Reverse impulse
            var reverseImpulse = -(rigidBody.mass * rigidBody.velocity.x) / timeToStop;     // negative to represent opposite force
            rigidBody.AddForce(new Vector2(reverseImpulse * Mathf.Abs(rigidBody.velocity.normalized.x), 0), ForceMode2D.Impulse);
            playerReleasedKey = false;
        }

        downSpeed = maxFallMagnitude;
        if (playerInputY > 0)   //If the key up is pressed and the player is falling
        {
            if (rigidBody.velocity.y < 0)
            {
                downSpeed = hoverFallMagnitude;
            }
        }
        else
        {
            //Ignore the max fall magnitude
            var movementY = playerInputY;
            rigidBody.gravityScale = initialGravity;
            var moveDown = new Vector2(0, movementY * downKeySpeedFactor);
            rigidBody.AddForce(moveDown);
        }

        //! Now add horizontal force
        float movementX = playerInputX;
        if (!fightAgainstWind)
        {
            rigidBody.AddForce(new Vector2(movementX, 0) * glidingAcceleration);
        }
        else
        {
            //? This will allow the player to always go against the wind if they want to
            rigidBody.AddForce(new Vector2(movementX, 0) * windForce * accelerationAgainstWind);
        }

        if (rigidBody.velocity.y < 0 && playerInputY >= 0)
        {
            rigidBody.velocity = new Vector2(Mathf.Clamp(rigidBody.velocity.x, -maxMoveMagnitude, maxMoveMagnitude), Mathf.Clamp(rigidBody.velocity.y, -downSpeed, 0f));
        }
        else
        {   //Only clamp the left/right
            //! This else clause also accepts the case when the player NOT FALLIng, but its holding down
            rigidBody.velocity = Vector2.ClampMagnitude(rigidBody.velocity, maxMoveMagnitude);
        }
    }
}
