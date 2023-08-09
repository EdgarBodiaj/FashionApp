using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    public GameObject Toolbar_Bottom;
    public GameObject Toolbar_Right;

    public GameObject Tool_Torch;
    public GameObject Tool_TopLight;
    GameObject Tool_ToplightRotationSphere;

    public GameObject Tool_TopLightRotationControl;
    private void Start()
    {
        Tool_ToplightRotationSphere = Tool_TopLight.GetComponentInParent<Transform>().gameObject;
    }

    public void Toolbar_ToggleToolbox()
    {
        Toolbar_Bottom.SetActive(!Toolbar_Bottom.activeInHierarchy);
        Toolbar_Right.SetActive(!Toolbar_Right.activeInHierarchy);
    }

    public void Toolbar_ToggleTorch()
    {
        Tool_Torch.SetActive(!Tool_Torch.activeInHierarchy);
    }

    public void Toolbar_Toplight_Rotate(string dir)
    {
        //Direction Code:
        //00 = Up, X+
        //01 = Right, Z-
        //11 = Down, X-
        //10 = Left, Z+
        switch (dir)
        {
            case "00":
                Debug.Log("Up");
                break;
            case "01":
                Debug.Log("Right");
                break;
            case "11":
                Debug.Log("Down");
                break;
            case "10":
                Debug.Log("Left");
                break;
            default:
                break;
        }

    }

    public void Toolbar_ToggleLightAngleControl()
    {
        Tool_TopLightRotationControl.SetActive(!Tool_TopLightRotationControl.activeInHierarchy);
    }
}
