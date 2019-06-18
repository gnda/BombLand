using SDD.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

using Random = UnityEngine.Random;

public abstract class Enemy : SimpleGameStateObserver,IScore,IMoveable {

	enum State { none, enemy,coin }
	State m_State;
	
	public bool IsMoving { get; set; }
	public char Symbol { get; set; }
	public Transform Transf { get; set; }

	public bool IsEnemy { get { return m_State == State.enemy; } }

	public bool IsCoin
	{
		get { return m_State == State.coin; }
	}

	IEnumerator BeACoinCoroutine(float duration)
	{
		m_State = State.coin;
		yield return new WaitForSeconds(duration);
		m_State = State.enemy;
	}
	public void BeACoin(float duration)
	{
		StartCoroutine(BeACoinCoroutine(duration));
	}

	[Header("Movement")]
	[SerializeField] private float m_MaxTranslationSpeed;
	[SerializeField] private float m_MinTranslationSpeed;
	[SerializeField] private AnimationCurve m_TranslationSpeedProbaCurve;
	protected float m_TranslationSpeed;
	public float TranslationSpeed { get { return m_TranslationSpeed; } }
	
	char[,] tilesState;

	protected abstract Vector3 MoveVect { get; }

	//Enemy Gfx 
	[SerializeField] Transform m_EnemyGfx;


	[Header("Score")]
	[SerializeField] int m_ScoreCoin;
	public int Score { get { return m_ScoreCoin; } }

	protected bool m_Destroyed = false;

	protected override void Awake()
	{
		base.Awake();
		
		Transf = GetComponent<Transform>();

		m_TranslationSpeed = Mathf.Lerp(m_MinTranslationSpeed, m_MaxTranslationSpeed, 
			m_TranslationSpeedProbaCurve.Evaluate(Random.value));
	}

	protected virtual void Start()
	{
		m_State = State.enemy;
	}

	private void Update()
	{
		m_EnemyGfx.gameObject.SetActive(m_State == State.enemy);
	}

	public virtual void FixedUpdate()
	{
		if (!GameManager.Instance.IsPlaying) return;

		if(IsEnemy)
			Transf.position = Transf.position + MoveVect;
	}

	private void OnTriggerEnter(Collider col)
	{
		//Debug.Log( name+" Collision with " + col.gameObject.name);
		//Debug.Break();
		if(GameManager.Instance.IsPlaying
			&& col.gameObject.CompareTag("Player"))
		{
			if (IsCoin)
			{
				EventManager.Instance.Raise(new ScoreItemEvent() { eScore = this as IScore });
				EventManager.Instance.Raise(new EnemyHasBeenDestroyedEvent() { eEnemy = this, eDestroyedByPlayer = true });
				m_Destroyed = true;
				Destroy(gameObject);
			}
		}
	}
}
