using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_bullet : MonoBehaviour
{
	Vector3[] point = new Vector3[4];

	private float maxTimer = 0;
	private float timer = 0;
	public float speed;

	public void Init(Transform startPos,Transform endPos,float newStartPoint,float newEndPoint)
	{
		maxTimer = Random.Range(0.8f, 1.0f);
		point[0] = startPos.position;
		point[1] = startPos.position +
			(newStartPoint * Random.Range(-1.0f, 1.0f) * startPos.right) + 
			(newStartPoint * Random.Range(-0.15f, 1.0f) * startPos.up) +
			(newStartPoint * Random.Range(-1.0f, -0.8f) * startPos.forward);

		point[2] = endPos.position +
		  (newEndPoint * Random.Range(-1.0f, 1.0f) * endPos.right) +
		  (newEndPoint * Random.Range(-1.0f, 1.0f) * endPos.up) + 
		  (newEndPoint * Random.Range(0.8f, 1.0f) * endPos.forward);

		point[3] = endPos.position;
		transform.position = startPos.position;
	}
	public IEnumerator ShootBullet()
	{
		if (timer > maxTimer)
			yield break;

		timer += Time.deltaTime * speed;

		transform.position = new Vector3(
			CubicBezierCurve(point[0].x, point[1].x, point[2].x, point[3].x),
			CubicBezierCurve(point[0].y, point[1].y, point[2].y, point[3].y),
			CubicBezierCurve(point[0].z, point[1].z, point[2].z, point[3].z));
		yield return 0;
	}
	private float CubicBezierCurve(float a, float b, float c, float d)
	{
		float t = timer / maxTimer;

		float ab = Mathf.Lerp(a, b, t);
		float bc = Mathf.Lerp(b, c, t);
		float cd = Mathf.Lerp(c, d, t);

		float abbc = Mathf.Lerp(ab, bc, t);
		float bccd = Mathf.Lerp(bc, cd, t);

		return Mathf.Lerp(abbc, bccd, t);
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag != "Monster")
		{
			StopCoroutine(ShootBullet());
			timer = 0f;
			ObjectPoolManager.Instance.DeSpawn(gameObject);
		}
		
	}
}
