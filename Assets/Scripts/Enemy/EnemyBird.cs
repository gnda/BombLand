using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

public class EnemyBird : Enemy {

	enum Phase { wait,move};

	Vector3 m_TranslationDir;
	[SerializeField] float m_MoveDuration;
	[SerializeField] float m_WaitDuration;

	Phase m_Phase;
	float m_TimeNextPhase;
	float m_Time;

	Transform m_Player;
	char[,] tilesState;
	
	public BoxCollider m_BoxCollider { get; set; }

	private Vector3 NextDirection
	{
		get {
			List<Vector3> directions = new List<Vector3>() { Vector3.right/*,Vector3.forward,-Vector3.right,-Vector3.forward*/};
			
			for (int i = directions.Count-1; i >=0; i--)
			{
				/*List<RaycastHit> hits = Physics.BoxCastAll(Transf.position,)
					
					
					Physics.SphereCastAll(Transf.position,m_BoxCollider.radius, directions[i], 
					m_MoveDuration*m_TranslationSpeed).Where(
					item => item.transform.CompareTag("Platform")|| 
					        item.transform.CompareTag("Limit")||
					        item.transform.CompareTag("Ground")).ToList();
				if (hits.Count > 0)
				{
					hits.Sort((a, b) => Vector3.Distance(a.point, Transf.position)
						.CompareTo(Vector3.Distance(b.point, Transf.position)));
//					if (Vector3.Distance(hits[0].point, m_Transform.position) < m_MoveDuration * m_TranslationSpeed + m_SphCollider.radius + .1f)
					foreach (RaycastHit h in hits)
					{
						Debug.Log(h.transform.position);
						Debug.DrawLine(Transf.position, h.transform.position, Color.red, 100f);
					}
					directions.RemoveAt(i);
				}*/
			}

			Debug.Log("END");
			if (directions.Count == 0) return Vector3.zero;

			// on choisit la direction qui nous rapproche le plus du player
			directions.Sort((a, b) => Vector3.Distance(
					Transf.position + m_MoveDuration * m_TranslationSpeed * a, m_Player.position)
				.CompareTo(Vector3.Distance(Transf.position + b * m_MoveDuration * m_TranslationSpeed, 
					m_Player.position)));
			
			return directions[0];
		}
	}

	protected override void Awake()
	{
		base.Awake();
		//m_SphCollider = GetComponent<SphereCollider>();
		//m_SphCollider = GetComponent<SphereCollider>();
	}

	protected override void Start()
	{
		base.Start();
		m_Player = GameManager.Instance.PlayerTransforms[0];
		tilesState = GameManager.Instance.Level.TilesState;

		m_Phase = Phase.wait;
		m_TimeNextPhase = m_WaitDuration;
		m_Time = 0;
		m_TranslationDir = NextDirection;
	}

	protected override Vector3 MoveVect
	{
		get
		{
			m_Time += Time.fixedDeltaTime;

			if(m_Time>m_TimeNextPhase)
			{
				m_Time = 0;
				switch(m_Phase)
				{
					case Phase.wait:
						m_Phase = Phase.move;
						m_TimeNextPhase = m_MoveDuration;
						m_TranslationDir = NextDirection;
						break;
					case Phase.move:
						m_Phase = Phase.wait;
						m_TimeNextPhase = m_WaitDuration;
						break;
				}
			}

			if (m_Phase == Phase.move)
				return m_TranslationSpeed * Time.fixedDeltaTime * m_TranslationDir;
			else return Vector3.zero;
		}
	}
}
