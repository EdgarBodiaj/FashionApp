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

    public GameObject Tool_Object_Move;
    public GameObject Tool_Object_Rotate;
    public GameObject Tool_Object_Scale;

    public GameObject Menu_Style;
    public GameObject Menu_Hamburger;
    public GameObject Menu_Dialog;

    public GameObject Object_Style_CurrentActive;

    List<string> Tool_RotateDir;
    Vector3 Tool_EulerRot;

    List<string> Tool_MoveDir;
    Vector3 Tool_MovePos;

    private void Start()
    {
        Tool_ToplightRotationSphere = Tool_TopLight.GetComponentInParent<Transform>().parent.gameObject;
        Tool_RotateDir = new List<string>();
        Tool_MoveDir = new List<string>();
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

        if(Tool_MoveDir.Count > 0 && Object_Style_CurrentActive != null)
        {
            Tool_MovePos = Object_Style_CurrentActive != null ? Object_Style_CurrentActive.transform.position : new Vector3(0,0,0);
            //Positive X
            if (Tool_MoveDir.Exists(x => x == "00")) Tool_MovePos.x += 0.002f;
            //Negative X
            else if (Tool_MoveDir.Exists(x => x == "11")) Tool_MovePos.x -= 0.002f;
            //Negative Z
            if (Tool_MoveDir.Exists(x => x == "01")) Tool_MovePos.z -= 0.002f;
            //Positive Z
            else if (Tool_MoveDir.Exists(x => x == "10")) Tool_MovePos.z += 0.002f;

            Object_Style_CurrentActive.transform.position = Tool_MovePos;
        }
    }

    public void Tool_Light_SetTemperature(float temp)
    {
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

    public void Toolbar_Movement_Move(string dir)
    {
        //Direction Code:
        //00 = Up, X+
        //01 = Right, Z-
        //11 = Down, X-
        //10 = Left, Z+
        if (Tool_MoveDir.Exists(x => x == dir)) Tool_MoveDir.Remove(dir);
        else Tool_MoveDir.Add(dir);
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
        Menu_Style.SetActive(!Menu_Style.activeInHierarchy);
    }

    public void Menu_SetCurrentStyle(int i)
    {
        Debug.Log("Style " + i + " pressed");
    }

    public void Toolbar_ToggleScale()
    {
        Tool_Object_Scale.SetActive(!Tool_Object_Scale.activeInHierarchy);
    }

    public void Toolbar_ScaleCurrentObject(float scale)
    {
        if(Object_Style_CurrentActive != null)
        {
            Object_Style_CurrentActive.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    public void Toolbar_ToggleRotate()
    {
        Tool_Object_Rotate.SetActive(!Tool_Object_Rotate.activeInHierarchy);
    }

    public void Toolbar_RotateCurrentObject(float y)
    {
        if(Object_Style_CurrentActive != null)
        {
            Object_Style_CurrentActive.transform.rotation = Quaternion.Euler(0, y, 0);
        }
    }

    public void Toolbar_ToggleMove()
    {
        Tool_Object_Move.SetActive(!Tool_Object_Move.activeInHierarchy);
    }

    public void Toolbar_DeleteDialog()
    {
        Menu_Dialog.SetActive(true);
    }

    public void Toolbar_Dialog_DeleteCurrentObject()
    {
        Destroy(Object_Style_CurrentActive);
        Object_Style_CurrentActive = null;
        Menu_Dialog.SetActive(false);
    }
    
    public void Toolbar_Dialog_CancelDelete()
    {
        Menu_Dialog.SetActive(false);
    }

    public void Control_SetActiveObject(GameObject obj)
    {
        Object_Style_CurrentActive = obj;
    }
}
