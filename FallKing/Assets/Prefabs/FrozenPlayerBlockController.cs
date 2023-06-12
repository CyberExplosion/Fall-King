using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenPlayerBlockController : MonoBehaviour
{
    [SerializeField] private Transform physicalPlayer;

    // Update is called once per frame
    void Update()
    {
        transform.position = physicalPlayer.position;    
    }
}
