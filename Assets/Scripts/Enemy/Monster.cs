using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;
using SDD.Events;

public class Monster : MonoBehaviour, IMoveable, IScore {
	
	#region Settings
	[Header("MonsterSettings")]
	[SerializeField] private int score = 100;
	public int Score { get {return score;} }
	#endregion
	
	#region Movement
	[Header("MovementSettings")]
	[SerializeField] private float moveDuration = 0.6f;
	
	public Transform Transf { get; set; }
	public bool IsMoving { get; set; }
	public bool IsDestroyed { get; set; }
	
	public float MoveDuration
	{
		get { return moveDuration; }
		set { moveDuration = value; }
	}
	
	private Vector3 NextDirection
	{
		get {
			List<Vector3> directions = new List<Vector3>() 
				{ Vector3.right,Vector3.forward,-Vector3.right,-Vector3.forward};

			for (int i = directions.Count - 1; i >= 0; i--)
			{
				RaycastHit hit;
				Physics.Raycast(Transf.position, directions[i], out hit);

				if (hit.transform.GetComponent<Player>())
					return directions[i];

				if (i < directions.Count - 1)
					directions.RemoveAt(Random.Range(0, 2) == 0 ? i + 1 : i);
			}

			return directions[0];
		}
	}
	#endregion

	#region MonoBehaviour lifecycle
	protected void Awake()
	{
		Transf = GetComponent<Transform>();
	}
	
	public void FixedUpdate()
	{
		if (!GameManager.Instance.IsPlaying) return;

		if (!IsMoving)
			EventManager.Instance.Raise(new MoveElementEvent() 
				{eMoveable = this, eDirection = NextDirection});
	}
	#endregion
}