using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
	private bool isActive = false;
	public float speed;
	private void Awake()
	{
		SetActive(true);
	}

	private void Update()
	{
		StartCoroutine("Move");
	}

	private IEnumerator Move()
	{
		while (isActive)
		{
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, transform.position.y, 0), Time.deltaTime * speed);
			SetRotation();
			ReachPos(new Vector3(0, transform.position.y, 0));
			yield return new WaitForEndOfFrame();
		}
			yield break;
	}
	private void SetRotation()
	{
		transform.LookAt(new Vector3(0, transform.position.y, 0));
	}
	private void SetActive(bool active)
	{
		isActive = active;
		gameObject.SetActive(isActive);
	}
	private void ReachPos(Vector3 endPos)
	{
		float dis = Vector3.Distance(transform.position, endPos);
		if(dis < 0.1f)
		{
			SetActive(false);
		}
	}
}
