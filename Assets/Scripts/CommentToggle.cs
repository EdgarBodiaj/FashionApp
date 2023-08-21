using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentToggle : MonoBehaviour
{
    public void ToggleMenu()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
