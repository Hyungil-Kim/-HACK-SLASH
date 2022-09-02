using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectType = System.String;

[DisallowMultipleComponent]
public class PoolObject : MonoBehaviour
{
    public ObjectType key;

    public PoolObject Clone()
	{
		GameObject go = Instantiate(gameObject);
		if (!go.TryGetComponent(out PoolObject ob))
			ob = go.AddComponent<PoolObject>();
		go.SetActive(false);

		return ob;
	}

	public void Active(bool active)
	{
		gameObject.SetActive(active);
	}

}
