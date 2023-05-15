using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SliderMenu : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public GameObject PanelMenu;
    private Vector3 panelLocation;
    public float percentThreshold = 0.2f;
    public float easing = 0.5f;
    public int totalPages = 1;
    private int currentPage = 1;
    
    public void ShowHideMenu()
    {
        if (PanelMenu != null)
        {
            Animator animator = PanelMenu.GetComponent<Animator>();
            if (animator != null)
            {
                bool isOpen = animator.GetBool("showMenu");
                animator.SetBool("showMenu", !isOpen);
            }
        }
        
    }

    public void OnDrag(PointerEventData eventData) {
        float percentage = (eventData.pressPosition.y - eventData.position.y) / Screen.height; 
        //Debug.Log(percentage);
    }

    public void OnEndDrag(PointerEventData eventData) {
        
    }
    
    

}
