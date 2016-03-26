using UnityEngine;
using System.Collections;

public class RangedTrap : MonoBehaviour {

    public float minDamage, maxDamage;
    public float triggerTime = 4f;
    public GameObject[] pLaunchers;
    private float timer;
    // Use this for initialization
    void Start () {
        for (int i = 0; i < pLaunchers.Length; i++)
        {
            pLaunchers[i].GetComponent<ProjectileLauncher>().SetDamage(minDamage,maxDamage);
        }
    }
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            for (int i = 0; i < pLaunchers.Length; i++)
            {
                pLaunchers[i].GetComponent<ProjectileLauncher>().LaunchProjectile(null);
            }
            timer = triggerTime;
        }
    }
}
