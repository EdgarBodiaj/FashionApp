using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using static ABLUtilities;
using System.Linq;

public class PrepareForExport : EditorWindow
{
    public string bundleName = "BundleName";
    //public string gameserverURL = "oxjambeeston.org/chat";
    //public string bundleserverURL = "http://paultennent.co.uk/bundles";
    public GameObject rootAsset;

    //private string bundleRegisterText = "Please prepare an asset first...";
    private Vector2 scroll;

    private List<string> assetPaths;
    ObjectScriptPairList list;
    private Texture2D icon;
    private GUIStyle guiStyle = new GUIStyle();

    public enum CompileType
    {
        iOS,
        Android,
        Windows,
        Mac
    };

    public CompileType compileType;

    [MenuItem("Window/Prepare Export")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<PrepareForExport>("Prepare Assets for Export...");
    }

    void OnEnable()
    {
        icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/BubblesAssetBuilder/Icons/bubbles.png");
        guiStyle.fontSize = 16; //change the font size
        guiStyle.normal.textColor = Color.white;
        guiStyle.alignment = TextAnchor.MiddleLeft;
    }


    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 50, 50), icon);
        GUI.Label(new Rect(70, 10, 200, 50), "Simple AssetBundle Build System", guiStyle);
        GUILayout.Space(70);
        //window code
        EditorGUILayout.HelpBox("Your bundle should have a single root object to be loadable. Select it in the scene here.", MessageType.Info, true);

        rootAsset = (GameObject) EditorGUILayout.ObjectField("Root Asset", rootAsset, typeof(GameObject), true);
        string rootAssetPath = AssetDatabase.GetAssetPath(rootAsset);

        GUILayout.Space(20);

        EditorGUILayout.HelpBox("Please select tarhet OS for compiling. You should ensure you have the relevant build targets installed.", MessageType.Info, true);
        compileType = (CompileType)EditorGUILayout.EnumPopup("Built Target", compileType);
        GUILayout.Space(20);

        EditorGUILayout.HelpBox("Your bundle name should not contain spaces. Tickets must include this bundle name to access the world, so you may wish to use an ID of some kind.", MessageType.Info, true);
        bundleName = EditorGUILayout.TextField("Bundle Name", bundleName);

        assetPaths = new List<string>();

        GUILayout.Space(20);
        EditorGUILayout.HelpBox("Please remember that custom scripts do not work with assetbundles.", MessageType.Info, true);
        if (GUILayout.Button("Prepare Assets"))
        {
            Debug.Log("Button Pressed");

            if (compileType == CompileType.Mac)
            {
                if (bundleName.Substring(bundleName.Length - 3) != "osx")
                {
                    bundleName += "osx";
                }
            }
            else if (compileType == CompileType.Android)
            {
                if (bundleName.Substring(bundleName.Length - 3) != "aOS")
                {
                    bundleName += "aOS";
                }
            }
            else if (compileType == CompileType.iOS)
            {
                if (bundleName.Substring(bundleName.Length - 3) != "iOS")
                {
                    bundleName += "iOS";
                }
            }
            else
            {
                if(bundleName.Substring(bundleName.Length-3) == "osx" || bundleName.Substring(bundleName.Length - 3) == "iOS" || bundleName.Substring(bundleName.Length - 3) == "aOS")
                {
                    bundleName = bundleName.Substring(0, bundleName.Length - 3);
                }
            }

            //build the unadulterated prefab that we will reinstasiate
            if (!Directory.Exists("Assets/AssetBundles"))
            {
                AssetDatabase.CreateFolder("Assets", "AssetBundles");
            }

            string rootAssetName = rootAsset.name;

            if (!Directory.Exists("Assets/Temp"))
            {
                AssetDatabase.CreateFolder("Assets", "Temp");
            }

            string localCleanPath = "Assets/Temp/" + bundleName + "-" + rootAsset.name + "-unedited" + ".prefab";
            localCleanPath = AssetDatabase.GenerateUniqueAssetPath(localCleanPath);

            bool cleanprefabSuccess;
            PrefabUtility.SaveAsPrefabAsset(rootAsset, localCleanPath, out cleanprefabSuccess);
            if (cleanprefabSuccess == true)
            {
                Debug.Log("Prefab was saved successfully");
            }
            else
            {
                Debug.Log("Prefab failed to save" + cleanprefabSuccess);
            }


            //now it's time to prep our existing asset for export - delete the scrips from it that aren't in our safe list
            list = new ObjectScriptPairList();
            recursiveScriptRemove(rootAsset.transform);


            //now build the prepped one

            if (!Directory.Exists("Assets/Temp"))
            {
                AssetDatabase.CreateFolder("Assets", "Temp");
            }

            string localPath = "Assets/Temp/" + bundleName + "-" + rootAsset.name + ".prefab";
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

            bool prefabSuccess;
            PrefabUtility.SaveAsPrefabAsset(rootAsset, localPath, out prefabSuccess);
            if (prefabSuccess == true)
            {
                Debug.Log("Prefab was saved successfully");
            }
            else
            {
                Debug.Log("Prefab failed to save" + prefabSuccess);
            }

            //now we're going to remove the live one, replace it with the unedited prefab which we then delete
            DestroyImmediate(rootAsset);

            AssetDatabase.ImportAsset(localCleanPath, ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ImportRecursive);
            var originalPrefab = AssetDatabase.LoadMainAssetAtPath(localCleanPath);
            var go = Instantiate(originalPrefab) as GameObject;
            go.name = rootAssetName;

            rootAsset = go;

            //flip the order of the list so that the unsalting works properly
            list.pairs.Reverse();

            //now add the script register
            string s = JsonUtility.ToJson(list);
            File.WriteAllText("Assets/Temp/ScriptRegister.json", s);

            assetPaths.Add(localPath);
            assetPaths.Add("Assets/Temp/ScriptRegister.json");

            foreach(string path in assetPaths)
            {
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ImportRecursive);
                AssetImporter.GetAtPath(path).SetAssetBundleNameAndVariant(bundleName, "");
            }

            //if (Application.platform == RuntimePlatform.WindowsEditor)
            if(compileType == CompileType.Windows)
            {
                BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
            }
            else if (compileType == CompileType.iOS)
            {
                BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.iOS);
            }
            else if (compileType == CompileType.Android)
            {
                BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.Android);
            }
            else
            {
                BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneOSX);
            }

            //now tidy up
            AssetDatabase.DeleteAsset(localCleanPath);
            foreach (string path in assetPaths)
            {
                AssetDatabase.DeleteAsset(path);
            }
            AssetDatabase.DeleteAsset("Assets/Temp");
            EditorUtility.RevealInFinder(Application.dataPath + "/AssetBundles/"+bundleName);
        }

        GUILayout.Space(20);
        EditorGUILayout.HelpBox("Once you have prepared the asset, You should upload it to the bundle server.", MessageType.Info, true);
        //scroll = EditorGUILayout.BeginScrollView(scroll);
        //EditorGUILayout.EndScrollView();
    }


    private void recursiveScriptRemove(Transform t)
    {
        Component[] components = t.gameObject.GetComponents<MonoBehaviour>();
        foreach(Component c in components)
        {
            MonoScript ms = MonoScript.FromMonoBehaviour((MonoBehaviour) c);
            string m_ScriptFilePath = AssetDatabase.GetAssetPath(ms);
            Debug.Log(m_ScriptFilePath);

            //only deal with it if it's not one of our bubbles scripts
            if (!m_ScriptFilePath.Contains("Assets/BubblesAssetBuilder"))
            {

                string f_filepath = m_ScriptFilePath.Substring(0, m_ScriptFilePath.Length - 3) + ".txt";
                Debug.Log(f_filepath);

                string s = File.ReadAllText(m_ScriptFilePath);
                //add the code to make this object get added to where it needs to go by the dynamic compiler - and also unsalt the name
                string toInsert = "\npublic static " + ms.name + " AddYourselfTo(GameObject host){\nhost.name=ABLUtilities.GetUnsaltedName(host);\nreturn host.AddComponent<" + ms.name + ">();\n}\n";
                s = s.Insert(s.LastIndexOf("}"), toInsert);

                File.WriteAllText(f_filepath, s);
                assetPaths.Add(f_filepath);

                //salt the name to ensure they're unique
                t.name = ABLUtilities.GetSaltedName(t.gameObject);
                list.AddPair(new ObjectScriptPair(t.name, c.GetType().Name));

                DestroyImmediate(c);
            }
        }

        foreach(Transform child in t)
        {
            recursiveScriptRemove(child);
        }
    }

    private void dumpStrings(string[] s)
    {
        foreach (string st in s)
        {
            Debug.Log(st);
        }
    }

}


