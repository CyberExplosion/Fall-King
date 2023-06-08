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
        Debug.Log("The layer that involves with the contacts");
        Collider2D[] res = new Collider2D[10];
        int numContacts = collision.GetContacts(res);
        for (int i = 0; i < numContacts; i++)
        {
            Debug.Log($"The contact name is {res[i].name}");
        }
        detectedPlayer = true;
        //Debug.Log("TRIGGER");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedPlayer = false;
        //Debug.Log("EXITTTT");
    }
}
