using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] private Transform physicalBodyPos;

    [HideInInspector]
    public bool detectedPlayer = false;

    private void Update()
    {
        transform.position = physicalBodyPos.position;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //FindObjectOfType<SoundManager>().PlaySoundEffect("Hit");
        Debug.Log("The layer that involves with the contacts");
        if (collision.CompareTag("Player"))
        {
            //FindObjectOfType<SoundManager>().PlaySoundEffect("Hit");
            detectedPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            detectedPlayer = false;
        }
    }
}
