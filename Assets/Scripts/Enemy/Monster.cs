using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;
using SDD.Events;
using UnityEngine.PlayerLoop;

public class Monster : Enemy {

	public override float MoveDuration { get { return moveDuration;} }
	[SerializeField] private float moveDuration;

	private Vector3 NextDirection
	{
		get {
			List<Vector3> directions = new List<Vector3>() 
				{ Vector3.right,Vector3.forward,-Vector3.right,-Vector3.forward};

			for (int i = directions.Count - 1; i >= 0; i--)
			{
				RaycastHit hit;

				Physics.Raycast(new Ray(Transf.position, directions[i]), out hit);

				if (hit.transform.GetComponent<Player>())
					return directions[i];

				if (i < directions.Count - 1)
					directions.RemoveAt(Random.Range(0, 2) == 0 ? i + 1 : i);
			}

			return directions[0];
		}
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();

		if (!IsMoving)
		{
			EventManager.Instance.Raise(new MoveElementEvent() 
				{eMoveable = this, eDirection = NextDirection});
		}
	}
}
