using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class IllustrationPart : BaseAttributes<IllustrationPart>
{

    public int id;
    public float xOffset;
    public float yOffSet;
    public float zOffSet;
    public float Scale;
    public bool shouldAnimate;
    public string scan;

    public StrapiMedia image;

}
