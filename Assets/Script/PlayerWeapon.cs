using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Enum;
[CreateAssetMenu(fileName = "PlayerWeapon", menuName = "Scriptable Ob/PlayerWeapon")]
public class PlayerWeapon : ScriptableObject
{
	public GameObject weaponObject;
	[NonSerialized]
	public IWeapon weapon;
	[SerializeField]
	public WeaponType weaponType;
	[SerializeField]
	private float weaponAtk;
	public Dictionary<KeyCode,Action> atkKeyCode = new();

	private void OnEnable()
	{
		weapon = weaponObject.GetComponent<IWeapon>();
	}
	public void SetSkill()
	{
		atkKeyCode.Add(KeyCode.Q, Sklil_Q);
		atkKeyCode.Add(KeyCode.W, Sklil_W);
		atkKeyCode.Add(KeyCode.E, Sklil_E);
		atkKeyCode.Add(KeyCode.R, Sklil_R);
	}
	public void Sklil_Q()
	{
		weapon.Skill_Q();
	}
	public void Sklil_W()
	{
		weapon.Skill_W();
	}
	public void Sklil_E()
	{
		weapon.Skill_E();
	}
	public void Sklil_R()
	{
		weapon.Skill_R();
	}
}
