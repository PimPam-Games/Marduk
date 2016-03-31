using UnityEngine;
using System.Collections;

public class Support : SpellStats { 

	public Types.Element dmgElement = 0;
	public float damageAdded = 0;
	
	public override string ToolTip ()
	{
		string tooltip = "";
		tooltip =  base.ToolTip();
		if(damageAdded > 0){
			tooltip += "Only works with melee and Ranged skills \n";
		}
		tooltip += "<color=grey>----------------------------------</color> \n";
		if(string.Compare(spellName,"Iced Damage") == 0){
			tooltip += "Adds ice damage to one skill \n \n";
		}
		if(damageAdded > 0 && dmgElement != Types.Element.None)
			tooltip += "Adds " + damageAdded + "% of " + dmgElement + " Damage \n";
		if(damageAdded > 0 && dmgElement == Types.Element.None)
			tooltip += "Adds " + damageAdded + "% of Physical Damage \n";
		return tooltip;
	}

}
