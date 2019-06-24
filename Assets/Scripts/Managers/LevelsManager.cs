using System.Collections;
using UnityEngine;
using SDD.Events;

public class LevelsManager : Manager<LevelsManager> {

	[Header("LevelsManager")]
	
	#region levels & current level management
	
	private int currentLevelIndex;
	private GameObject currentLevelGO;


	[SerializeField] private GameObject[] levelsPrefabs;
	
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
		Player.PlayerCount = 0;

		EventManager.Instance.Raise(new LevelHasBeenDestroyedEvent());
		Destroy(currentLevelGO);
		currentLevelGO = null;
	}

	void InstantiateLevel()
	{
		currentLevelIndex = Mathf.Max(currentLevelIndex, 0) % levelsPrefabs.Length;
		currentLevelGO = Instantiate(levelsPrefabs[currentLevelIndex]);
	}

	private IEnumerator GoToNextLevelCoroutine()
	{
		Destroy(currentLevelGO);
		while (currentLevelGO) yield return null;

		if (currentLevelIndex == levelsPrefabs.Length)
			EventManager.Instance.Raise(new CreditsButtonClickedEvent());
		else InstantiateLevel();
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
		Player.PlayerCount = 0;
		
		if (e.eLevelIndex != -1)
			currentLevelIndex = e.eLevelIndex;
		else
			currentLevelIndex++;
		
		StartCoroutine(GoToNextLevelCoroutine());
	}
	#endregion
}
