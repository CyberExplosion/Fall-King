using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageNameOverlay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private string textValue;

    private float typingSpeed = 0.1f;
    private float animationLength = 3.0f;
    private Coroutine typingRoutine;

    void Start()
    {
        textMeshPro.text = string.Empty;
    }

    public void StartTyping()
    {
        if (this.typingRoutine != null)
        {
            StopCoroutine(this.typingRoutine);
        }

        this.typingRoutine = StartCoroutine(TypeStageName());
    }

    IEnumerator TypeStageName()
    {
        foreach (char letter in this.textValue)
        {
            this.textMeshPro.text += letter;
            yield return new WaitForSeconds(this.typingSpeed);
        }

        yield return new WaitForSeconds(this.animationLength);

        for (int i = textMeshPro.text.Length - 1; i >= 0; i--)
        {
            this.textMeshPro.text = textMeshPro.text.Remove(i);
            yield return new WaitForSeconds(this.typingSpeed);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            this.StartTyping();
        }
    }
}
