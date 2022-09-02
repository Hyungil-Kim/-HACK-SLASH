using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{

	public struct ViewCastInfo
	{
		public bool hit;
		public Vector3 point;
		public float dis;
		public float angle;

		public ViewCastInfo(bool _hit, Vector3 _point, float _dis, float _angle)
		{
			hit = _hit;
			point = _point;
			dis = _dis;
			angle = _angle;
		}
	}
	public struct Edge
	{
		public Vector3 PointA, PointB;
		public Edge(Vector3 _PointA, Vector3 _PointB)
		{
			PointA = _PointA;
			PointB = _PointB;
		}
	}
	public float viewRadius;
	[Range(0, 360)]
	public float viewAngle;
	public List<Transform> visibleTargets = new List<Transform>();
	public LayerMask targetMask, obstacleMask;
	public float meshResolution;
	Mesh viewMesh;
	public MeshFilter viewMeshFilter;
	private int edgeResolveIterations;
	private float edgeDstThreshold;

	public bool isCollision=false;

	private void Start()
	{
		viewMesh = new Mesh();
		viewMesh.name = "View Mesh";
		viewMeshFilter.mesh = viewMesh;

		StartCoroutine(FindTargetsWithDelay(0.2f));
	}

	private void LateUpdate()
	{
		DrawFieldOfView();
	}

	IEnumerator FindTargetsWithDelay(float delay)
	{
		while (true)
		{
			yield return new WaitForSeconds(delay);
			FindVisibleTargets();
			CheckCollision();
		}
	}

	private void CheckCollision()
	{
		isCollision = visibleTargets.Count > 0 ? true : false;
	}

	private void FindVisibleTargets()
	{
		visibleTargets.Clear();

		Collider[] targetsInView = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
		for (int i = 0; i < targetsInView.Length; i++)
		{
			Transform target = targetsInView[i].transform;
			Vector3 dirToTarget = (target.position - transform.position).normalized;

			if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
			{
				float disToTarget = Vector3.Distance(transform.position, target.transform.position);

				if (!Physics.Raycast(transform.position, dirToTarget, disToTarget, obstacleMask))
				{
					visibleTargets.Add(target);
				}

			}

		}
	}

	public Vector3 DirFromAngle(float angleDegrees, bool angleIsGlobal)
	{
		if (!angleIsGlobal)
		{
			angleDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Cos((-angleDegrees + 90) * Mathf.Deg2Rad), 0, Mathf.Sin((-angleDegrees + 90) * Mathf.Deg2Rad));
	}
	ViewCastInfo ViewCast(float globalAngle)
	{
		Vector3 dir = DirFromAngle(globalAngle, true);
		RaycastHit hit;
		if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
		{
			return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
		}
		else
		{
			return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
		}
	}
	Edge FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
	{
		float minAngle = minViewCast.angle;
		float maxAngle = maxViewCast.angle;
		Vector3 minPoint = Vector3.zero;
		Vector3 maxPoint = Vector3.zero;

		for (int i = 0; i < edgeResolveIterations; i++)
		{
			float angle = minAngle + (maxAngle - minAngle) / 2;
			ViewCastInfo newViewCast = ViewCast(angle);
			bool edgeDstThresholdExceed = Mathf.Abs(minViewCast.dis - newViewCast.dis) > edgeDstThreshold;
			if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceed)
			{
				minAngle = angle;
				minPoint = newViewCast.point;
			}
			else
			{
				maxAngle = angle;
				maxPoint = newViewCast.point;
			}
		}

		return new Edge(minPoint, maxPoint);
	}
	void DrawFieldOfView()
	{
		int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
		float stepAngleSize = viewAngle / stepCount;
		List<Vector3> viewPoints = new List<Vector3>();
		ViewCastInfo prevViewCast = new ViewCastInfo();

		for (int i = 0; i <= stepCount; i++)
		{
			float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
			ViewCastInfo newViewCast = ViewCast(angle);

			if (i != 0)
			{
				bool edgeDstThresholdExceed = Mathf.Abs(prevViewCast.dis - newViewCast.dis) > edgeDstThreshold;

				if (prevViewCast.hit != newViewCast.hit || (prevViewCast.hit && newViewCast.hit && edgeDstThresholdExceed))
				{
					Edge e = FindEdge(prevViewCast, newViewCast);

					if (e.PointA != Vector3.zero)
					{
						viewPoints.Add(e.PointA);
					}

					if (e.PointB != Vector3.zero)
					{
						viewPoints.Add(e.PointB);
					}
				}
			}

			viewPoints.Add(newViewCast.point);
			prevViewCast = newViewCast;
		}

		int vertexCount = viewPoints.Count + 1;
		Vector3[] vertices = new Vector3[vertexCount];
		int[] triangles = new int[(vertexCount - 2) * 3];
		vertices[0] = Vector3.zero;
		for (int i = 0; i < vertexCount - 1; i++)
		{
			vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
			if (i < vertexCount - 2)
			{
				triangles[i * 3] = 0;
				triangles[i * 3 + 1] = i + 1;
				triangles[i * 3 + 2] = i + 2;
			}
		}
		viewMesh.Clear();
		viewMesh.vertices = vertices;
		viewMesh.triangles = triangles;
		viewMesh.RecalculateNormals();
	}

}
