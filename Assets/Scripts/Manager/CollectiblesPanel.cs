using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesPanel : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject itemPanel;
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        itemPanel.gameObject.SetActive(false);
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void OnItemButtonClicked()
    {
        audioManager.Play("ButtonClick");
        menuPanel.gameObject.SetActive(false);
        itemPanel.gameObject.SetActive(true);
    }

    public void OnMainMenuButtonClicked()
    {
        audioManager.Play("ButtonClick");
        itemPanel.gameObject.SetActive(false);
        menuPanel.gameObject.SetActive(true);
    }
}
