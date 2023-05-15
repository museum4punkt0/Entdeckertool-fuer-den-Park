
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GrabungsData : BaseAttributes<GrabungsData> {
    public int id;
    public GrabungsAttributes attributes;
}

[System.Serializable]
public class GrabungsAttributes : BaseAttributes<GrabungsAttributes> {
    public string location;
    public string buttonTarget;
    public string buttonHeadline;
    public string size;
    public string description;
    public string category;
}




