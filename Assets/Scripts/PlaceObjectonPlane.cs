using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceObjectonPlane : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    private Pose placementPose;
    private bool placementPoseisValid;
    private bool isObjectPlaced;

    public TMP_Text debugText;

    //Rotator script added

    public GameObject positionIndicator;
    public GameObject prefabToPlace;
    public Camera ARCamera;

    public UIControl UIControl;

    private void Start()
    {
       UIControl = GameObject.FindGameObjectWithTag("Control").GetComponent<UIControl>(); 
    }

    private void Awake(){
        raycastManager = GetComponent<ARRaycastManager>();
        if (debugText != null) debugText = GameObject.FindGameObjectWithTag("Debug").GetComponent<TMP_Text>();
        debugText.text = "Debug started";
    }

    // Update is called once per frame
    void Update()
    {
        if(!isObjectPlaced){

            UpdatePlacementPose();
            if(placementPoseisValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){
                if(UIControl.Object_Style_CurrentActive == null) PlaceObject();
            }
        }
    }

    private void UpdatePlacementPose(){
        var screenCenter = ARCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();

        raycastManager.Raycast(screenCenter, hits, TrackableType.All); 

        placementPoseisValid = hits.Count > 0;

        if (placementPoseisValid){
            placementPose = hits[0].pose;
            var cameraForward = ARCamera.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;

            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
            positionIndicator.SetActive(true);
            positionIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else{
            positionIndicator.SetActive(false);
        }
    }

    private void PlaceObject(){
        GameObject newObj =  Instantiate(prefabToPlace, placementPose.position, placementPose.rotation);
        UIControl.Control_SetActiveObject(newObj);
        isObjectPlaced = true;
        positionIndicator.SetActive(false); 
    }

}
