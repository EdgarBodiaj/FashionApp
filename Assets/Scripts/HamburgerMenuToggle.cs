using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamburgerMenuToggle : MonoBehaviour
{
    public void ToggleMenu()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
