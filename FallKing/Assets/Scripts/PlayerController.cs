using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.UIElements;
using System;

////Todo: When player release the key, force the user to stop (currently using add force make the user keep sliding)
////TODO: The user will keep a minimal amount of momentum when stop?
////TODO: remove down movement
//TODO: add down as an option
//TODO: move up and left right not workings
//! Add a huge force to the opposite moving direction?
public class PlayerController : MonoBehaviour
{
    [Header("Logic")]
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] bool deathOnCollision = true;

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
    
    private Transform respawnPoint;
    //private CinemachineVirtualCamera virtualCamera;
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
        ResetAction();
        UnSubCurrentActionCallback();
    }

    void Start()
    {
        //respawnPoint = respawnLevel.Find("StageRespawnPoint");
        rigidBody = GetComponent<Rigidbody2D>();
        initialGravity = rigidBody.gravityScale;
        //Debug.LogError($"The gravity force {initialGravity} and the hoverForce {hoverForce}");
        Assert.IsTrue(hoverFallMagnitude > 0);  //Cannot have negative hover force for later calculation nor too big either
                                                //this.virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        //Register input
        playerInput = GetComponent<PlayerInput>();
        playerAction = playerInput.actions["Move"];
        originalActMap = playerInput.currentActionMap;
        //testJumpAction = playerInput.actions["TestJump"];

        playerAction.performed += MoveAction_performed;   //Subcribe to the event
        playerAction.canceled += MoveAction_canceled;

        //Testng
        playerInput.actions["SwitchMap"].performed += SwitchMap;
        freezingMap = playerInput.actions.FindActionMap("Freezing");
    }

    //THIS CALL FROM DIFFERNT CLASS
    public void SwitchMap()
    {
        if (playerInput == null)
        {
            Debug.Log("This betch was null all this time");
        }
        else
        {
            Debug.Log($"Map should switch now into Freezing");
            playerInput.SwitchCurrentActionMap("Freezing");
        }
    }

    private void SwitchMap(InputAction.CallbackContext context)
    {
        //playerInput.actions.FindActionMap("Freezing").Enable();
        //playerInput.actions.FindActionMap("Player").Disable();
        Debug.Log("Map should switch now");
        playerInput.actions.FindActionMap("Player").Disable();
        freezingMap.Enable();
    }

    private void MoveAction_canceled(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValue<Vector2>() == Vector2.zero)
        {
            playerReleasedKey = true;
            playerInputX = 0;
            playerInputY = 0;
        }
        Debug.Log($"The player input {ctx.ReadValue<Vector2>()}");
    }

    private void MoveAction_performed(InputAction.CallbackContext ctx)
    {
        playerInputX = ctx.ReadValue<Vector2>().x;
        playerInputY = ctx.ReadValue<Vector2>().y;
        if (playerInputY < 0)
        {
            FindObjectOfType<SoundManager>().PlaySoundEffect("Boost");
        }
        Debug.Log($"The player input {ctx.ReadValue<Vector2>()}");
    }

    /// <summary>
    /// Instead of Move left and right, we use different action
    /// </summary>
    /// <param name="differentAction">Action, with callback included, to use for the player</param>
    //public void ChangeAction(InputAction differentAction)
    //{
    //    //? If you change action, make sure to unsubscribe old action callback
    //    playerAction.started -= MoveAction_started;
    //    playerAction.canceled -= MoveAction_canceled;

    //    playerAction = differentAction;
    //}

    public void ChangeActionMap(InputActionMap differentActionMap)
    {
        playerInput.currentActionMap = differentActionMap;
    }

    public void ResetActionMap()
    {
        playerInput.currentActionMap = originalActMap;
    }

    /// <summary>
    /// Reset the input action back to moving the player
    /// </summary>
    public void ResetAction()
    {
        //playerAction = playerInput.actions["Move"];
        //playerAction.started += MoveAction_started;   //Subcribe to the event
        //playerAction.canceled += MoveAction_canceled;
    }

    public void UnSubCurrentActionCallback()
    {
        //playerAction.started -= MoveAction_started;
        //playerAction.canceled -= MoveAction_canceled;
    }

    //public void RegisterMoveActionCallback(Action<InputAction.CallbackContext> startedCallback, Action<InputAction.CallbackContext> performedCallback, Action<InputAction.CallbackContext> cancledCallback)
    //{
    //    playerAction.started -= MoveAction_started;
    //    playerAction.canceled -= MoveAction_canceled;

    //    playerAction.started += startedCallback;
    //    playerAction.performed += performedCallback;
    //    playerAction.canceled += cancledCallback;
    //}

    //public void UnregisterMoveAction(Action<InputAction.CallbackContext> startedCallback, Action<InputAction.CallbackContext> performedCallback, Action<InputAction.CallbackContext> cancledCallback)
    //{
    //    playerAction.started -= startedCallback;
    //    playerAction.performed -= performedCallback;
    //    playerAction.canceled-= cancledCallback;

    //    playerAction.started += MoveAction_started;
    //    playerAction.canceled += MoveAction_canceled;
    //}

    //void OnMove(InputValue movementValue)
    //{
    //    Vector2 movementVector = movementValue.Get<Vector2>();
    //    playerInputX = movementVector.x;
    //    playerInputY = movementVector.y;
    //    if (playerInputX == 0)
    //    {
    //        playerReleasedKey = true;
    //    }
    //    // Plays boost sound when player clicks down
    //    if (playerInputY != 0 && playerInputY < 0)
    //    {
    //        FindObjectOfType<SoundManager>().PlaySoundEffect("Boost");
    //    }
    //}

    //public void setRespawnPoint(Transform newRespawn)
    //{
    //    this.respawnPoint = newRespawn;
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Collide with left or right boundary
        if (collision.gameObject.tag == "Boundary")
        {
            playerInputX = 0;
        }

        //Collide with any tile collision box
        //if (collision.gameObject.tag == "Ground" && deathOnCollision)
        //{
        //    virtualCamera.Follow = this.respawnLevel;
        //    this.transform.position = new Vector2(this.respawnPoint.position.x, this.respawnPoint.position.y);
        //}
    }


    void FixedUpdate()
    {   ////TODO: When hover key pressed, fan physics doesn't work due to manually changing velocity in controll
        ////TODO: Stop using Add force to control player hovering speed? Instead change the gravity? Clamp the velocity as needed
        ////? Currently if the player is shoot up by fan while holding hover, the lesser gravity will allow to shoot up even further

        // Player release moving key, add a force to the opposite moving direction
        if (playerReleasedKey)
        {
            //! Reverse impulse
            var reverseImpulse = -(rigidBody.mass * rigidBody.velocity.x) / timeToStop;     // negative to represent opposite force
            //Debug.Log($"The impulse stop force {reverseImpulse}");
            //Debug.LogError("Only print once");
            //Debug.Log($"The current normalized velocity {rigidBody.velocity.normalized.x}");
            rigidBody.AddForce(new Vector2(reverseImpulse * Mathf.Abs(rigidBody.velocity.normalized.x), 0), ForceMode2D.Impulse);
            //Debug.Log($"The force being add {new Vector2(reverseImpulse * Mathf.Abs(rigidBody.velocity.normalized.x), 0)}");
            playerReleasedKey = false;
        }

        downSpeed = maxFallMagnitude;
        if (playerInputY > 0)   //If the key up is pressed and the player is falling
        {
            if (rigidBody.velocity.y < 0)
            {
                downSpeed = hoverFallMagnitude;
                //var moveWithUp = new Vector2(movementX, 0); //? Seperate add force horizontal and vertical later
                //rigidBody.AddForce(moveWithUp * glidingAcceleration);
                //rigidBody.velocity = new Vector2(rigidBody.velocity.x, -hoverForce);  //! Force the player to have only one speed
            }
        }
        else
        {   ////! Allow player to hold down to go down faster -> Will be a problem since the downspeed is clamped below
            var movementY = playerInputY;
            rigidBody.gravityScale = initialGravity;
            var moveDown = new Vector2(0, movementY * downKeySpeedFactor);
            rigidBody.AddForce(moveDown);
            //New audio for boost
            //FindObjectOfType<SoundManager>().PlaySoundEffect("Boost");
            //Debug.Log($"The velocity without press up {moveDown}");
        }

        //! Now add horizontal force
        float movementX = playerInputX;
        rigidBody.AddForce(new Vector2(movementX, 0) * glidingAcceleration);

        //Clamp the falling speed AND left/right movement IF falling AND the player not holding down to go down faster
        if (rigidBody.velocity.y < 0 && playerInputY >= 0)
        {
            rigidBody.velocity = new Vector2(Mathf.Clamp(rigidBody.velocity.x, -maxMoveMagnitude, maxMoveMagnitude), Mathf.Clamp(rigidBody.velocity.y, -downSpeed, 0f));
            //Debug.Log("The velocity is clamped");
        }
        else
        {   //Only clamp the left/right
            //! This else clause also accepts the case when the player NOT FALLIng, but its holding down
            rigidBody.velocity = Vector2.ClampMagnitude(rigidBody.velocity, maxMoveMagnitude);
        }

        //Debug.Log($"current velocity vertical: {rigidBody.velocity.y} and horizontal: {rigidBody.velocity.x}");
    }
}
