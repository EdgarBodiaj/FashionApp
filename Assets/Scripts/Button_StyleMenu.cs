using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Button_StyleMenu : MonoBehaviour
{
    public string Name;
    public int ID;
    public string Description;
    public int size;

    public TMP_Text Title;
    public TMP_Text DescriptionText;
    public TMP_Text Size;

    public UIControl Control;

    // Start is called before the first frame update
    void Start()
    {
        Control = GameObject.FindGameObjectWithTag("Control").GetComponent<UIControl>();
        GetComponentInChildren<Button>().onClick.AddListener(() => Control.Menu_SetCurrentStyle(ID));
        Title.text = Name;
        DescriptionText.text = Description;
        Size.text = "" + size;
    }
}
