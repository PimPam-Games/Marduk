using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour {

    public Text text;
    public static bool showTutorial = false;
    public static bool grabTutorialOn = false;
    public static bool moveTutorialOn = false;
    public static bool attackTutorialOn = false;
    public static bool attributesTutorialOn = false;
    public static bool traitsTutorialOn = false;
    public static bool inventoryTutorialOn = false;
    //public static bool skillsTutorialOn = true;

    public static bool invTutorialShowed = false;
    public static bool traitsTutorialShowed = false;
    public static bool attributesTutorialShowed = false;
    public static bool attackTutorialShowed = false;
    public static bool grabTutorialShowed = false;
    public static bool moveTutorialShowed = false;

    private float checkTimer = 0;

    void Update()
    {
        checkTimer -= Time.deltaTime;
        if (checkTimer >= 0)
            return;
        checkTimer = 0.6f;
        text.text = "";
        if (inventoryTutorialOn)
        {
            text.text += "- New item obtained! Press I or V to open the inventory \n\n";
        }
        if (attributesTutorialOn)
        {
            text.text += "- Attributes points earned! Press C to open character window \n\n";
        }
        if (traitsTutorialOn)
        {
            text.text += "- Trait point earned! Press T to open Traits panel \n\n";
        }
       
    }

    public static void EnableTutorial(bool enable)
    {
        bool inGameTut = false;
        if(!moveTutorialShowed)
            moveTutorialOn = enable;
        if (!grabTutorialShowed)
            grabTutorialOn = enable;
        if(!attackTutorialShowed)
            attackTutorialOn = enable;
        inGameTut = attackTutorialOn || moveTutorialOn || grabTutorialOn;
        TutorialText.EnableTutorial(inGameTut);
    }

}
