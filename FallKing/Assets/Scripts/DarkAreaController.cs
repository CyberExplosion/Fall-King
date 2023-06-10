using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class DarkAreaController : MonoBehaviour
{
    [SerializeField] Transform playerPosition;
    [Tooltip("Level that use this limited visibility on player")]

    Renderer darkBgRenderer;

    //[HideInInspector]
    public bool activateDarkArea = false;

    private void Start()
    {
        darkBgRenderer = gameObject.transform.Find("DarkBackground").GetComponent<Renderer>();
    }

    private void Update()
    {
        if (activateDarkArea)
        {
            darkBgRenderer.enabled = true;
            transform.position = playerPosition.position;
        }
        else
        {
            darkBgRenderer.enabled = false;
        }
    }
}
