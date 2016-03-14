using UnityEngine;
using System.Collections;

public class Sounds : MonoBehaviour {

    public AudioSource arrowShootSoundGo;
    public AudioSource attackSoundGo;
    public AudioSource playerDeathGo;
    public AudioSource walkGrassSoundGo;
    public AudioSource playerHitSoundGo;
    public AudioSource levelUpSoundGo;
    public AudioSource blockSoundGo;
    public AudioSource buttonPressedSoundGo;

    public static AudioSource arrowShootSound;
    public static AudioSource attackSound;
    public static AudioSource playerDeathSound;
    public static AudioSource walkGrassSound;
    public static AudioSource playerHitSound;
    public static AudioSource levelUpSound;
    public static AudioSource blockSound;
    public static AudioSource buttonPressedSound;

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this.gameObject);
        arrowShootSound = arrowShootSoundGo;
        playerDeathSound = playerDeathGo;
        attackSound = attackSoundGo;
        walkGrassSound = walkGrassSoundGo;
        blockSound = blockSoundGo;
        levelUpSound = levelUpSoundGo;
        playerHitSound = playerHitSoundGo;
        buttonPressedSound = buttonPressedSoundGo;
        walkGrassSound.pitch = 1.2f;
    }
	
    public void PlayButtonSound()
    {
        buttonPressedSound.Play();
    }

}
