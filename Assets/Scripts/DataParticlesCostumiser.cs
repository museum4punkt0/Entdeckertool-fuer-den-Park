using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[RequireComponent(typeof(ParticleSystem))]
public class DataParticlesCostumiser : MonoBehaviour
{

    public int amount = 3;
    public Mesh mesh;
    public Material mat;

    [Header("mensch knocken")]
    public bool knocken;
    public List<Material> KnockenMat;
    public Mesh KnockenMesh;
   

    [Header("helm")]
    public bool helm;
    public List<Material> HelmMat;
    public Mesh HelmMesh;

    [Header("munzen")]
    public bool munzen;
    public List<Material> munzenMat;
    public Mesh munzenMesh;

    [Header("sandalen")]
    public bool sandalen;
    public List<Material> sandalenMat;
    public Mesh sandalenMesh;

    [Header("tier knocken")]
    public bool tier;
    public List<Material> tierMat;
    public Mesh tierMesh;

    [Header("all fund")]
    public bool fund;
    public List<Material> fundMat;
    public Mesh fundMesh;

    public ParticleSystem particleSystem;
    public Particle[] particles;
    public ParticleSystemRenderer particleSystem_renderer;

    public GameObject prefab;

    public Transform destination;

    public bool isAscending;
    public float duration;
    public Vector3 visiblePos;
    public Vector3 invisblePos;

    private float startTime;

    public float heightincrement = 0.00001f;



    void Start()
    {
        particleSystem = this.GetComponent<ParticleSystem>();
        particleSystem_renderer = this.GetComponent<ParticleSystemRenderer>();

        visiblePos = this.transform.position;

        var shape = particleSystem.shape;
     
        shape.enabled = true;

        visiblePos = new Vector3(this.transform.position.x, 0f, this.transform.position.z);

        //0 - shape.length
        invisblePos = new Vector3(this.transform.position.x, -100f, this.transform.position.z);

        this.transform.position = invisblePos;

        startTime = Time.time;


        if (knocken) {
            CostumiseParticleSystem(amount, KnockenMat, KnockenMesh);
        } else if (helm) {
            CostumiseParticleSystem(amount, HelmMat, HelmMesh);
            
        } else if (munzen) {
            CostumiseParticleSystem(amount,munzenMat, munzenMesh);

        } else if (tier) {
            CostumiseParticleSystem(amount, tierMat, tierMesh);

        } else if (sandalen) {
            CostumiseParticleSystem(amount, sandalenMat, sandalenMesh);

        } else if (tier) {
            CostumiseParticleSystem(amount, tierMat, tierMesh);

        } else if (fund) {
            CostumiseParticleSystem(amount, fundMat, fundMesh);

            visiblePos = new Vector3(this.transform.position.x, 200f, this.transform.position.z);

            //0 - shape.length
            invisblePos = new Vector3(this.transform.position.x, -200f, this.transform.position.z);

            this.transform.position = invisblePos;

        }
    }


    public void CostumiseParticleSystem(int amount, List<Material> _mats, Mesh _mesh) {


        if (_mats.Count <= 1) {

            mesh = _mesh;
            mat = _mats[0];

            //get main panel in particle effect; 
            var main = particleSystem.main;
            main.maxParticles = amount;

            //emmision controller in particle effect
            var emmision = particleSystem.emission;
            emmision.rateOverTime = amount;

            particleSystem_renderer.mesh = mesh;
            particleSystem_renderer.material = mat;


        } else {

            var _main = particleSystem.main;
            _main.maxParticles = 0;


            foreach (Material _mat in _mats) {

         
                GameObject particleSystemChild = Instantiate(prefab, this.transform);
                particleSystemChild.transform.localPosition = new Vector3(0, 0, 0);
               ParticleSystem particlePart = particleSystemChild.GetComponent<ParticleSystem>();

                ParticleSystemRenderer particlePart_renderer = particleSystemChild.GetComponent<ParticleSystemRenderer>();

                mesh = _mesh;
               
                //get main panel in particle effect; 
                var main = particlePart.main;
                main.maxParticles = amount/_mats.Count;

                //emmision controller in particle effect
                var emmision = particlePart.emission;
                emmision.rateOverTime = amount / _mats.Count;

                var shape = particlePart.shape;
                //float startRadius = shape.radius;
                //float newRadius = startRadius * _mats.Count;
                //shape.radius = newRadius;
                shape.enabled = true;
                shape.shapeType = ParticleSystemShapeType.Box;

                if (amount > 500) {
                    shape.scale = new Vector3(400, 1000, 1000);
                }
                if (amount > 900) {
                    shape.scale = new Vector3(400, 1600, 1000);
                }



                particlePart_renderer.mesh = mesh;
                particlePart_renderer.material = _mat;
            }
        
        }


    }

    private void Update() {
        //particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];

        //int numParticles = particleSystem.GetParticles(particles);

        //for (int i = 0; i < numParticles; i++) {
        //    Particle particle = particles[i];

        //    if (isAscending) {

        //        Vector3 newPos = new Vector3(particle.position.x, particle.position.y + 0.001f, particle.position.z);
        //        particle.position = Vector3.Lerp(particle.position, newPos, duration);
        //    } else {
        //        Vector3 newPos = new Vector3(particle.position.x, -1, particle.position.z);
        //        particle.position = Vector3.Lerp(particle.position, newPos, duration);
        //    }


        //    Debug.Log(particle.position);

        //    particles[i] = particle;
        //}

        //particleSystem.SetParticles(particles, numParticles);

        if (isAscending && this.transform.position.y < visiblePos.y) {

            float newY = transform.position.y + heightincrement;

            transform.position = new Vector3(transform.position.x, newY, transform.position.z);


        }


    }
}
