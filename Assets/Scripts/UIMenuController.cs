using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIBuilder;
using UnityEngine.UIElements;

public class UIMenuController : MonoBehaviour 
{
    
    Button mainMenu;
    VisualElement panel;
    bool isOpen = true;

    // Start is called before the first frame update
    void Start()
    {
        mainMenu = GetComponent<UIDocument>().rootVisualElement.Q<Button>("mainButton");
        panel = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("MenuController");

    }

    // Update is called once per frame
    void Update()
    {
        

        print(Input.touchCount);
        //mainMenu?.RegisterCallback<ClickEvent>(ev => mainButtonOnClick());
          mainMenu.RegisterCallback<PointerDownEvent>(mainButtonOnClick, TrickleDown.TrickleDown);
    }

    private void mainButtonOnClick(PointerDownEvent evt) {

        
        //this.m_Panel.AddToClassList("opacityOut");
        float height = Screen.height * 0.65f;



        if (isOpen) {
            //initial pos
            this.panel.transform.position = new Vector2(0, 0);
            //end pos
            this.panel.experimental.animation.Position(new Vector2(0, height), 550);
           // isOpen = false;
        } else if (!isOpen) {
            //initial pos
            this.panel.transform.position = new Vector2(0, height);
            //end pos
            this.panel.experimental.animation.Position(new Vector2(0, 0), 550);
           // isOpen = true;
        }


    }
}
