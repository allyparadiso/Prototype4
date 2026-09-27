using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SanityManager : MonoBehaviour
{
    public float maxSanity = 100f;
    public float currentSanity;
    public float baseDrainRate = 1f;

    public Slider sanitySlider;

    public UnityEvent onInsane;

    private bool isDraining = true;

    private void Start()
    {
        currentSanity = maxSanity;
        if (sanitySlider != null )
        {
            sanitySlider.maxValue = maxSanity;
            sanitySlider.value = currentSanity;
        }
    }

    private void Update()
    {
        if (isDraining)
        {
            ChangeSanity(-baseDrainRate * Time.deltaTime);
        }
    }

    public void ChangeSanity(float amount)
    {
        currentSanity += amount;
        currentSanity = Mathf.Clamp(currentSanity, 0f, maxSanity);

        if (sanitySlider != null)
        {
            sanitySlider.value = currentSanity;
        }

        if (currentSanity <= 0f)
        {
            onInsane?.Invoke();
        }
    }

    public void SetDrainStatus(bool status)
    {
        isDraining = status;
    }
}
