using System.Collections;
using System.Collections.Generic;
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
    // Start is called before the first frame update
    void Start()
    {
        setCurrentClothing(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setCurrentClothing(int s){

            if(currentClothing != null){
                GameObject.Destroy(GameObject.Find(currentClothing),0);
            }

            GameObject go = Instantiate(prefabs[s]);
            go.transform.parent = arCamera;
            go.transform.localPosition = new Vector3(0,0,-100);
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
