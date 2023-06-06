using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using Cinemachine;

//Todo: When player release the key, force the user to stop (currently using add force make the user keep sliding)
//TODO: The user will keep a minimal amount of momentum when stop?
//! Add a huge force to the opposite moving direction?
public class Respawn : MonoBehaviour
{
    [SerializeField] private Transform respawnLevel;
    [SerializeField] private GameObject playerObj;

    private Transform respawnPoint;
    private CinemachineVirtualCamera virtualCamera;
    private Rigidbody2D rigidBody;

    void Start()
    {
        respawnPoint = respawnLevel.Find("StageRespawnPoint");
        rigidBody = GetComponent<Rigidbody2D>();
        this.virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }

    public void setRespawnLevel(Transform newRespawn)
    {
        //Debug.Log($"This respawn point {respawnLevel.position}");
        //Debug.Log($"New respawn point {newRespawn.position}");
        this.respawnLevel = newRespawn;
        respawnPoint = respawnLevel.Find("StageRespawnPoint");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Collide with any tile collision box
        if (collision.gameObject.tag == "Ground")
        {
            FindObjectOfType<SoundManager>().PlaySoundEffect("Death");
            virtualCamera.Follow = this.respawnLevel;
            playerObj.transform.position = new Vector2(this.respawnPoint.position.x, this.respawnPoint.position.y);
            //Debug.Log($"The player position {playerObj.transform.position}");
            this.rigidBody.velocity = new Vector2(0, 0);
        }
    }
}
