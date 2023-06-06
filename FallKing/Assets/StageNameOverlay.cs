using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageNameOverlay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private string textValue;

    // Update is called once per frame
    void Update()
    {
        this.textMeshPro.text = this.textValue;
    }
}
