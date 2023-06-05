using UnityEngine;

public class WindAreaController : MonoBehaviour
{
    /*[Tooltip("The time interval in which the force will change to the opposite direction")]
    [SerializeField] private float timeBetForceDirChange = 1f;
    [Tooltip("The duration of when the wind will be on")]
    [SerializeField] private float windForceDuration = 1f;
    [Tooltip("The duration of when there is no wind")]
    [SerializeField] private float noForceDuration = 2f;

    private AreaEffector2D areaEffector;
    private float initialForceMagnitude;
    bool windForceActive = true;
    private float windForceTimer = 0f;
    private float noForceTimer = 0f;
    private float forceChangeTimer = 0f;

    void Start()
    {
        areaEffector = GetComponent<AreaEffector2D>();
        initialForceMagnitude = areaEffector.forceMagnitude;
    }

    // Update is called once per frame
    void Update()
    {
        if (windForceActive && windForceTimer < windForceDuration)
        {
            //Debug.Log("Currently have force");
            areaEffector.forceMagnitude = initialForceMagnitude;
            windForceTimer += Time.deltaTime;
        }
        else
        {
            windForceTimer = 0f;
            windForceActive = false;
        }

        if (!windForceActive && noForceTimer < noForceDuration)
        {
            //Debug.Log("Currently no force");
            areaEffector.forceMagnitude = 0f;
            noForceTimer += Time.deltaTime;
        }
        else
        {
            noForceTimer = 0f;
            windForceActive = true;
        }

        if (forceChangeTimer > timeBetForceDirChange)
        {
            areaEffector.forceAngle = (areaEffector.forceAngle + 180) % 360;
            forceChangeTimer = 0f;
        }
        forceChangeTimer += Time.deltaTime;
    }*/
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Player entered the windy area
            ActualPlayerController player = other.GetComponent<ActualPlayerController>();
            if (player != null)
            {
                player.SetInWindyArea(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Player exited the windy area
            ActualPlayerController player = other.GetComponent<ActualPlayerController>();
            if (player != null)
            {
                player.SetInWindyArea(false);
            }
        }
    }
}
