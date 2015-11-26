using UnityEngine;
using System.Collections;

public class UtilitySkill: SpellStats{

	public float movementX;      //para movimiento
	public float movementY;
	public float moveTime; //tiempo en el que tiene que estar activado el skill de movimieto


	public override string ToolTip ()
	{
		string tooltip = "";
		tooltip =  base.ToolTip();
		tooltip += "<color=#EAC117>----------------------------------</color> \n";		
		if(string.Compare(spellName,"Dash") == 0){
			tooltip += "Performs a quick movement to one side \n \n";
			tooltip += "Movement Speed: " + movementX + "\n";
		}
		return tooltip;
	}
}


