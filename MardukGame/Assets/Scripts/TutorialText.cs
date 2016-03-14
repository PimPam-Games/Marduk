using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialText : MonoBehaviour {

    public static bool showTutorial = false;
    public static bool grabTutorialOn = true;
    public static bool moveTutorialOn = true;
    public static bool attackTutorialOn = true;

    private bool tutorialEnable = true;
    public GameObject grabTutorial;
    public GameObject moveTutorial;
    public GameObject normalAttackTutorial;

    private static GameObject grabTut;
    private static GameObject moveTut;
    private static GameObject attacktut;

    void Start()
    {
        grabTut = grabTutorial;
        moveTut = moveTutorial;
        attacktut = normalAttackTutorial;
    }

    void Update() {
        if(!grabTutorialOn)
        {
            grabTutorial.SetActive(false);
        }
        if (!moveTutorialOn)
        {
            moveTutorial.SetActive(false);
        }
        if (!attackTutorialOn)
        {
            normalAttackTutorial.SetActive(false);
        }
    }

    public static void EnableTutorial(bool enable)
    {
        grabTutorialOn = enable;
         moveTutorialOn = enable;
        attackTutorialOn = enable;
        moveTut.SetActive(enable);
        grabTut.SetActive(enable);
        attacktut.SetActive(enable);
    }

}
