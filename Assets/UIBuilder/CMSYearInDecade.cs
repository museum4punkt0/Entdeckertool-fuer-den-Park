
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

namespace UIBuilder {

    public class CMSYearInDecade : VisualElement {

        Foldout wrapper = new Foldout();
        Label year = new Label();
        VisualElement icon = new VisualElement();
        Label subheadline = new Label();
        Label area = new Label();
        TextElement body = new TextElement();
        int newScreenWidth = 800;
        int imgWidth;
        int imgHeight;
        int newImgWidth;
        int newImgHeight;

        UnityEngine.UIElements.Button btn = new UnityEngine.UIElements.Button();
        VisualElement btnIcon = new VisualElement();

        public CMSYearInDecade(int year, Item item, UIItemViewController uIItemViewController) {

            string[] targets = item.attributes.date.Split("-");

            wrapper.text = targets[0];
            wrapper.Q<VisualElement>("unity-content").style.marginLeft = 0;

            icon.AddToClassList("cms-archaologie-accordeon-year");
            wrapper.Q<UnityEngine.UIElements.Toggle>().Q<VisualElement>().Add(icon);

            subheadline.style.paddingTop = 20;
            subheadline.text = "<b>" + item.attributes.location + "</b>";
            subheadline.style.fontSize = 58;
            area.text = "<b>" + item.attributes.size + "</b>";
            area.style.fontSize = 58;

            body.style.paddingTop = 80;
            body.style.paddingBottom = 80;
            body.style.fontSize = 48;
            body.text = item.attributes.description;

            btn.text = "<b>" + item.attributes.buttonHeadline + "</b>";
            btn.AddToClassList("cms-archaologie-grabungen-button");
            btnIcon.AddToClassList("cms-archaologie-grabungen-button-icon");
            btn.Add(btnIcon);

            btn.clicked += delegate {
                LoadImage(year, item, uIItemViewController);
            };

            wrapper.AddToClassList("cms-archaologie-grabungen-year");
            wrapper.Add(subheadline);
            wrapper.Add(area);
            wrapper.Add(body);

            if (item.attributes.grabungskarte.data.attributes.url != null) {
                wrapper.Add(btn);
            }

            Add(wrapper);

            wrapper.RegisterCallback<ChangeEvent<bool>>(evt => RotateIcon());

        }

        public void RotateIcon() {
            if (wrapper.value == false) {
                icon.style.rotate = new Rotate(180);

            } else if (wrapper.value == true) {
                icon.style.rotate = new Rotate(0);
            }
        }

        async void LoadImage(int year, Item item, UIItemViewController uIItemViewController) {
            uIItemViewController.yearPanel.SetActive(true);
            string imgUrl = item.attributes.grabungskarte.data.attributes.GetFullImageUrl();
            Davinci.get().load(imgUrl).setLoadingPlaceholder(Resources.Load<Texture2D>("Images/loadingwheel")).into(uIItemViewController.yearPanel.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("img")).start();
            uIItemViewController.yearPanel.GetComponent<UIDocument>().rootVisualElement.Q<Label>("description").text = "<b>" + year.ToString() + " / " + item.attributes.location + " / " + item.attributes.size + "</b>";

            uIItemViewController.yearPanel.GetComponent<UIDocument>().rootVisualElement.Q<UnityEngine.UIElements.Button>("close").clicked += delegate {
                uIItemViewController.yearPanel.SetActive(false);
            };
        }

    }
}


