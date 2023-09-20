using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefabToPlace;

    public ChoiceManager cm;
    public AssetBundleGetter asb;
    public UIControl Control;

    // Cache ARRaycastManager GameObject from ARCoreSession
    private ARRaycastManager _raycastManager;
    private ARAnchorManager _anchorManager;

    private static readonly List<ARRaycastHit> Hits = new List<ARRaycastHit>();

    private bool isAssetLoaded;

void Awake()
{
    _raycastManager = GetComponent<ARRaycastManager>();
    _anchorManager = GetComponent<ARAnchorManager>();
    asb.worldReady.AddListener(AssetLoaded);
}

    void AssetLoaded()
    {
        Debug.Log("Asset loaded in object placer");
        isAssetLoaded = true;
    }

ARAnchor CreateAnchor(in ARRaycastHit hit) {
    ARAnchor anchor;

    if (hit.trackable is ARPlane plane) {
        var planeManager = GetComponent<ARPlaneManager>(); if (planeManager)
        {
            var oldPrefab = _anchorManager.anchorPrefab; _anchorManager.anchorPrefab = _prefabToPlace;
            anchor = _anchorManager.AttachAnchor(plane, hit.pose); _anchorManager.anchorPrefab = oldPrefab;
            Debug.Log($"Created anchor attachment for plane (id: {anchor.nativePtr}).");
            return anchor;
        }
    }
    
    //here?
    var instantiatedObject = Instantiate(_prefabToPlace, hit.pose.position, hit.pose.rotation);
    anchor = instantiatedObject.GetComponent<ARAnchor>(); 
    
    if (anchor == null)
    {
        anchor = instantiatedObject.AddComponent<ARAnchor>(); 
    }
    Debug.Log($"Created regular anchor (id: {anchor.nativePtr})."); 

    return anchor;
}

    // Update is called once per frame
    void Update()
    {
       Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began){
        return; 
        }
        if (Control.Object_Style_CurrentActive != null) return;
        if (!isAssetLoaded) return;
        if (_raycastManager.Raycast(touch.position, Hits, TrackableType.Planes)) {
            var hitPose = Hits[0].pose;
            Debug.Log("trying to instasiate anchor");
            GameObject go = CreateAnchor(Hits[0]).gameObject;
            Debug.Log("done");

            //find corret clothing item in scene
            GameObject clo = GameObject.Find(cm.currentClothing);
            Debug.Log("Found:"+clo.name);
            //attach to our newly instnsiated object and zero it's local position
            clo.transform.parent = go.transform;
            clo.transform.localEulerAngles = Vector3.zero;
            clo.transform.localPosition = Vector3.zero;
            go.GetComponent<Renderer>().enabled = false;

            Control.Control_SetActiveObject(clo);

            Debug.Log($"Instantiated on: {Hits[0].hitType}");
        } 
    }
}