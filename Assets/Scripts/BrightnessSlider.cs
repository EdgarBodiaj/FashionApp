using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrightnessSlider : MonoBehaviour
{
    public Light areaLight;
    public Slider brightnessSlider;

    private void Start()
    {
        brightnessSlider.onValueChanged.AddListener(AdjustBrightness);
        areaLight = GetComponent<Light>();
    }

    private void AdjustBrightness(float value)
    {
        areaLight.intensity = value;
    }
}

