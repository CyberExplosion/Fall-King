using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


//? Froze = the player lose all control and player mass increase by a certain factor
//? Unfroze = player shake left and right quick enough to break free

//? WARNING = DO NOT USE THE DEFAULT PREFAB
public class FreezingAreaController : MonoBehaviour
{
    [Header("Resource")]
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerInput playerInput;

    [Tooltip("The factor to apply to player current movement speed")]
    [SerializeField] private float speedFactor = 1.0f;
    [Tooltip("The time it takes for the target to become frozen in this area")]
    [SerializeField] private float freezeTime = 5.0f;
    [Tooltip("The factor to apply to target mass once it froze")]
    [SerializeField] private float frozenMassFactor = 1.5f;
    [Tooltip("The number of alternating key press cycle to unfroze the player")]
    [SerializeField] private int alternatePressCycle = 2;

    PlayerController playerController;
    float freezeCounter = 0;
    bool playerFroze = false;
    float playerInitialMass;
    InputAction unfrozeAction;

    int wiggleCounter = 0;

    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        playerInitialMass = player.GetComponent<Rigidbody2D>().mass;
        unfrozeAction = playerInput.actions["Unfroze"];
        unfrozeAction.started += UnfrozeAction_started;
        unfrozeAction.performed += UnfrozeAction_performed;
        unfrozeAction.canceled += UnfrozeAction_canceled;
        Debug.Log($"The unfroze action {unfrozeAction}");
    }

    private void Update()
    {
        //Debug.LogError($"initial mass {player.GetComponent<Rigidbody2D>().mass}");
        checkRapidMovement();
    }

    //Player would need to rapidly pressing left and right to free themselves
    void checkRapidMovement()
    {
        //player.GetComponent<PlayerInput>().actions.Disable();
        //InputAction moveAction = player.GetComponent<PlayerInput>().actions["move"];
        //Debug.LogError($"input: {moveAction}");
        //if (moveAction.triggered)
        //{
            //Debug.LogError("Moving right now");
        //}
        //Debug.LogError($"The move is: {moveAction.ReadValue<Vector2>()}");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        freezeCounter = 0;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //checkRapidMovement();
        if (collision.GetComponent<Collider2D>().CompareTag("Player"))
        {
            //Debug.LogError($"The player velocity {collision.GetComponent<Rigidbody2D>().velocity}");
            collision.GetComponent<Rigidbody2D>().velocity = collision.GetComponent<Rigidbody2D>().velocity * speedFactor;

            if (freezeCounter > freezeTime && !playerFroze)
            {
                playerFroze = true; // Call a function from the player controller here, or announce sth to the game controller
                FreezePlayer();
            }
            freezeCounter += Time.deltaTime;
        }
    }


    //Freeze = keep the same velocity and momentum, but player cannot control anymore
    private void FreezePlayer()
    {
        //Debug.LogError($"The player is froze, initial mass {player.GetComponent<Rigidbody2D>().mass}");
        //player.GetComponent<PlayerInput>().actions.Disable();
        player.GetComponent<Rigidbody2D>().mass = player.GetComponent<Rigidbody2D>().mass * frozenMassFactor;

        unfrozeAction.started += UnfrozeAction_started;
        unfrozeAction.performed += UnfrozeAction_performed;
        unfrozeAction.canceled += UnfrozeAction_canceled;   //? Make sure to unsub later when unfroze


        //playerController.UnSubCurrentActionCallback();

        Debug.Log("at least get here before");

        playerController.SwitchMap();    //? Switching without unsubscribing, memory leak?

        //Debug.LogError($"After froze mass {player.GetComponent<Rigidbody2D>().mass}");
        // Activate freezing effect here
    }

    private void UnfrozeAction_canceled(InputAction.CallbackContext ctx)
    {
        Debug.Log("Bring player back to static sprite");
    }

    private void UnfrozeAction_performed(InputAction.CallbackContext ctx)
    {
        wiggleCounter++;

        if (wiggleCounter > 3)
        {
            Debug.Log("UNFROZE THE PLAYER");
            UnFrozePlayer();
        }
    }

    private void UnfrozeAction_started(InputAction.CallbackContext ctx)
    {
        Debug.Log("Show character freezing sprite shaking in place");
    }


    private void UnFrozePlayer()
    {
        player.GetComponent<Rigidbody2D>().mass = playerInitialMass;
        playerFroze = false;
        //Deactive freezing effects
        playerController.ResetActionMap();
    }
}
