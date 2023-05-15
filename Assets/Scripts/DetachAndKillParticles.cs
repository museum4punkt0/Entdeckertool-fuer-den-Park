using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachAndKillParticles : MonoBehaviour
{

    public void DetachAndKillParticle(GameObject parent) {

        for (int i = 0; i < parent.transform.childCount; i++) {

            if (parent.transform.GetChild(i).GetComponent<ParticleSystem>()) {
                parent.transform.GetChild(i).parent = null;
                ParticleSystem ps = parent.transform.GetChild(i).GetComponent<ParticleSystem>();
                ps.Stop();

                var main = ps.main;

                main.stopAction = ParticleSystemStopAction.Destroy;

                StartCoroutine(DestroyOnDelay(parent.transform.GetChild(i).gameObject));
            }

        }

    }

    /// <summary>
    /// //need to move this to a script that does not get killed
    /// </summary>
    /// <param name="particle"></param>
    /// <returns></returns>
    IEnumerator DestroyOnDelay(GameObject particle) {
        yield return new WaitForSeconds(5);
        Destroy(particle);
    
    }

}
