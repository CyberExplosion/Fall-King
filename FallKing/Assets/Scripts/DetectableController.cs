using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectableController : MonoBehaviour
{
    [SerializeField] private Transform physicalBodyPos;

    // Update is called once per frame
    void Update()
    {
        transform.position = physicalBodyPos.position;
    }
}
