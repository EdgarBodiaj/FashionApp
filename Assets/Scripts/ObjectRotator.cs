using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ObjectRotator : MonoBehaviour
{
    public ARRaycastManager arRaycastManager;
    public GameObject prefabToPlace;
    public Slider rotationSlider;
    public Transform rotationAxis;

    private GameObject placedObject;

    void Start()
    {
        rotationSlider.onValueChanged.AddListener(UpdateRotationAngle);
    }

    void Update()
    {
        if (placedObject != null)
        {
            // Rotate the placed object based on the slider value
            placedObject.transform.rotation = Quaternion.Euler(rotationAxis.eulerAngles.x, rotationSlider.value, rotationAxis.eulerAngles.z);
        }
    }

    public void UpdateRotationAngle(float angle)
    {
        // Update the rotation angle based on the slider value
        if (placedObject != null)
        {
            placedObject.transform.rotation = Quaternion.Euler(rotationAxis.eulerAngles.x, angle, rotationAxis.eulerAngles.z);
        }
    }

    public void PlacePrefab()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        if (arRaycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            placedObject = Instantiate(prefabToPlace, hitPose.position, hitPose.rotation);
        }
    }
}