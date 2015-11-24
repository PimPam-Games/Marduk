using UnityEngine;
using System.Collections;

public class Support : SpellStats { 

	public Types.Element dmgElement = 0;
	public float damageAdded = 0;
	
	public override string ToolTip ()
	{
		string tooltip = "";
		tooltip =  base.ToolTip();
		tooltip += "<color=red>----------------------------------</color> \n";
		if(damageAdded > 0 && dmgElement != Types.Element.None)
			tooltip += "Adds " + damageAdded + " of " + dmgElement + " Damage \n";
		if(damageAdded > 0 && dmgElement == Types.Element.None)
			tooltip += "Adds " + damageAdded + " of Physical Damage \n";
		return tooltip;
	}

}
