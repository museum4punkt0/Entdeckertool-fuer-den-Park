using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttributes<T> {



    // Start is called before the first frame update

    public static T CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<T>(jsonString);
    }


}
