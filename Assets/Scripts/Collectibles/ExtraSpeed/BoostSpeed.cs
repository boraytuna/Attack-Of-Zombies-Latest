using System.Collections;
using UnityEngine;

public class BoostSpeed : MonoBehaviour
{
    private CollectibleManager collectibleManager;

    void Start()
    {
        collectibleManager = CollectibleManager.Instance;
    }

    public void OnBoostButton()
    {
        collectibleManager.OnSpeedBoosterButton();
    }
}