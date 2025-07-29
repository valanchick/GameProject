using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void setMaxAmount(float amount)
    {
        slider.maxValue = amount;
        slider.value = amount;
        gradient.Evaluate(1f);
    }

    public void setAmount(float amount)
    {
        slider.value = amount;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
