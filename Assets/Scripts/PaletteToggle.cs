using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteToggle : MonoBehaviour
{
    public void TogglePicker()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
