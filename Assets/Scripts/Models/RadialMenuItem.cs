using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]

public class RadialMenuItem {
    public float angle;
    public RectTransform item;
    public string name;
    public Sprite image;
    public bool isUsed;

    public bool IsUsed {
        get {
            //Some other code
            return this.isUsed;
        }
        set {
            //Some other code
            this.isUsed = value;
        }
    }
    public float Angle {
        get {
            //Some other code
            return this.angle;
        }
        set {
            //Some other code
            this.angle = value;
        }
    }
    public RectTransform Item {
        get {
            return this.item;
        }
        set {
            this.item = value;
        }
    }
    public string Name {
        get {
            return this.name;
        }
        set {
            this.name = value;
        }
    }
    public Sprite Image {
        get {
            return this.image;
        }
        set {
            this.image = value;
        }

    }
}