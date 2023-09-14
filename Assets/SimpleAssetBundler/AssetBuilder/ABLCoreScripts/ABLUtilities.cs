using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ABLUtilities : MonoBehaviour
{

    public static string GetSaltedName(GameObject go)
    {
        return go.name + "$" + go.GetInstanceID();
    }

    public static string GetUnsaltedName(GameObject go)
    {
        return go.name.Substring(0, go.name.IndexOf("$"));
    }

    [Serializable]
    public class ObjectScriptPairList
    {
        public List<ObjectScriptPair> pairs;
        public ObjectScriptPairList()
        {
            pairs = new List<ObjectScriptPair>();
        }

        public void AddPair(ObjectScriptPair pair)
        {
            pairs.Add(pair);
        }
    }

    [Serializable]
    public class ObjectScriptPair
    {
        public string gameObjectName;
        public string script;

        public ObjectScriptPair(string gameObjectName, string script)
        {
            this.gameObjectName = gameObjectName;
            this.script = script;
        }
    }

    [System.Serializable]
    public class TicketURL
    {
        public string ticketID;
        public string bundleURL;
        public string serverURL;

        public TicketURL(string ticketID, string bundleURL, string serverURL)
        {
            this.ticketID = ticketID;
            this.bundleURL = bundleURL;
            this.serverURL = serverURL;
        }
    }

    [System.Serializable]
    public class TicketBundle
    {
        public List<TicketURL> bundles;

        public TicketBundle()
        {
            bundles = new List<TicketURL>();
        }

        public void AddBundle(TicketURL t)
        {
            bundles.Add(t);
        }
    }
}


