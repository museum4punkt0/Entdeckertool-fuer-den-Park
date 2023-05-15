using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

//namespace UIBuilder {

    public class MenuController : VisualElement {

        public VisualElement m_Root;
        public VisualElement m_scrollContent;
        VisualElement m_MainButton;
        VisualElement m_detectorButton;
        VisualElement m_Panel;
        VisualElement m_InfoButton;
        UIItemViewController uIItemViewController;
        bool isOpen = true;
        string menuType;
        public new class UxmlFactory : UxmlFactory<MenuController, UxmlTraits> {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits {
        }

        public MenuController() {
            this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);

            if (SceneManager.GetActiveScene().name == "ARFilterScene" ||
                SceneManager.GetActiveScene().name == "ARScene") {
                isOpen = false;
            }

            // menuType = uIItemViewController.menuType;

        }

        

        public void OnGeometryChange(GeometryChangedEvent evt) {
            m_MainButton = this.Q("mainButton");
            m_detectorButton = this.Q("detectorButton");



            string targetType;

           // m_MainButton?.RegisterCallback<ClickEvent>(ev => mainButtonOnTap());
          //  m_detectorButton?.RegisterCallback<ClickEvent>(ev => detectorButtonClick());
          //  m_MainButton.RegisterCallback<PointerDownEvent>(mainButtonOnClick, TrickleDown.TrickleDown);


        }

        public void detectorButtonClick() {
            if (SceneManager.GetActiveScene().name == "FindTool") {
                SceneManager.LoadScene("MainScene");

            } else {
                SceneManager.LoadScene("FindTool");

            }
        }


        public void mainButtonOnClick(PointerDownEvent evt) {
            m_Panel = this.Q("MenuController");

            if (isOpen) {
                this.m_Panel.style.translate = new Translate(0, Length.Percent(78), 0);
                isOpen = false;
                //MenuOnDownPosition();

            } else if (!isOpen) {
                this.m_Panel.style.translate = new Translate(0, Length.Percent(0), 0);
                isOpen = true;
                //uIItemViewController.SetMenuFromVisualElement("menumenu");
                //MenuOnUpPosition();
            }


        }

        private void mainButtonOnTap() {

        }

        public void MenuOnDownPosition() {

            this.Q("menu-filter").style.display = DisplayStyle.Flex;
            this.Q("menu-liste").style.display = DisplayStyle.Flex;
            this.Q("detectorButton").style.display = DisplayStyle.Flex;
            this.Q("menu-360").style.display = DisplayStyle.Flex;

            this.Q("empty").style.display = DisplayStyle.None;
            this.Q("menu-info").style.display = DisplayStyle.None;
            this.Q("menu-questions").style.display = DisplayStyle.None;
            this.Q("mainButton-AR").style.display = DisplayStyle.None;
            this.Q("closeAR").style.display = DisplayStyle.None;
            this.Q("menu-info-enabled").style.display = DisplayStyle.None;

        }

        public void MenuOnUpPosition(string menuType) {

            this.m_scrollContent = this.Q<VisualElement>("ScrollContent");

            if (menuType == "fragen") {
                this.Q<VisualElement>("Menu").style.display = DisplayStyle.Flex;
                this.Q<VisualElement>("Menu").style.height = Length.Percent(20);
                this.m_scrollContent.style.height = Length.Percent(80);

                this.Q<VisualElement>("menu-filter").style.display = DisplayStyle.None;
                this.Q<VisualElement>("menu-liste").style.display = DisplayStyle.None;
                this.Q<VisualElement>("detectorButton").style.display = DisplayStyle.None;
                this.Q<VisualElement>("menu-360").style.display = DisplayStyle.None;
                this.Q<VisualElement>("menu-info").style.display = DisplayStyle.None;
                this.Q<VisualElement>("empty").style.display = DisplayStyle.Flex;
                this.Q<VisualElement>("menu-questions").style.display = DisplayStyle.Flex;

            } else if (menuType == "mainMenu" || menuType == "game" || menuType == "touren" || menuType == "archaologie" || menuType == "hilfe" || menuType == "info" || menuType == "contextQuestions" || menuType == "belohnungen") {
                this.Q<VisualElement>("Menu").style.display = DisplayStyle.Flex;
                this.Q<VisualElement>("Menu").style.height = Length.Percent(20);
                this.m_scrollContent.style.height = Length.Percent(80);

                this.Q<VisualElement>("menu-filter").style.display = DisplayStyle.None;
                this.Q<VisualElement>("menu-liste").style.display = DisplayStyle.None;
                this.Q<VisualElement>("detectorButton").style.display = DisplayStyle.None;
                this.Q<VisualElement>("menu-360").style.display = DisplayStyle.None;
                this.Q<VisualElement>("menu-info").style.display = DisplayStyle.Flex;
                this.Q<VisualElement>("empty").style.display = DisplayStyle.None;
                this.Q<VisualElement>("menu-questions").style.display = DisplayStyle.Flex;

                if (menuType == "hilfe") {
                    this.Q<VisualElement>("menu-info").style.display = DisplayStyle.None;
                    this.Q<VisualElement>("menu-info-enabled").style.display = DisplayStyle.Flex;
                }
            } else if (menuType == "detail") {
                this.Q<VisualElement>("Menu").style.display = DisplayStyle.None;
                this.m_scrollContent.style.height = Length.Percent(100);
            }
        }
    
    
    
    
    }
//}