using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class LevelsManager : Manager<LevelsManager> {

	[Header("LevelsManager")]
	#region levels & current level management
	private int m_CurrentLevelIndex;
	private GameObject m_CurrentLevelGO;
	private Level m_CurrentLevel;
	public Level CurrentLevel { get { return m_CurrentLevel; } }
	[SerializeField] private Texture2D[] levelDesigns;
	[SerializeField] private GameObject[] levelPrefabs;

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
	}

	public override void UnsubscribeEvents()
	{
		base.UnsubscribeEvents();
		EventManager.Instance.RemoveListener<GoToNextLevelEvent>(GoToNextLevel);
	}
	#endregion

	#region Level flow
	void Reset()
	{
		Destroy(m_CurrentLevelGO);
		m_CurrentLevelGO = null;
		m_CurrentLevelIndex = -1;
	}

	void GenerateLevel(int levelIndex)
	{
		Instantiate(levelPrefabs[0], new Vector3(0,0,0), Quaternion.identity);
		// Iterate through it's pixel
		for (int i = 0; i < levelDesigns[0].width; i++)
		{
			for (int j = 0; j < levelDesigns[0].height; j++)
			{
				Debug.Log(levelDesigns[0].GetPixel(i, j));
				
			}
		}
		//levelIndex = Mathf.Max(levelIndex, 0) % m_LevelsPrefabs.Length;
		//m_CurrentLevelGO = Instantiate(m_LevelsPrefabs[levelIndex]);
		//m_CurrentLevel = m_CurrentLevelGO.GetComponent<Level>();
	}

	private IEnumerator GoToNextLevelCoroutine()
	{
		Destroy(m_CurrentLevelGO);
		while (m_CurrentLevelGO) yield return null;

		GenerateLevel(m_CurrentLevelIndex);

		EventManager.Instance.Raise(new LevelHasBeenInstantiatedEvent() { eLevel = m_CurrentLevel });
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

	public void GoToNextLevel(GoToNextLevelEvent e)
	{
		m_CurrentLevelIndex++;
		StartCoroutine(GoToNextLevelCoroutine());
	}
	#endregion
}
