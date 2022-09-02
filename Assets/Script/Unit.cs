using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    protected float hp;
    [SerializeField]
    protected float maxHp;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float rotateSpeed;
    [SerializeField]
    protected float atk;
    [SerializeField]
    protected float watingTime;

    protected bool IsDead = false;

    private float timer = 0f;
    public float Hp
    {
        get { return hp; }
        protected set { hp = value; }
    }
    public float Atk
	{
		get { return atk; }
        protected set { atk = value; }
	}
    public float Speed
    {
        get { return speed; }
        protected set { speed = value; }
    }
   
    protected void DoDamage(float hp,float atk)
	{
        hp = hp - atk <= 0 ? 0 : hp;
	}

    public void DelayTime(Action action)
    {
        timer += Time.deltaTime;
        if (timer > watingTime)
        {
            action();
            timer -= watingTime;
        }
    }
    protected bool CheckDeath()
	{
        if(hp <= 0)
		{
            return true;
		}
        return false;
	}
    protected void ResetTimer()
	{
        timer = 0;
	}
}
