using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ObjectType = System.String;
[Serializable]
public class ObjectPoolData
{
	public const int MIN_COUNT = 5;
	public string type = null;
	public GameObject prefab;
	public int min_count = MIN_COUNT;
}
public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
	protected ObjectPoolManager() { }
	[SerializeField]
	private List<ObjectPoolData> objectPoolDatas = new List<ObjectPoolData>();

	private Dictionary<ObjectType, GameObject> prefabDic;
	private Dictionary<ObjectType, ObjectPoolData> dataDic;
	private Dictionary<ObjectType, Stack<GameObject>> poolDic;
	private Dictionary<GameObject, Stack<GameObject>> clonePoolDic;

	private void OnEnable()
	{
		Init();
		DontDestroyOnLoad(this);
	}
	private void Init()
	{
		int count = objectPoolDatas.Count;
		if (count == 0) return;

		prefabDic = new Dictionary<ObjectType, GameObject>(count);
		dataDic = new Dictionary<ObjectType, ObjectPoolData>(count);
		poolDic = new Dictionary<ObjectType, Stack<GameObject>>(count);
		clonePoolDic = new Dictionary<GameObject, Stack<GameObject>>(count * ObjectPoolData.MIN_COUNT);

		foreach(var data in objectPoolDatas)
		{
			Register(data);
		}
	}
	private void Register(ObjectPoolData data)
	{
		if(poolDic.ContainsKey(data.type))
		{
			return;
		}

		GameObject sample =data.prefab;
		sample.name = data.prefab.name;
		sample.SetActive(false);

		Stack<GameObject> pool = new Stack<GameObject>(data.min_count);
		for(int i =0; i<data.min_count;i++)
		{
			GameObject clone = Instantiate(data.prefab);
			clone.SetActive(false);
			pool.Push(clone);

			clonePoolDic.Add(clone, pool);
		}

		prefabDic.Add(data.type, sample);
		dataDic.Add(data.type, data);
		poolDic.Add(data.type, pool);
	}

	private GameObject CloneFromSample(ObjectType key)
	{
		if (!prefabDic.TryGetValue(key, out GameObject sample)) return null;

		return Instantiate(sample);
	}

	public GameObject Spawn(ObjectType key, GameObject parent = null,bool active = false)
	{
		if (!poolDic.TryGetValue(key, out var pool)) return null;

		GameObject go;

		if(pool.Count > 0)
		{
			go = pool.Pop();
		}
		else
		{
			go = CloneFromSample(key);
			clonePoolDic.Add(go, pool);
		}

		go.SetActive(active);
		go.transform.SetParent(parent.transform);

		return go;
	}
	
	public void DeSpawn(GameObject go)
	{
		if(!clonePoolDic.TryGetValue(go,out var pool))
		{
			Destroy(go);
			return;
		}
		go.SetActive(false);
		pool.Push(go);
	}

}
