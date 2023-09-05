using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CommentMaker : MonoBehaviour
{
    public TMP_Text DebugText;
    public TMP_Text DebugText2;

    private ARRaycastManager raycastManager;
    private static readonly List<ARRaycastHit> Hits = new List<ARRaycastHit>();

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }
    void Update()
    {
        
        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            Debug.Log("Screen not touched");
            return;
        }
        Debug.Log("Screen touched!------------------------------------");
        if(raycastManager.Raycast(touch.position, Hits))
        {
            DebugText.text = Hits[0].pose.ToString() + ", hit count: " + Hits.Count;

            RaycastHit hit;

            Physics.Raycast(touch.position, new Vector3(0,0,1), out hit);

            DebugText2.text = hit.collider.gameObject.name;
        }
        
    }
}
