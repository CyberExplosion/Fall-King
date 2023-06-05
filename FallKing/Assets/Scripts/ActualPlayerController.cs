using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using Cinemachine;

public class ActualPlayerController : MonoBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody2D rigidBody;


    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }


    void Update()
    {

        float horizontalInput = Input.GetAxis("Horizontal");

        this.rigidBody.velocity = new Vector2(horizontalInput * this.speed, this.rigidBody.velocity.y);
    }
}