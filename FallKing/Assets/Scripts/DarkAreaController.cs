using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class DarkAreaController : MonoBehaviour
{
    [SerializeField] Transform physicalPlayer;

    [Tooltip("Use to get the levels the player is currenlt in")]
    [SerializeField] CinemachineVirtualCamera virtualCamera;

    Renderer darkBgRenderer;
    Transform currentLevel;

    public List<Transform> limitedVisbilityLevels = new List<Transform>();

    private void Start()
    {
        darkBgRenderer = gameObject.transform.Find("DarkBackground").GetComponent<Renderer>();
        darkBgRenderer.enabled = false;
    }

    private void Update()
    {
        currentLevel = virtualCamera.Follow;
        if (limitedVisbilityLevels.Contains(currentLevel))
        {
            darkBgRenderer.enabled = true;
            transform.position = physicalPlayer.position;
        }
        else
        {
            darkBgRenderer.enabled = false;
        }
    }
}
