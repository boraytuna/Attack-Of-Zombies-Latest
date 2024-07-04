using UnityEngine;

// Changes the alpha level of the buildings to see the player
public class TransparencyController : MonoBehaviour
{
    private Material material;
    private float targetAlpha;
    private Color originalColor;

    public float alphaValue = 0.4f;
    public float lerpSpeed = 5f;

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material;
            originalColor = material.color;
            targetAlpha = originalColor.a;
        }
        else
        {
            Debug.LogError("Renderer component not found on object.");
        }
    }

    private void Update()
    {
        Color color = material.color;
        color.a = Mathf.Lerp(color.a, targetAlpha, Time.deltaTime * lerpSpeed);
        material.color = color;
    }

    public void SetTargetAlpha(float alpha)
    {
        targetAlpha = alpha;
    }
}
