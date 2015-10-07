using UnityEngine;
using System;
using p = PlayerStats;

[Serializable]
public class Trait
{
	private string name;
	private int cost;
	private bool active;
	private string description;

	public string getName(){
		return name;
	}
	public int getCost(){
		return cost;
	}
	public bool isActive(){
		return active;
	}
	public string getDescription(){
		return description;
	}

	public void setName(string s){
		name = s;
	}
	public void setCost(int i){
		cost = i;
	}

	public void setActive(bool b){
		active = b;
	}
	public void setDescription(string s){
		description = s;
	}

	public Trait(string n, int c, string d){
		name = n;
		cost = c;
		description = d;
	}

}


