using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExtraPointsDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI extraPointBoostIsUsedText;
    private float displayDuration = 2f;
    private bool isDisplayingText = false;

    private void Start()
    {
        if (extraPointBoostIsUsedText == null)
        {
            Debug.LogError("TextMeshProUGUI component is not assigned.");
        }
    }

    public void DisplayBoostUsed()
    {
        if (!isDisplayingText)
        {
            StartCoroutine(DisplayBoostUsedCoroutine());
        }
    }

    private IEnumerator DisplayBoostUsedCoroutine()
    {
        isDisplayingText = true;
        if (extraPointBoostIsUsedText != null)
        {
            extraPointBoostIsUsedText.text = "Extra Points Collected";
            yield return new WaitForSeconds(displayDuration);
            extraPointBoostIsUsedText.text = "";
        }
        isDisplayingText = false;
    }
}
