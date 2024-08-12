using UnityEngine;
using UnityEngine.UI;

public class TransitionPanel : MonoBehaviour
{
    public static TransitionPanel Instance { get; private set; }
    public GameObject loadingPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        HideLoadingScreen();
    }

    public void ShowLoadingScreen()
    {
        loadingPanel.SetActive(true);
    }

    public void HideLoadingScreen()
    {
        loadingPanel.SetActive(false);
    }
}
