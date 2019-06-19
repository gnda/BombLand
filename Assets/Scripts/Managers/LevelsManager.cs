using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class LevelsManager : Manager<LevelsManager> {

	[Header("LevelsManager")]
	#region levels & current level management
	private int currentLevelIndex;
	private GameObject currentLevelGO;
	private Level currentLevel;
	public Level CurrentLevel { get { return currentLevel; } }

	[SerializeField] private GameObject[] levelsPrefabs;

	protected override void Awake()
	{
		base.Awake();
	} 
	#endregion

	#region Manager implementation
	protected override IEnumerator InitCoroutine()
	{
		yield break;
	}
	#endregion

	#region Events' subscription
	public override void SubscribeEvents()
	{
		base.SubscribeEvents();
		EventManager.Instance.AddListener<GoToNextLevelEvent>(GoToNextLevel);
		EventManager.Instance.AddListener<GoToLevelEvent>(GoToLevel);
	}

	public override void UnsubscribeEvents()
	{
		base.UnsubscribeEvents();
		EventManager.Instance.RemoveListener<GoToNextLevelEvent>(GoToNextLevel);
		EventManager.Instance.RemoveListener<GoToLevelEvent>(GoToLevel);
	}
	#endregion

	#region Level flow
	void Reset()
	{
		Destroy(currentLevelGO);
		currentLevelGO = null;
		currentLevelIndex = -1;
	}

	void InstantiateLevel()
	{
		currentLevelIndex = Mathf.Max(currentLevelIndex, 0) % levelsPrefabs.Length;
		currentLevelGO = Instantiate(levelsPrefabs[currentLevelIndex]);
		currentLevel = currentLevelGO.GetComponent<Level>();
	}

	private IEnumerator GoToNextLevelCoroutine()
	{
		Destroy(currentLevelGO);
		while (currentLevelGO) yield return null;

		InstantiateLevel();

		
	}
	#endregion

	#region Callbacks to GameManager events
	protected override void GameMenu(GameMenuEvent e)
	{
		Reset();
	}
	protected override void GamePlay(GamePlayEvent e)
	{
		Reset();
	}
	
	public void GoToLevel(GoToLevelEvent e)
	{
		currentLevelIndex = e.eLevelIndex;
		StartCoroutine(GoToNextLevelCoroutine());
	}

	public void GoToNextLevel(GoToNextLevelEvent e)
	{
		Debug.Log("Here");
		currentLevelIndex++;
		StartCoroutine(GoToNextLevelCoroutine());
	}
	#endregion
}
