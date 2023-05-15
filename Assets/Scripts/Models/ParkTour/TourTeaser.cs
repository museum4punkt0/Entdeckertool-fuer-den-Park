using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIBuilder;


[System.Serializable]
public class TourTeaser : BaseAttributes<TourTeaser> {
    public int id;
    public string headline;
    public string subheadline;
    public string body;
    public List<CMSButton> button;
    public List<CMSButton> icons;
    public Icon icon;
    public int duration;
    public int reward;
}


