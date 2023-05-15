using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Game4DragDrop : MonoBehaviour, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    public Material frameMaterial;
    public Material canvasMaterial;
    private bool testingOnLapTop;
    private Vector2 position;
    public GameObject GameController;

    private void Awake() {
        this.rectTransform = GetComponent<RectTransform>();
        Debug.Log(this.position);
        this.position = this.rectTransform.transform.position;
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
            this.testingOnLapTop = false;
        } 
    }


    public void OnDrag(PointerEventData eventData) {
        this.rectTransform.anchoredPosition += eventData.delta /this.canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData) {
          HandleTouch(eventData.position);
    }

    void HandleTouch(Vector3 pos) {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100)) {

            if (hit.transform.gameObject.CompareTag("tent_frame") && this.transform.CompareTag("tent_frame")) {
                GameObject frame = hit.transform.gameObject;
                ChangeMaterialsOnMeshWithTagFrame("tent_frame");
                this.Destroy();
                Destroy(this.gameObject);

            } else if (hit.transform.gameObject.CompareTag("tent_canvas") && this.transform.CompareTag("tent_canvas")) {
                GameObject canvas = hit.transform.gameObject;
                ChangeMaterialsOnMeshWithTagCanvas("tent_canvas");

                this.Destroy();
                Destroy(this.gameObject);
            } else {
                this.rectTransform.position = this.position;
            }

        } else {
            this.rectTransform.position = this.position;

        }

    }



    // Update is called once per frame
    void ChangeMaterialsOnMeshWithTagFrame(string tag)
    {
        GameObject[] meshes = GameObject.FindGameObjectsWithTag(tag);
        foreach (var mesh in meshes) {
            if (mesh.GetComponent<Renderer>()) {
                mesh.GetComponent<Renderer>().material = this.frameMaterial;

            }
        }
      




    }
    void ChangeMaterialsOnMeshWithTagCanvas(string tag) {
        GameObject[] meshes = GameObject.FindGameObjectsWithTag(tag);
        foreach (var mesh in meshes) {
            if (mesh.GetComponent<Renderer>()) {
                mesh.GetComponent<Renderer>().material = this.canvasMaterial;

            }
        }

  
 
    }

    // Start is called before the first frame update
 


}
