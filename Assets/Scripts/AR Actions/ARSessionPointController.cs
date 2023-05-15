    using System.Collections;
    using System.Collections.Generic;
using ARLocation;
using Services;
    using UnityEngine;

    public class ARSessionPointController : MonoBehaviour
    {

        [SerializeField]
        public GameObject _pointsRoot;


        [SerializeField]
        public List<GameObject> prefabs;

        private List<Poi> pointOfInterests;
        public CrossGameManager crossGameManager;


    private void Awake() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
    }

    // Start is called before the first frame update
    void Start()
        {
            StartCoroutine(crossGameManager.strapiService.getPointOfInterests((StrapiResponse<Poi> res) =>
            {
                this.pointOfInterests = res.data;
                this.spawnPointsinAR();
            }
        ));
      
        }
        void spawnPointsinAR() {
            this.pointOfInterests.ForEach((Poi poi) => {
                var opts = new PlaceAtLocation.PlaceAtOptions() {
                    HideObjectUntilItIsPlaced = true,
                    MaxNumberOfLocationUpdates = 2,
                    MovementSmoothing = 0.1f,
                    UseMovingAverage = false
                };

                PlaceAtLocation.AddPlaceAtComponent(this.getPrefabInstaceByType(poi.attributes.type), poi.attributes.getARLocation(), opts);


            });
        }

    private GameObject getPrefabInstaceByType(string type) {
        GameObject prfb;

        switch (type) {
            case "pin1":
                prfb = this.prefabs[0];
                break;
            case "pin2":
                prfb = this.prefabs[1];
                break;
            case "pin3":
                prfb = this.prefabs[2];
                break;
            case "alleobjekte":
                prfb = this.prefabs[3];
                break;
            case "obj1":
                prfb = this.prefabs[4];
                break;
            case "obj2":
                prfb = this.prefabs[5];
                break;
            case "obj3":
                prfb = this.prefabs[6];
                break;
            case "obj4":
                prfb = this.prefabs[7];
                break;
            case "obj5":
                prfb = this.prefabs[8];
                break;
            case "obj6":
                prfb = this.prefabs[9];
                break;
            case "obj7":
                prfb = this.prefabs[10];
                break;
            case "obj8":
                prfb = this.prefabs[11];
                break;
            case "obj9":
                prfb = this.prefabs[12];
                break;
            case "obj10":
                prfb = this.prefabs[13];
                break;
            case "obj11":
                prfb = this.prefabs[14];
                break;
            default:
                prfb = this.prefabs[0];
                break;
        }

        
        GameObject instanciatedPrfb = Instantiate(prfb);
        instanciatedPrfb.transform.parent = this._pointsRoot.transform;
        return instanciatedPrfb;
    }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
