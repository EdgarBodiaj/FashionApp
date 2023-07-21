using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightnessToggle : MonoBehaviour
{
    public void ToggleSlider()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
