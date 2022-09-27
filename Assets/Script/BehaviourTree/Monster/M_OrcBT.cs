using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class M_OrcBT : BehaviorTree.Tree
{
	protected override Node SetUpTree()
	{
		Node root = new Selector(new List<Node>
		{
			new Sequence(new List<Node>					//attack
			{
				new CheckPlayerInAtkRange(transform),
				new AttackNode(transform),
			}),
			new Sequence(new List<Node>					//move
			{
				new CheckPlayerInRange(transform),
				new TauntNode(transform),
				new MoveTargetNode(transform),
			}),
			new PatrolNode(transform),
		});

		return root;
	}
}
