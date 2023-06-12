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
    [SerializeField] private float glidingAcceleration = 15f;
    [Tooltip("The player max left and right velocity")]
    [SerializeField] private float maxMoveMagnitude = 20f;
    [Tooltip("Time from when release key until the player come to complete stop")]
    [SerializeField] private float timeToStop = 1f;

    [Header("Vertical Physics")]
    [Tooltip("The player falling speed when user holding up key")]
    [SerializeField] private float hoverFallMagnitude = 15f;
    [Tooltip("The max falling speed the player can have through gravity (doesn't account for holding down key)")]
    [SerializeField] private float maxFallMagnitude = 20f;
    [Tooltip("Speed factor apply to downward speed when holding the down key")]
    [SerializeField] private float downKeySpeedFactor = 5f;

    [SerializeField] private Transform respawnLevel;
    
    private Rigidbody2D rigidBody;
    private float initialGravity;
    private float playerInputX, playerInputY;
    bool playerReleasedKey = true;
    float downSpeed;

    private InputActionMap freezingMap;
    private PlayerInput playerInput;
    private InputActionMap originalActMap;
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
        originalActMap = playerInput.currentActionMap;

        SubscribeActCallback();
    }

    //THIS CALL FROM DIFFERNT CLASS
    public void SwitchMap(string mapName)
    {
        UnSubCurrentActionCallback();
        playerInput.SwitchCurrentActionMap(mapName);
        Debug.Log($"Switched to input map {playerInput.currentActionMap.name}");
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
        if (playerInputY < 0)
        {
            FindObjectOfType<SoundManager>().PlaySoundEffect("Boost");
        }
    }

    public void ResetActionMap()
    {
        playerInput.SwitchCurrentActionMap("Player");
        Debug.Log($"The current action map now is {playerInput.currentActionMap.name}");
        SubscribeActCallback();
    }

    public void UnSubCurrentActionCallback()
    {
        playerAction.performed -= MoveAction_performed;
        playerAction.canceled -= MoveAction_canceled;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Collide with left or right boundary
        if (collision.gameObject.tag == "Boundary")
        {
            playerInputX = 0;
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
            var movementY = playerInputY;
            rigidBody.gravityScale = initialGravity;
            var moveDown = new Vector2(0, movementY * downKeySpeedFactor);
            rigidBody.AddForce(moveDown);
        }

        //! Now add horizontal force
        float movementX = playerInputX;
        rigidBody.AddForce(new Vector2(movementX, 0) * glidingAcceleration);

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
