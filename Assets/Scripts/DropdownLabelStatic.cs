using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropdownLabelStatic : MonoBehaviour
{
    private TMP_Dropdown dropdown;
    private TMP_Text originalLabel;

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        originalLabel = dropdown.captionText;
    }

    private void Start()
    {
        RestoreLabel();
    }

    private void OnEnable()
    {
        RestoreLabel();
    }

    private void RestoreLabel()
    {
        dropdown.captionText = originalLabel;
        dropdown.RefreshShownValue();
    }  
}
