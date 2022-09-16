using System;
using System.Linq;
using UnityEngine;
public class M_Magician : Monster
{
	private int A_AttackEnd = Animator.StringToHash("AttackEnd");

	public float sqrAngle = 15;
	public float sqrLength = 2;

	private Vector3 newPos;
	private Vector3[] pivots = new Vector3[4];
	private Vector3[] startPos = new Vector3[4];
	private Vector3[,] sqrPos = new Vector3[4, 4];
	public MeshFilter[] viewMeshFilter;
	Mesh[] mesh = new Mesh[4];

	private bool meshOn = false;
	private bool collision = false;

	protected override void Start()
	{
		base.Start();
		for (int i = 0; i < 4; i++)
		{
			mesh[i] = new Mesh();
			mesh[i].name = "View Mesh";
			viewMeshFilter[i].mesh = mesh[i];
		}
	}
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		newPos = transform.position;
		newPos.y = 0;
	}
	protected override void Update()
	{
		base.Update();
		if (meshOn)
		{
			for (int i = 0; i < 4; i++)
			{
				SetCollision(CheckPointInSqr(i, target.transform.position));
			}
		}
	}
	protected override void LateUpdate()
	{
		base.LateUpdate();
		if (meshOn)
		{
			for (int i = 0; i < 4; i++)
			{
				MakeMesh2(i);
			}
		}
	}
	public override void Search()
	{
		base.Search();
	}
	public override void Attack(Action action = null)
	{
		base.Attack(action);
	}

	protected override void AddAtkList()
	{
		atkDic.Add(() => Attack1(), 0.99f);
		atkDic.Add(() => Attack2(), 0.01f);
		atkDic.OrderBy(x => x.Value);
	}

	private void Attack1()
	{
		Debug.Log("1");
		sight.viewMeshFilter.gameObject.SetActive(false);
		//IsAttackId = Animator.StringToHash("IsAttack1");
		SetStartPos();
		for (int i = 0; i < 4; i++)
		{
			SetSqrPos(i, startPos[i]);
		}
		SetMesh(true);

	}

	private void Attack2()
	{
		//IsAttackId = Animator.StringToHash("IsAttack2");
	}


	protected override void EndAttack()
	{
		base.EndAttack();
		animator.SetTrigger(A_AttackEnd);
	}
	public Vector3 DirFromAngle(float angleDegrees)
	{
		return new Vector3(Mathf.Cos((-angleDegrees +90) * Mathf.Deg2Rad), 0, Mathf.Sin((-angleDegrees +90) * Mathf.Deg2Rad));
	}
	private void SetStartPos()
	{
		for (int i = 0; i < 4; i++)
		{
			startPos[i] = transform.position + DirFromAngle(i * 90) *2 ;
			startPos[i].y += 3;
		}
	}
	private void SetSqrPos(int num, Vector3 mainPos)
	{
		var radius = 0f;
		for (int i = 0; i < 4; i++)
		{
			if (i % 2 == num % 2)
				radius += sqrAngle;
			else
				radius += (180 - sqrAngle);

			mainPos.y = 0.01f;
			sqrPos[num, i] = (mainPos + DirFromAngle(radius)) * 2;			
		}
	}
	private void MakeMesh2(int num)
	{
		int vertexCount = sqrPos.GetLength(1);
		Vector3[] vertices = new Vector3[vertexCount];
		int[] triangles = new int[(vertexCount - 2) * 3];
		for (int i = 0; i < vertexCount ; i++)
		{
			vertices[i] = transform.InverseTransformPoint(sqrPos[num,i]);
			if (i < vertexCount - 2)
			{
				triangles[i * 3] = 0;
				triangles[i * 3 + 1] = i + 1;
				triangles[i * 3 + 2] = i + 2;
			}
		}
		mesh[num].Clear();
		mesh[num].vertices = vertices;
		mesh[num].triangles = triangles;
		mesh[num].RecalculateNormals();
	}

	private void SetMesh(bool boolean)
	{
		meshOn = boolean;
	}
	private void SetCollision(bool boolean)
	{
		collision = boolean;
	}

	private float GetAreaOfTriangle(Vector3 pos1, Vector3 pos2, Vector3 pos3)
	{
		Vector3 a = pos2 - pos1;
		Vector3 b = pos3 - pos1;
		Vector3 cross = Vector3.Cross(a, b);
		return cross.magnitude / 2.0f;
	}
	private bool CheckPointInSqr(int num, Vector3 point)
	{
		Vector3 a, b, c, d;
		float triarea1, triarea2, triarea3, triarea4;
		float horizontal;
		float vertical;
		a = sqrPos[num, 0];
		b = sqrPos[num, 1];
		c = sqrPos[num, 2];
		d = sqrPos[num, 3];

		horizontal = Vector3.Distance(a, b);
		vertical = Vector3.Distance(b, c);

		float area = horizontal * vertical;

		triarea1 = GetAreaOfTriangle(point, a, b);
		triarea2 = GetAreaOfTriangle(point, b, c);
		triarea3 = GetAreaOfTriangle(point, c, d);
		triarea4 = GetAreaOfTriangle(point, d, a);

		return (triarea1 + triarea2 + triarea3 + triarea4) < area + 0.1f;
	}

	private void MakeLine()
	{
		
	}

}
