using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System.CodeDom.Compiler;
using System.Linq;

public class AssetBundleGetter : MonoBehaviour
{

    //public string bundleRegisterURL = "http://paultennent.co.uk/bundles/erica/assetbundleregister.txt";
    public string bundleRegisterURL = "https://future-festivals.mrl.nottingham.ac.uk/Paul_Asset_Bundles.txt";
    public string bundleBaseURL = "https://future-festivals.mrl.nottingham.ac.uk/Bundles/";

    public float timeout = 120; //time to try and get stuff in seconds;

    public float downloadProgress = 0;

    private bool registerDownloaded = false;
    private string register;
    private string[] options;

    public Transform parentForWorld;
    public UnityEvent assetbundleDatabaseReady;

    public UnityEvent downloadFailed;

    public UnityEvent downloadStarted;
    public UnityEvent downloadComplete;

    public UnityEvent worldReady;

    private bool downloading = false;
    List<string> Register;

    private bool worldDownloaded = false;

    public string serverURL = "";


    // Start is called before the first frame update
    void Start()
    {
        downloadFailed.AddListener(LogDownloadFailed);
        assetbundleDatabaseReady.AddListener(LogAssetBundleDBReady);
        Register = new List<string>();
        //lets unload any existing ones
        //AssetBundle.UnloadAllAssetBundles(true);
        StartCoroutine(GetRegister());
    }

    public List<string> askRegister()
    {
        return Register;
    }

    public string[] getOptions()
    {
        return options;
    }

    private void LogAssetBundleDBReady()
    {
        Debug.Log("Assetbundle Database Downloaded");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startLoadAssetsForTicket(string ticket)
    {
        StartCoroutine(LoadAssetsForTicket(ticket));
    }

    public IEnumerator LoadAssetsForTicket(string ticket)
    {
        Debug.Log("Start load assets for ticket:" + ticket);
        //StartCoroutine(GetRegister());
        float timeOutTimer = 0;
        while (!registerDownloaded && timeOutTimer < timeout)
        {
            timeOutTimer += Time.deltaTime;
            yield return null;
        }

        string url = bundleBaseURL + ticket;
        if (url != null)
        {
            StartCoroutine(GetAndSpawn(url));
        }

        while (!worldDownloaded && timeOutTimer < timeout)
        {
            timeOutTimer += Time.deltaTime;
            yield return null;
        }

    }



    public IEnumerator GetRegister()
    {
        var request = UnityWebRequest.Get(bundleRegisterURL);
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        //if (!request.isDone)
        {
            Debug.Log(request.error);
            downloadFailed.Invoke();
            StopAllCoroutines();
        }
        else
        {
            register = request.downloadHandler.text;
            options = register.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            dumpStrings(options);
            foreach (var item in options)
            {
                GetComponent<StylesPopulteMenu>().AddCard(item);
            }
            registerDownloaded = true;
            assetbundleDatabaseReady.Invoke();
        }
    }

    public IEnumerator GetAndSpawn(string url)
    {
        var request = UnityWebRequestAssetBundle.GetAssetBundle(url, 0);
        downloading = true;
        Debug.Log(url);
        StartCoroutine(downloadProgressMonitor(request));
        yield return request.SendWebRequest();
        downloading = false;

        if (request.result != UnityWebRequest.Result.Success)
        //if (!request.isDone)
        {
            Debug.Log(request.error);
            downloadFailed.Invoke();
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
            //string name = bundle.GetAllAssetNames()[0];

            //need to extract the names of all the assets and handle the scripts afterwards.
            string[] names = bundle.GetAllAssetNames();
            dumpStrings(names);

            string scriptRegister = "";
            List<String> scripts = new List<string>();
            List<String> prefabs = new List<string>();

            foreach (string name in names)
            {
                if (name.Contains(".json"))
                {
                    scriptRegister = name;
                }
                if (name.Contains(".txt"))
                {
                    scripts.Add(name);
                }
                if (name.Contains(".prefab"))
                {
                    prefabs.Add(name);
                }
            }

            foreach (string name in prefabs)
            {
                GameObject parent = Instantiate(bundle.LoadAsset<GameObject>(name), parentForWorld);
                moveToStartPosition(parent);
                parent.transform.localRotation = new Quaternion(0,180,0,0);
            }

            if (scriptRegister != "")
            {
                Debug.Log("Scripts were attempted to be attached to this asset- these have been removed");
            }


        }
        worldDownloaded = true;
        worldReady.Invoke();
    }

    private void moveToStartPosition(GameObject parent)
    {
        parent.transform.position = Vector3.zero;
    }

    public IEnumerator downloadProgressMonitor(UnityWebRequest request)
    {
        downloadStarted.Invoke();
        while (downloading)
        {
            downloadProgress = request.downloadProgress;
            yield return null;
        }
        downloadComplete.Invoke();
    }

    private void LogDownloadFailed()
    {
        Debug.Log("Download Failed");
    }

    private void LogNoBundle()
    {
        Debug.Log("No Bundle Found in Register");
    }

    private void dumpStrings(string[] s)
    {
        foreach(string st in s)
        {
            Debug.Log(st);
        }
    }




}
