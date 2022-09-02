using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float radius = 10f;

    [SerializeField]
    private string key = null;
   
    [SerializeField]
    private float spawnTime;
    
    [SerializeField]
    private int maxSpawn;

    [System.NonSerialized]
    public List<GameObject> monsterList = new();

    private float timer;
	private void Start()
	{
        SpawnMonster();
    }

	private void Update()
	{
       Spawning();
    }

    private void SpawnMonster()
	{
        for (int i = monsterList.Count; i < maxSpawn; i++)
        {
            monsterList.Add(ObjectPoolManager.Instance.Spawn(key,gameObject,false));
        }
        SetSpawnPoint();
    }

    private void Spawning()
	{
        if (monsterList.Count == 0)
        {
            timer += Time.deltaTime;
            if (timer >= spawnTime)
			{
                SpawnMonster();
                timer = 0; 
			}
        }
    }
    private void SetSpawnPoint()
	{
        if (monsterList.Count == 0) return;

        for(int i =0; i < monsterList.Count; i++)
		{
            monsterList[i].transform.position = RandomPointInSphere(3f);
            monsterList[i].SetActive(true);
		}
	}
    public Vector3 RandomPointInSphere(float radius)
    {
        Vector3 getPoint = Random.onUnitSphere;
        getPoint.y = 0.0f;
        float r = Random.Range(0.0f, radius);
        return getPoint * r;
    }
}
