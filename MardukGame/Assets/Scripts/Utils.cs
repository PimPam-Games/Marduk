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

	public static int ChooseItem(Types.EnemyTypes eType)
    {
        float normalItemLimit = 0.7f;
        float magicItemLimit = 0.85f;
        float rarelItemLimit = 0.98f;
        switch (eType){ //cambia probabilidades de acuerdo al tipo del enemigo
            case Types.EnemyTypes.Champion:
                normalItemLimit = 0.52f;
                magicItemLimit = 0.75f;
                rarelItemLimit = 0.93f;
                break;
            case Types.EnemyTypes.MiniBoss:
                normalItemLimit = 0.26f;
                magicItemLimit = 0.65f;
                rarelItemLimit = 0.88f;
                break;
            case Types.EnemyTypes.Boss:
                normalItemLimit = -1f;
                magicItemLimit = 0.40f;
                rarelItemLimit = 0.82f;
                break;
        }
        float randomPoint = Random.value; // valor entre 0.0 y 1.0 inclusive
		if(randomPoint < normalItemLimit)
        {
			return 0; //normal
		}
		else{
			randomPoint = Random.value;
			if(randomPoint < magicItemLimit){ //cae un magico o un skill
				randomPoint = Random.value;
				if(randomPoint < 0.5f)
					return 1; //magico
				else
					return 4; //skill
			}
			else{
				randomPoint = Random.value;
				if(randomPoint < rarelItemLimit)
					return 2; //amarillo
				else
					return 3; //unico
			}
		}
	}
}
