using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceObjectonPlane : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    private Pose placementPose;
    private bool placementPoseisValid;
    private bool isObjectPlaced;

    //Rotator script added

    public GameObject positionIndicator;
    public GameObject prefabToPlace;
    public Camera ARCamera;

    private void Awake(){
        raycastManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isObjectPlaced){
            UpdatePlacementPose();
            if(placementPoseisValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){
                PlaceObject();
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
        Instantiate(prefabToPlace, placementPose.position, placementPose.rotation);
        isObjectPlaced = true;
        positionIndicator.SetActive(false); 
    }

}
