using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectsPool : MonoBehaviour {

	public GameObject bloodGo;
    public GameObject combatText;

	private static GameObject bloodGoStatic;
    private static GameObject combatTextStatic;
    public static Queue<GameObject> bloods;
    public static Queue<GameObject> combatTexts;

    // Use this for initialization
    void Start () {
		bloods = new Queue<GameObject>();
        combatTexts = new Queue<GameObject>();
        bloodGoStatic = bloodGo;
        combatTextStatic = combatText;
		for(int i = 0; i < 3; i++){ //crea 3 sangres al inicio del juego
			GameObject b = (GameObject)Instantiate(bloodGo, this.transform.position, this.transform.rotation);          
            DontDestroyOnLoad(b);           
            b.SetActive(false);
            bloods.Enqueue(b);

            GameObject cbt = (GameObject)Instantiate(combatText, this.transform.position, this.transform.rotation);
            DontDestroyOnLoad(cbt);
            cbt.SetActive(false);
            combatTexts.Enqueue(cbt);
        }
	}
	
	public static void GetBlood(Vector3 pos, Quaternion rot){ 
		if(bloods.Peek() != null && !bloods.Peek().activeSelf){	//se fija si la siguiente sangre no se esta usando
			GameObject b = bloods.Dequeue();
			b.transform.position = pos;
			b.transform.rotation = rot;
			b.SetActive(true);
			bloods.Enqueue(b);
		}
		else{ //si se esta usando instancia otra y la agrega a la cola
			GameObject b = (GameObject)Instantiate(bloodGoStatic, pos, rot);
			b.transform.position = pos;
			b.transform.rotation = rot;
			b.SetActive(true);
			bloods.Enqueue(b);
		}	
	}

    public static void GetCombatText(Vector3 pos, Quaternion rot, string text)
    {
        if (combatTexts.Peek() != null && !combatTexts.Peek().activeSelf)
        {   //se fija si la siguiente sangre no se esta usando
            GameObject cbt = combatTexts.Dequeue();
            cbt.transform.position = pos;
            cbt.transform.rotation = rot;
            cbt.SetActive(true);
            cbt.GetComponent<EnemyCombatText>().ShowCombatText(text);   
            combatTexts.Enqueue(cbt);
        }
        else { //si se esta usando instancia otra y la agrega a la cola
            GameObject cbt = (GameObject)Instantiate(combatTextStatic, pos, rot);
            cbt.transform.position = pos;
            cbt.transform.rotation = rot;
            cbt.GetComponent<EnemyCombatText>().ShowCombatText(text);
            cbt.SetActive(true);
            combatTexts.Enqueue(cbt);
        }
    }

    public static void ClearQueues(){
		while(bloods.Count > 0){
			Destroy (bloods.Dequeue());
		}
        while (combatTexts.Count > 0)
        {
            Destroy(combatTexts.Dequeue());
        }
    }

}
