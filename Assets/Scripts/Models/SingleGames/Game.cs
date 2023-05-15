using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Game : BaseAttributes<Game>
{
    public int id;
    public GameaAttributes attributes;
}




[System.Serializable]
public class GameaAttributes : BaseAttributes<GameaAttributes> {
    public int tourID;
    public TempPOI point_of_interest;
    public TempPOI glocke;
    public TempPOI maultier;
    public string description;
    public string gameTitle;
    public int reward;

    public RelPOI point_of_interests;

    public Game4PopUpStartContent popupStart;
    public Game4PopUpStartContent popupStep1;
    public Game4PopUpStartContent winMessageP1;
    public Game4PopUpStartContent popupStep2;
    public Game4PopUpStartContent winMessageP2;
    public Game4PopUpStartContent popupStep3;
    public Game4PopUpStartContent popupStep4;
    public Game4PopUpStartContent popupStep5;
    public Game4PopUpStartContent winMessageP3;
    public Game4PopUpStartContent failMessage;

    public SliderItem winimage1;
    public SliderItem winimage2;


}
