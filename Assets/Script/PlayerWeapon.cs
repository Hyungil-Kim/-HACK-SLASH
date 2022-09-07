using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PlayerWeapon", menuName = "Scriptable Ob/PlayerWeapon")]
public class PlayerWeapon : ScriptableObject
{
	public GameObject weaponObject;
	public GameObject skill_Q_Ob;
	public GameObject skill_W_Ob;
	public GameObject skill_E_Ob;
	public GameObject skill_R_Ob;
	
	public Dictionary<KeyCode,Action> atkKeyCode = new();

	private void Awake()
	{
		AddSkill();
	}
	private void AddSkill()
	{
		atkKeyCode.Add(KeyCode.Q, Sklil_Q);
		atkKeyCode.Add(KeyCode.W, Sklil_W);
		atkKeyCode.Add(KeyCode.E, Sklil_E);
		atkKeyCode.Add(KeyCode.R, Sklil_R);
	}
	public void Sklil_Q()
	{

	}
	public void Sklil_W()
	{

	}
	public void Sklil_E()
	{

	}
	public void Sklil_R()
	{

	}
}
