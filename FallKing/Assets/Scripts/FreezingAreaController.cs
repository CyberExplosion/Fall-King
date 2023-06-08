using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


//? Froze = the player lose all control and player mass increase by a certain factor
//? Unfroze = player shake left and right quick enough to break free

//? WARNING = DO NOT SET THE PREFAB's PLAYER CACHE TO THE PLAYER PREFAB (due to input manager shenanigans). Set it to the actual player in the game and not the player prefab
//TODO: unfreeze the player when you die
public class FreezingAreaController : MonoBehaviour
{
    [Header("Resource")]
    [SerializeField] private GameObject player;

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
    bool unfreezePlayerNow = false;
    float playerInitialMass;
    InputAction unfrozeAction;

    int wiggleCounter = 0;

    private void OnDisable()
    {
        RemoveInputCallback();
    }

    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        playerInitialMass = player.GetComponent<Rigidbody2D>().mass;
        unfrozeAction = playerController.GetComponent<PlayerInput>().actions["Unfroze"];
        Debug.Log($"The unfroze action {unfrozeAction}");
    }

    private void Update()
    {
        if (unfreezePlayerNow)
        {
            UnFrozePlayer();
            unfreezePlayerNow = false;
        }
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

            if (freezeCounter > freezeTime)
            {
                //playerFroze = true; // Call a function from the player controller here, or announce sth to the game controller
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

        SubscribeInputCallback();   //Freezing actions
        playerController.SwitchMap("Freezing");    //// Switching without unsubscribing, memory leak?
    }

    private void UnfrozeAction_canceled(InputAction.CallbackContext ctx)
    {
        Debug.Log("Bring player back to static sprite - The player didn't wiggle quick enough");
        wiggleCounter = 0;
    }

    private void UnfrozeAction_performed(InputAction.CallbackContext ctx)
    {
        wiggleCounter++;

        if (wiggleCounter > alternatePressCycle)
        {
            Debug.Log("UNFROZE THE PLAYER");
            unfreezePlayerNow = true;
        }
    }

    private void UnfrozeAction_started(InputAction.CallbackContext ctx)
    {
        Debug.Log("Show character freezing sprite shaking in place");
    }

    private void RemoveInputCallback()
    {
        unfrozeAction.started -= UnfrozeAction_started;
        unfrozeAction.performed -= UnfrozeAction_performed;
        unfrozeAction.canceled -= UnfrozeAction_canceled;
    }

    private void SubscribeInputCallback()
    {
        unfrozeAction.started += UnfrozeAction_started;
        unfrozeAction.performed += UnfrozeAction_performed;
        unfrozeAction.canceled += UnfrozeAction_canceled;
    }

    private void UnFrozePlayer()
    {
        player.GetComponent<Rigidbody2D>().mass = playerInitialMass;
        freezeCounter = 0;
        RemoveInputCallback();
        playerController.ResetActionMap();
        //Deactive freezing effects
    }
}
