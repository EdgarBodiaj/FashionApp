using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ChoiceManager : MonoBehaviour
{
    public string currentClothing;
    public GameObject[] prefabs;
    public Transform arCamera;
    public ARPlaneManager arpm;

    public GameObject LoadedAsset;

    public TMP_Text InfoDebug;

    // Start is called before the first frame update
    void Start()
    {
        //setCurrentClothing(0);
        LoadedAsset = GameObject.FindGameObjectWithTag("LoadedAsset");
    }

    public void setCurrentClothingLoaded()
    {
        //StartCoroutine(waitForClothLoad());
    }

    IEnumerator waitForClothLoad()
    {
        while(LoadedAsset.GetComponentsInChildren<Transform>().Length < 1)
        {
            Debug.Log("Not Loaded");
            yield return null;
        }
        Debug.Log("Done");
        GameObject child = LoadedAsset.GetComponentsInChildren<Transform>()[0].gameObject;
        child.transform.parent = arCamera;
        //child.transform.localPosition = new Vector3(0, 0, -100);
        child.transform.rotation = new Quaternion(0,180,0,0);
        currentClothing = child.name;
        InfoDebug.text = child.name;
        Debug.Log("Created" + child.name);
    }

    public void setCurrentClothing(int s){

            if(currentClothing != null){
                GameObject.Destroy(GameObject.Find(currentClothing),0);
            }

            GameObject go = Instantiate(prefabs[s]);
            go.transform.parent = arCamera;
            go.transform.localPosition = new Vector3(0,0,-100);
            go.transform.localRotation = new Quaternion(0,180,0,0);
            currentClothing = go.name;
            Debug.Log("Created" + go.name);

            // if(s == 1){
            //     arpm.requestedDetectionMode = PlaneDetectionMode.Vertical;
            // }
            // else{
            //     arpm.requestedDetectionMode = PlaneDetectionMode.Horizontal;
            // }
    }
}
