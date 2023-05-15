using System.Collections;
using System.Collections.Generic;
using ARLocation;
using UnityEngine;

public class LocationDebugger : MonoBehaviour
{
    private ARLocationProvider _arLocationProvider;

    
    // Start is called before the first frame update
    void Start()
    {
        this._arLocationProvider = ARLocationProvider.Instance;

    }

    // Update is called once per frame
    void Update()
    {
    
    }

        
}
