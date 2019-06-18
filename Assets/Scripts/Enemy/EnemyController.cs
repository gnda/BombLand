using System.Collections;
using System.Collections.Generic;
using Common;
using DefaultNamespace;
using JetBrains.Annotations;
using UnityEngine;
using SDD.Events;

public class EnemyController : SimpleGameStateObserver, IMoveable{
	
	public Transform Transf { get; set; }
	public bool IsMoving { get; set; }
	public char Symbol { get; set; }

	private int[,] step;
	
	char[,] tilesState;

	protected override void Awake()
	{
		base.Awake();
		Transf = GetComponent<Transform>();
	}

	// Use this for initialization
	void Start () {
		tilesState = GameManager.Instance.Level.TilesState;
		step = new int[tilesState.GetLength(0),tilesState.GetLength(1)];
		
		Debug.Log(step[0,0]);
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (GameManager.Instance && !GameManager.Instance.IsPlaying) return;

		if (!IsMoving) Move();
	}

	private void Move()
	{
		
	}
}