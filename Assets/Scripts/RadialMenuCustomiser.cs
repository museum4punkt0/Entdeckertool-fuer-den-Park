using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RadialWheel))]
public class RadialMenuCustomiser : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject tourLoaderObject;
    public TourLoader tourLoader;
    public RadialWheel radialWheel;

    private bool isWaitingForTourLoaderItems;
    void Start()
    {
      
    }



    public void CostumiseRadialWheel() {

        tourLoader = tourLoaderObject.GetComponent<TourLoader>();
        radialWheel = transform.GetComponent<RadialWheel>();

        isWaitingForTourLoaderItems = true;



    }


    private void Update() {
        if (isWaitingForTourLoaderItems && tourLoader.ItemsOnMap.Count > 1) {

            Debug.Log("costumises radial menu items");
            isWaitingForTourLoaderItems = false;
            int index = 0;

            foreach (RadialMenuItem menuItem in this.radialWheel.menuItems) {

                ItemOnMap itemOnMap = new ItemOnMap();

                if (index == 0) {
                    itemOnMap = tourLoader.ItemsOnMap.Find(item => item.Name.Contains("Dolch"));
                } else if (index == 1) {
                  itemOnMap = tourLoader.ItemsOnMap.Find(item => item.Name.Contains("fibel"));
                } else if (index == 2) {
                    itemOnMap = tourLoader.ItemsOnMap.Find(item => item.Name.Contains("Hängeschurzriemen"));
                } else if (index == 3) {
                    itemOnMap = tourLoader.ItemsOnMap.Find(item => item.Name.Contains("Unterkiefer"));
                } else if (index == 4) {
                    itemOnMap = tourLoader.ItemsOnMap.Find(item => item.Name.Contains("Pilum"));
                } else if (index == 5) {
                    itemOnMap = tourLoader.ItemsOnMap.Find(item => item.Name.Contains("Schienen"));
                } else if (index == 6) {
                    itemOnMap = tourLoader.ItemsOnMap.Find(item => item.Name.Contains("Sandal"));
                } else if (index == 7) {
                    itemOnMap = tourLoader.ItemsOnMap.Find(item => item.Name.Contains("Silber"));
                } else if (index == 8) {
                    itemOnMap = tourLoader.ItemsOnMap.Find(item => item.Name.Contains("Schwert"));
                }

                Debug.Log(itemOnMap.Name);
                menuItem.Image = itemOnMap.AssociatedMenuItems[0];
                menuItem.Name = itemOnMap.Name;

                index++;

         
                if (index > tourLoader.ItemsOnMap.Count) {
                    index = 0;
                }

                menuItem.Item.GetChild(0).GetComponent<Image>().sprite = menuItem.Image;





            }


      

        }
    }
}
