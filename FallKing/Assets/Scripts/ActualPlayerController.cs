using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using Cinemachine;
using Unity.VisualScripting;

public class ActualPlayerController : MonoBehaviour
{
    [SerializeField] private float speed;

    private bool inWind = false;
    private Rigidbody2D rigidBody;


    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        
    }

    public void SetInWindyArea(bool value)
    {
        inWind = value;
    }

    void Update()
    {
        Debug.Log($"Player velocity {rigidBody.velocity}");

        float horizontalInput = Input.GetAxis("Horizontal");

        Vector2 movement = new Vector2(horizontalInput * this.speed, 0f);
       
        if (inWind)
        {
            rigidBody.AddForce(movement, ForceMode2D.Force);
            return;
        }

        this.rigidBody.velocity = new Vector2(movement.x, this.rigidBody.velocity.y);
    }
}