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
    public AudioSource ringSoundGo;
    public AudioSource weaponSoundGo;
    public AudioSource helmetSoundGo;
    public AudioSource shieldSoundGo;
    public AudioSource amuletSoundGo;
    public AudioSource beltSoundGo;
    public AudioSource armourSoundGo;

    public static AudioSource arrowShootSound;
    public static AudioSource attackSound;
    public static AudioSource playerDeathSound;
    public static AudioSource walkGrassSound;
    public static AudioSource playerHitSound;
    public static AudioSource levelUpSound;
    public static AudioSource blockSound;
    public static AudioSource buttonPressedSound;
    public static AudioSource ringSound;
    public static AudioSource weaponSound;
    public static AudioSource helmetSound;
    public static AudioSource shieldSound;
    public static AudioSource amuletSound;
    public static AudioSource beltSound;
    public static AudioSource armourSound;

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
        ringSound = ringSoundGo;
        weaponSound = weaponSoundGo;
        helmetSound = helmetSoundGo;
        shieldSound = shieldSoundGo;
        amuletSound = amuletSoundGo;
        beltSound = beltSoundGo;
        armourSound = armourSoundGo;
        walkGrassSound.pitch = 1.2f;
    }
	
    public void PlayButtonSound()
    {
        buttonPressedSound.Play();
    }

}
