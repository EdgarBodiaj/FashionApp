using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    public GameObject Toolbar_Bottom;
    public GameObject Toolbar_Right;

    public GameObject Tool_Torch;
    public GameObject Tool_TopLight;
    public GameObject Tool_ToplightRotationSphere;

    public GameObject Tool_TopLightRotationControl;

    public List<string> Tool_RotateDir;
    public Quaternion Tool_RotationQuart;
    public Vector3 Tool_EulerRot;

    private void Start()
    {
        Tool_ToplightRotationSphere = Tool_TopLight.GetComponentInParent<Transform>().parent.gameObject;
        Tool_RotateDir = new List<string>();
    }

    private void FixedUpdate()
    {
        if (Tool_RotateDir.Count > 0)
        {
            Tool_EulerRot = new Vector3(0, 0, 0);
            //Positive X
            if (Tool_RotateDir.Exists(x => x == "00")) Tool_EulerRot.x = 2;
            //Negative X
            else if (Tool_RotateDir.Exists(x => x == "11")) Tool_EulerRot.x = -2;
            //Negative Z
            if (Tool_RotateDir.Exists(x => x == "01")) Tool_EulerRot.z = -2;
            //Positive Z
            else if (Tool_RotateDir.Exists(x => x == "10")) Tool_EulerRot.z = 2;
            Debug.Log(Tool_EulerRot);
            Tool_EulerRot.y = 0;
            Tool_ToplightRotationSphere.transform.Rotate(Tool_EulerRot);
        }
    }

    private void Update()
    {
        
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

    public void Toolbar_Toplight_ToggleToolbar()
    {
        Tool_TopLightRotationControl.SetActive(!Tool_TopLightRotationControl.activeInHierarchy);
    }

    public void Toolbar_Toplight_Rotate(string dir)
    {
        //Direction Code:
        //00 = Up, X+
        //01 = Right, Z-
        //11 = Down, X-
        //10 = Left, Z+
        if(Tool_RotateDir.Exists(x => x == dir)) Tool_RotateDir.Remove(dir);
        else Tool_RotateDir.Add(dir);

    }

    public void Toolbar_Toplight_Reset()
    {
        Tool_ToplightRotationSphere.transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public void Toolbar_Toplight_Toggle()
    {
        Tool_TopLight.SetActive(!Tool_TopLight.activeInHierarchy);
    }

    public void Toolbar_ToggleLightAngleControl()
    {
        Tool_TopLightRotationControl.SetActive(!Tool_TopLightRotationControl.activeInHierarchy);
    }
}
