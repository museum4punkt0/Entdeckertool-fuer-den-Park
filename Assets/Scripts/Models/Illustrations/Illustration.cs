using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Illustration : BaseAttributes<Illustration>
{
  
    public float animationSpeed;
    public string color;
    public bool contains3D;
    public bool isFramed;
    public List<IllustrationPart> IllustrationParts;


}


