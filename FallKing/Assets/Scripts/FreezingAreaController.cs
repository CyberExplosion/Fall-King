using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


//? Froze = the player lose all control and player mass increase by a certain factor
//? Unfroze = player shake left and right quick enough to break free
public class FreezingAreaController : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [Tooltip("The factor to apply to player current movement speed")]
    [SerializeField] private float speedFactor = 1.0f;
    [Tooltip("The time it takes for the target to become frozen in this area")]
    [SerializeField] private float freezeTime = 5.0f;
    [Tooltip("The factor to apply to target mass once it froze")]
    [SerializeField] private float frozenMassFactor = 1.5f;
    [Tooltip("The number of alternating key press cycle to unfroze the player")]
    [SerializeField] private int alternatePressCycle = 2;

    float freezeCounter = 0;
    bool playerFroze = false;
    float playerInitialMass;

    private void Start()
    {
        playerInitialMass = player.GetComponent<Rigidbody2D>().mass;
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
        InputAction moveAction = player.GetComponent<PlayerInput>().actions["move"];
        //Debug.LogError($"input: {moveAction}");
        if (moveAction.triggered)
        {
            //Debug.LogError("Moving right now");
        }
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
        player.GetComponent<PlayerInput>().actions.Disable();
        player.GetComponent<Rigidbody2D>().mass = player.GetComponent<Rigidbody2D>().mass * frozenMassFactor;
        //Debug.LogError($"After froze mass {player.GetComponent<Rigidbody2D>().mass}");
        // Activate freezing effect here
    }

    private void UnFrozePlayer()
    {
        player.GetComponent<PlayerInput>().actions.Enable();
        player.GetComponent<Rigidbody2D>().mass = playerInitialMass;
        //Deactive freezing effects
    }
}
