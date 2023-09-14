using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadExample : MonoBehaviour
{
    public Dropdown drop;

    void Start()
    {
        GetComponent<AssetBundleGetter>().assetbundleDatabaseReady.AddListener(populateDropdown);
    }

    public void loadFromTextBox()
    {
        GetComponent<AssetBundleGetter>().startLoadAssetsForTicket(drop.options[drop.value].text);
    }

    public void populateDropdown()
    {
        string[] options = GetComponent<AssetBundleGetter>().getOptions();
        List<string> l = new List<string>();
        foreach(string s in options)
        {
            l.Add(s);
        }
        drop.AddOptions(l);
        Debug.Log("Added options");
    }
}
