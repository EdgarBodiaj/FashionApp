using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UIControl : MonoBehaviour
{
    public GameObject Toolbar_Bottom;
    public GameObject Toolbar_Right;

    public GameObject Tool_Torch;
    public GameObject Tool_TopLight;
    public GameObject Tool_ToplightRotationSphere;

    public GameObject Tool_TopLightRotationControl;

    public GameObject Tool_Light_Brightness;
    public GameObject Tool_Light_Temperature;

    public GameObject Menu_Style;

    public GameObject Menu_Hamburger;

    public GameObject Menu_Dialog;

    public GameObject Object_Style_CurrentActive;

    List<string> Tool_RotateDir;
    Vector3 Tool_EulerRot;

    private void Start()
    {
        Tool_ToplightRotationSphere = Tool_TopLight.GetComponentInParent<Transform>().parent.gameObject;
        Tool_RotateDir = new List<string>();
        GraphicsSettings.lightsUseColorTemperature = true;
        GraphicsSettings.lightsUseLinearIntensity = true;
        Tool_TopLight.GetComponent<Light>().useColorTemperature = true;
        Tool_Torch.GetComponent<Light>().useColorTemperature = true;
        
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
            Tool_EulerRot.y = 0;
            Tool_ToplightRotationSphere.transform.Rotate(Tool_EulerRot);
        }
    }

    public void Tool_Light_SetTemperature(float temp)
    {
        Debug.Log(temp);
        Tool_TopLight.GetComponent<Light>().colorTemperature = temp;
        Tool_Torch.GetComponent<Light>().colorTemperature = temp;
    }

    public void Tool_Light_SetBrightness(float bright)
    {
        Tool_TopLight.GetComponent<Light>().intensity = bright;
        Tool_Torch.GetComponent<Light>().intensity = bright;
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

    public void Toolbar_Light_ToggleTemperature()
    {
        Tool_Light_Temperature.SetActive(!Tool_Light_Temperature.activeInHierarchy);
    }

    public void Toolbar_Light_ToggleBrightness()
    {
        Tool_Light_Brightness.SetActive(!Tool_Light_Brightness.activeInHierarchy);
    }

    public void Menu_ToggleHamburgerMenu()
    {
        Menu_Hamburger.SetActive(!Menu_Hamburger.activeInHierarchy);
    }

    public void Menu_ToggleStyleMenu()
    {

    }

    public void Menu_SetCurrentStyle()
    {

    }

    public void Toolbar_ToggleScale()
    {

    }

    public void Toolbar_ScaleCurrentObject()
    {

    }

    public void Toolbar_ToggleRotate()
    {

    }

    public void Toolbar_RotateCurrentObject()
    {

    }

    public void Toolbar_ToggleMove()
    {

    }

    public void Toolbar_MoveCurrentObject()
    {

    }

    public void Toolbar_DeleteDialog()
    {
        Menu_Dialog.SetActive(!Menu_Dialog.activeInHierarchy);
    }

    public void Toolbar_DeleteCurrentObject()
    {

    }
}
