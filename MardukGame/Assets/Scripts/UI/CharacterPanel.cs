using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using p = PlayerStats;

public class CharacterPanel : MonoBehaviour {

	public GameObject offensivesPanel;
	public GameObject defensivesPanel;
	public GameObject utilsPanel;
	public GameObject addAtributesPanel;
	public Button btnOffensives;
	public Button btnDefensives;
	public Button btnUtils;	

	public Text txtOffensives;
	public Text txt2Offensives;
	public Text txtDefensives;
	public Text txt2Defensives;
	public Text txtUtils;
	public Text txt2Utils;

	public Text txtName;
	public Text txtStrength;
	public Text txtDexterity;
	public Text txtVitality;
	public Text txtSpirit;


	private const float ABtnSelected = 0.4f; 
	private const float RBtnSelected = 0.4f; 
	private const float GBtnSelected = 0.4f; 
	private const float BBtnSelected = 0.4f;

	void Awake(){
		offensivesPanel.SetActive(true); //empieza con la primera pestaña pulsada
		defensivesPanel.SetActive(false);
		utilsPanel.SetActive(false);
		btnOffensives.image.color = new Color(RBtnSelected,GBtnSelected,BBtnSelected,ABtnSelected);
		btnDefensives.image.color = new Color(1,1,1,ABtnSelected); 
		btnUtils.image.color = new Color(1,1,1,ABtnSelected); 
		
	}

	void Update(){
		UpdateOffensives();
		UpdateDefensives();
		UpdateUtils();
		txtStrength.text = p.atributes[p.Strength].ToString();
		txtDexterity.text = p.atributes[p.Dextery].ToString();
		txtName.text = p.playerName;
		txtSpirit.text = p.atributes[p.Spirit].ToString();
		txtVitality.text = p.atributes[p.Vitality].ToString();
		if(addAtributesPanel.activeSelf){
			if(p.atributesPoints <= 0)
				addAtributesPanel.SetActive(false);
		}
		else{
			if(p.atributesPoints>0)
				addAtributesPanel.SetActive(true);
		}
	}

	private void UpdateOffensives(){
		float critChance = 0;
		if (!Traits.traits [Traits.ACCURACY].isActive ()) {
			critChance = ((p.offensives [p.CritChance] + p.offensives [p.CritChance] * (p.offensives [p.IncreasedCritChance] / 100))*100);
		}
		txtOffensives.text = "Physical Damage         " + "\n"
			+"Attacks Per Second      " + "\n"
				+"Accuracy                \n" 
				+"Critical Chance         \n" 
				+"Crit Dmg Multiplier       \n" 
				+"Mana Per Second      \n"  
				+"Increased Cast Speed \n" ;
		txt2Offensives.text = System.Math.Round(p.offensives[p.MinDmg],1) + " - " + System.Math.Round(p.offensives[p.MaxDamge],1) + "\n"
			+(System.Math.Round(p.offensives[p.BaseAttacksPerSecond] + p.offensives [p.BaseAttacksPerSecond] * (p.offensives [p.IncreasedAttackSpeed]/100),2)).ToString() + "\n"
				+ 	p.offensives[p.Accuracy].ToString()+ "\n"
				+ critChance.ToString() + "%"+ "\n"
				+ p.offensives[p.CritDmgMultiplier].ToString()+ "\n"
				+ p.offensives[p.ManaPerSec].ToString()+ "% \n"
				+ p.offensives[p.IncreasedCastSpeed].ToString() + "%" + "\n";	
	
	}

	private void UpdateDefensives(){
		txtDefensives.text = "Defense         " + "\n"
							+"Evasion Rating      " + "\n"
							+"Cold Resistance               \n" 
							+"Fire Resistance        \n" 
							+"Lightning Resistance       \n" 
							+"Poison Resistance      \n"  
							+"Block Chance \n" 
							+"Life Gained Per Hit \n" ;
		txt2Defensives.text = p.defensives[p.Defense].ToString() + "\n"
							+ p.defensives[p.Evasiveness].ToString() + "\n"
				+ (p.defensives[p.ColdRes]+p.defensives[p.AllRes]).ToString() + "\n"
				+ (p.defensives[p.FireRes]+p.defensives[p.AllRes]).ToString() + "\n"
				+ (p.defensives[p.LightRes]+p.defensives[p.AllRes]).ToString() + "\n"
				+ (p.defensives[p.PoisonRes]+p.defensives[p.AllRes]).ToString() + "\n"
				+ p.defensives[p.BlockChance].ToString() + "\n"
				+ p.defensives[p.LifePerHit].ToString() + "\n";
	}

	private void UpdateUtils(){
		txtUtils.text = "Movement Speed  \n"
						+ "Magic Find \n";
		txt2Utils.text = p.utils[p.MovementSpeed].ToString() + "\n"
			+  p.utils[p.MagicFind].ToString() + "\n" ;

	}

	public void ButtonPressed(int id){
		switch (id){
			case 1: //btn offensives seleccionado
				offensivesPanel.SetActive(true);
				defensivesPanel.SetActive(false);
				utilsPanel.SetActive(false);
				btnOffensives.image.color = new Color(RBtnSelected,GBtnSelected,BBtnSelected,ABtnSelected);
				btnDefensives.image.color = new Color(1,1,1,ABtnSelected); 
				btnUtils.image.color = new Color(1,1,1,ABtnSelected); 
				break;
			case 2://btn defensives seleccionado
				offensivesPanel.SetActive(false);
				defensivesPanel.SetActive(true);
				utilsPanel.SetActive(false);
				btnOffensives.image.color = new Color(1,1,1,ABtnSelected);
				btnDefensives.image.color = new Color(RBtnSelected,GBtnSelected,BBtnSelected,ABtnSelected); 
				btnUtils.image.color = new Color(1,1,1,ABtnSelected); 
				break;
			case 3://btn utils seleccionado
				offensivesPanel.SetActive(false);
				defensivesPanel.SetActive(false);
				utilsPanel.SetActive(true);
				btnOffensives.image.color = new Color(1,1,1,ABtnSelected);
				btnDefensives.image.color = new Color(1,1,1,ABtnSelected); 
				btnUtils.image.color = new Color(RBtnSelected,GBtnSelected,BBtnSelected,ABtnSelected); 
				break;
		}
	}

	public void AddAtributBtnPressed(int id){
		if(p.atributesPoints <= 0)
			return;
		switch (id){
			case 0:
				p.atributesPoints--;
				p.strAddedPoints++;
				p.AddAtribute(0);
			break;
			case 1:
				p.atributesPoints--;
				p.dexAddedPoints++;
				p.AddAtribute(1);
			break;
			case 2:
				p.atributesPoints--;
				p.vitAddedPoints++;
				p.AddAtribute(2);
			break;
			case 3:
				p.atributesPoints--;
				p.spiAddedPoints++;
				p.AddAtribute(3);
			break;	
		}
	}

}
