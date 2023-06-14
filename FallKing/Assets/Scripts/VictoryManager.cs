using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    public GameObject menu;

    private void OnCollisionEnter2D(Collision2D other)
    {   
        // Player collides with Princess, i.e. victory
        if (other.gameObject.tag == "Player")
        {
            Time.timeScale = 0;
            FindObjectOfType<SoundManager>().StopMusicTrack();
            FindObjectOfType<SoundManager>().PlaySoundEffect("Victory");
            menu.SetActive(true);
        }
    }
}
