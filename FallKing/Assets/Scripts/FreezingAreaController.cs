using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


//? Froze = the player lose all control and player mass increase by a certain factor
public class FreezingAreaController : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [Tooltip("The factor to apply to player current movement speed")]
    [SerializeField] private float speedFactor = 1.0f;
    [Tooltip("The time it takes for the target to become frozen in this area")]
    [SerializeField] private float freezeTime = 5.0f;

    float freezeCounter = 0;
    bool playerFroze = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        freezeCounter = 0;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().CompareTag("Player"))
        {
            Debug.LogError($"The player velocity {collision.GetComponent<Rigidbody2D>().velocity}");
            collision.GetComponent<Rigidbody2D>().velocity = collision.GetComponent<Rigidbody2D>().velocity * speedFactor;

            if (freezeCounter > freezeTime && !playerFroze)
            {
                playerFroze = true; // Call a function from the player controller here, or announce sth to the game controller
            }
            freezeCounter += Time.deltaTime;
        }
    }


    //Freeze = keep the same velocity and momentum, but player cannot control anymore
    private void FreezePlayer()
    {
        player.GetComponent<PlayerInput>().actions.Disable();
        // Activate freezing effect here
    }

    private void UnFrozePlayer()
    {
        player.GetComponent<PlayerInput>().actions.Enable();
        //Deactive freezing effects
    }
}
