using UnityEngine;
using System.Collections;

public class Utils{

	/*Selecciona una posicion del arreglo de acuerdo a las probabilidades que tenga*/
	public static int Choose (float[] probs) {
		
		float total = 0;
		
		foreach (float elem in probs) {
			total += elem;
		}
		
		float randomPoint = Random.value * total;
		for (int i= 0; i < probs.Length; i++) {
			if (randomPoint < probs[i]) {
				return i;
			}
			else {
				randomPoint -= probs[i];
			}
		}
		return probs.Length - 1;
	}

	public static int ChooseItem(){
		float randomPoint = Random.value;
		if(randomPoint < 0.66f){
			return 0; //normal
		}
		else{
			randomPoint = Random.value;
			if(randomPoint < 0.85f){ //cae un magico o un skill
				randomPoint = Random.value;
				if(randomPoint < 0.5f)
					return 1; //magico
				else
					return 4; //skill
			}
			else{
				randomPoint = Random.value;
				if(randomPoint < 0.96f)
					return 2; //amarillo
				else
					return 3; //unico
			}
		}
	}
}
