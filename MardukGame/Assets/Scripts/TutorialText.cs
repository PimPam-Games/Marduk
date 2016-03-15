using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialText : MonoBehaviour {

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
        if(!TutorialController.grabTutorialOn)
        {
            grabTutorial.SetActive(false);
        }
        if (!TutorialController.moveTutorialOn)
        {
            moveTutorial.SetActive(false);
        }
        if (!TutorialController.attackTutorialOn)
        {
            normalAttackTutorial.SetActive(false);
        }
    }

    public static void EnableTutorial(bool enable)
    {
        moveTut.SetActive(enable);
        grabTut.SetActive(enable);
        attacktut.SetActive(enable);
    }

}
