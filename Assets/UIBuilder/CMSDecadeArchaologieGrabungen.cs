using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace UIBuilder {

public class CMSDecadeArchaologieGrabungen : VisualElement {

        Foldout wrapper = new Foldout();
        VisualElement icon = new VisualElement();
        Label headline = new Label();

 

        public CMSDecadeArchaologieGrabungen(string decade, StrapiItemResponse res, UIItemViewController uIItemViewController) {

            wrapper.name = "decade";
            wrapper.text = decade;
            icon.AddToClassList("cms-archaologie-accordeon-decade");

            wrapper.Q<Toggle>().Q<VisualElement>().Add(icon);
            wrapper.AddToClassList("cms-archaologie-grabungen-decade");

            res.data.Sort((x, y) => x.attributes.date.CompareTo(y.attributes.date));

            foreach (Item grabung in res.data) {
                string[] targets = grabung.attributes.date.Split("-");
                int year;

                if (int.TryParse(targets[0], out year)) {
                    if ((year >= 1989 && year <= 1999) && (decade == "1989-1999")) {
                        
                        wrapper.Add(new CMSYearInDecade(year, grabung, uIItemViewController));
                    } else if ((year >= 2000 && year <= 2009) && (decade == "2000-2009")) {
                        wrapper.Add(new CMSYearInDecade(year, grabung, uIItemViewController));
                    } else if ((year >= 2010 && year <= 2019) && (decade == "2010-2019")) {
                        wrapper.Add(new CMSYearInDecade(year, grabung, uIItemViewController));
                    } else if ((year >= 2020 && year <= 2029) && (decade == "2020-2029")) {
                        wrapper.Add(new CMSYearInDecade(year, grabung, uIItemViewController));
                    }
                }
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

    }

}