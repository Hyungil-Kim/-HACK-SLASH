using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	public AudioSource currentBackGroundSource;
	float[] spectrum = new float[2048];
	public GameObject player;

	public void SettingMusic(AudioSource source)
	{
		currentBackGroundSource = source;
	}
	private void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}
	private void Update()
	{
		PlayGame();
	}
	public void SetPlayer(GameObject player)
	{
		this.player = player;
	}
	void PlayGame()
	{
		currentBackGroundSource.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
		for(int i =0; i < 8;i++)
		{
			//Debug.Log(spectrum[i]*1000);
		}
	}
}
