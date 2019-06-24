using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDD.Events;

public class HudManager : Manager<HudManager>
{
	#region Labels & Values
	[Header("HudManager")]
	[SerializeField] private GameObject panelHUD;
	
	[Header("Texts")]
	[SerializeField] private Text txtBestScore;
	[SerializeField] private Text txtMonstersLeft;
	[SerializeField] private Text txtTimeLeft;
	
	[Header("ScoresTransform")]
	[SerializeField] private RectTransform playerOneScoreTransform;
	[SerializeField] private RectTransform playerTwoScoreTransform;
	[SerializeField] private RectTransform playerThreeScoreTransform;
	[SerializeField] private RectTransform playerFourScoreTransform;
	#endregion
	
	private List<Transform> playersScoreTransforms;
	
	#region Monobehaviour lifecycle
	private void Update()
	{
		if (txtTimeLeft.text != "0")
			txtTimeLeft.text = ((int) GameManager.Instance.Timer).ToString();
	}
	#endregion

	#region Manager implementation
	protected override IEnumerator InitCoroutine()
	{
		playersScoreTransforms = new List<Transform>()
		{
			playerOneScoreTransform,
			playerTwoScoreTransform,
			playerThreeScoreTransform,
			playerFourScoreTransform
		};

		panelHUD.SetActive(false);
		yield break;
	}
	#endregion

	#region Events subscription
	public override void SubscribeEvents()
	{
		base.SubscribeEvents();

		//level
		EventManager.Instance.AddListener<GameHasStartedEvent>(GameHasStarted);
		EventManager.Instance.AddListener<GoToNextLevelEvent>(GoToNextLevel);
		
	}
	public override void UnsubscribeEvents()
	{
		base.UnsubscribeEvents();

		//level
		EventManager.Instance.RemoveListener<GameHasStartedEvent>(GameHasStarted);
		EventManager.Instance.RemoveListener<GoToNextLevelEvent>(GoToNextLevel);

	}
	#endregion

	#region Callbacks to Level events

	private void GoToNextLevel(GoToNextLevelEvent e)
	{
		panelHUD.SetActive(true);
	}
	#endregion

	#region Callbacks to GameManager events
	protected override void GameStatisticsChanged(GameStatisticsChangedEvent e)
	{
		List<Transform> playersScoreActiveTransform = 
			playersScoreTransforms.FindAll(
				item => item.gameObject.activeInHierarchy);
		
		List<Text> playersScore = new List<Text>();

		foreach (var transform in playersScoreActiveTransform)
			playersScore.Add(transform.GetComponentsInChildren<Text>()[1]);

		if (e.ePlayerNumber != -1)
		{
			if(e.eScore != 0)
				playersScore[e.ePlayerNumber - 1].text = e.eScore.ToString();
		}
		else
			foreach (var score in playersScore)
				score.text = "0";
		
		if (e.eBestScore != 0)
			txtBestScore.text = e.eBestScore.ToString();

        txtMonstersLeft.text = e.eNMonstersLeft.ToString();
	}

	private void GameHasStarted(GameHasStartedEvent e)
	{
		txtTimeLeft.text = GameManager.Instance.Timer.ToString();
	}
	#endregion
	
	#region Callbacks to MenuManager events
	protected override void GameMenu(GameMenuEvent e)
	{
		if (panelHUD.activeInHierarchy)
		{
			foreach (var transf in playersScoreTransforms)
				transf.gameObject.SetActive(true);
			panelHUD.SetActive(false);
		}
	}
	
	protected override void GamePlay(GamePlayEvent e)
	{
		int nbPlayer = GameManager.Instance.NumberOfPlayer;
		
		for (int i = nbPlayer; i < 4; i++)
			playersScoreTransforms[i].gameObject.SetActive(false);
	}
	
	protected override void GameCredits(GameCreditsEvent e)
	{
		panelHUD.SetActive(false);
	}
	#endregion
}