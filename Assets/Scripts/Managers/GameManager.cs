using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using System.Linq;

public enum GameState {gameMenu, gamePlay,gameNextLevel,gamePause,gameOver,gameVictory, gameCredits}

public class GameManager : Manager<GameManager> {
	
	#region Time
	void SetTimeScale(float newTimeScale)
	{
		Time.timeScale = newTimeScale;
	}

	#endregion

	#region Game State
	private GameState m_GameState;
	public bool IsPlaying { get { return m_GameState == GameState.gamePlay; } }
	#endregion

	//LIVES
	#region Lives
	[Header("GameManager")]
	[SerializeField]
	private int m_NStartLives;

	private int m_NLives;
	public int NLives { get { return m_NLives; } }
	void DecrementNLives(int decrement)
	{
		SetNLives(m_NLives - decrement);
	}

	void SetNLives(int nLives)
	{
		m_NLives = nLives;
		EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestScore = BestScore, eScore = m_Score, eNLives = m_NLives, eNEnemiesLeftBeforeVictory = m_NEnemiesLeftBeforeVictory });
	}
	#endregion

	#region Score
	private int m_Score;
	public int Score {
		get { return m_Score; }
		set
		{
			m_Score = value;
			BestScore = Mathf.Max(BestScore, value);
		}
	}

	public int BestScore
	{
		get
		{
			return PlayerPrefs.GetInt("BEST_SCORE", 0);
		}
		set
		{
			PlayerPrefs.SetInt("BEST_SCORE", value);
		}
	}

	void IncrementScore(int increment)
	{
		SetScore(m_Score + increment);
	}

	void SetScore(int score)
	{
		Score = score;
		EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestScore = BestScore,eScore =m_Score,eNLives= m_NLives,eNEnemiesLeftBeforeVictory = m_NEnemiesLeftBeforeVictory });
	}
	#endregion

	[Header("GameManager")]

	#region Enemies to be destroyed
	[Header("Victory condition")]
	// Victory Condition : a certain number of enemies must be destroyed
	[SerializeField] private int m_NEnemiesToDestroyForVictory;
	private int m_NEnemiesLeftBeforeVictory;
	void DecrementNEnemiesLeftBeforeVictory(int decrement)
	{
		SetNEnemiesLeftBeforeVictory(m_NEnemiesLeftBeforeVictory - decrement);
	}
	void SetNEnemiesLeftBeforeVictory(int nEnemies)
	{
		m_NEnemiesLeftBeforeVictory = nEnemies;
		EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestScore = BestScore, eScore = m_Score, eNLives = m_NLives, eNEnemiesLeftBeforeVictory = m_NEnemiesLeftBeforeVictory });
	}
	#endregion

	#region Players
	public PlayerController[] PlayerControllers
	{
		get
		{
			return GameObject.FindObjectsOfType<PlayerController>();
		}
	}
	public Transform[] PlayerTransforms
	{
		get
		{
			return GameObject.FindObjectsOfType<PlayerController>().Select(item=>item.transform).ToArray();
		}
	}
	#endregion
	
	#region Level
	
	public Level Level
	{
		get { return GameObject.FindObjectsOfType<Level>()[0]; }
	}
	#endregion
	
	#region Camera
	public Camera Camera
	{
		get { return GameObject.FindObjectsOfType<Camera>()[0]; }
	}
	#endregion

	#region Events' subscription
	public override void SubscribeEvents()
	{
		base.SubscribeEvents();

		//MainMenuManager
		EventManager.Instance.AddListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
		EventManager.Instance.AddListener<NextLevelButtonClickedEvent>(NextLevelButtonClicked);
		EventManager.Instance.AddListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
		EventManager.Instance.AddListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
		EventManager.Instance.AddListener<CreditsButtonClickedEvent>(CreditsButtonClicked);
		
		//Game Type Choice
		EventManager.Instance.AddListener<LocalButtonClickedEvent>(LocalButtonClicked);
		EventManager.Instance.AddListener<MultiplayerButtonClickedEvent>(MultiplayerButtonClicked);
		
		//Player Type Choice
		EventManager.Instance.AddListener<VsHumanButtonClickedEvent>(VsHumanButtonClicked);
		EventManager.Instance.AddListener<VsCpuButtonClickedEvent>(VsCpuButtonClicked);
		
		//Number of player selection
		EventManager.Instance.AddListener<OnePlayerButtonClickedEvent>(OnePlayerButtonClicked);
		EventManager.Instance.AddListener<TwoPlayerButtonClickedEvent>(TwoPlayerButtonClicked);
		EventManager.Instance.AddListener<ThreePlayerButtonClickedEvent>(ThreePlayerButtonClicked);
		EventManager.Instance.AddListener<FourPlayerButtonClickedEvent>(FourPlayerButtonClicked);
		
		//Level Select
		EventManager.Instance.AddListener<LevelOneButtonClickedEvent>(LevelOneButtonClicked);
		EventManager.Instance.AddListener<LevelTwoButtonClickedEvent>(LevelTwoButtonClicked);
		EventManager.Instance.AddListener<LevelThreeButtonClickedEvent>(LevelThreeButtonClicked);

		//Enemy
		EventManager.Instance.AddListener<EnemyHasBeenDestroyedEvent>(EnemyHasBeenDestroyed);

		//Bomb
		EventManager.Instance.AddListener<BombIsDestroyingEvent>(BombIsDestroying);

		//Score Item
		EventManager.Instance.AddListener<ScoreItemEvent>(ScoreHasBeenGained);

		//Level
		EventManager.Instance.AddListener<LevelHasBeenInstantiatedEvent>(LevelHasBeenInstantiated);
		EventManager.Instance.AddListener<AllEnemiesOfLevelHaveBeenDestroyedEvent>(AllEnemiesOfLevelHaveBeenDestroyed);
		EventManager.Instance.AddListener<AllBombsHaveBeenDestroyedEvent>(AllBombsHaveBeenDestroyed);

		//Player
		EventManager.Instance.AddListener<PlayerHasBeenHitEvent>(PlayerHasBeenHit);
	}

	public override void UnsubscribeEvents()
	{
		base.UnsubscribeEvents();

		//MainMenuManager
		EventManager.Instance.RemoveListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
		EventManager.Instance.RemoveListener<NextLevelButtonClickedEvent>(NextLevelButtonClicked);
		EventManager.Instance.RemoveListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
		EventManager.Instance.RemoveListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
		
		//Game Type Choice
		EventManager.Instance.RemoveListener<LocalButtonClickedEvent>(LocalButtonClicked);
		EventManager.Instance.RemoveListener<MultiplayerButtonClickedEvent>(MultiplayerButtonClicked);
		
		//Player Type Choice
		EventManager.Instance.RemoveListener<VsHumanButtonClickedEvent>(VsHumanButtonClicked);
		EventManager.Instance.RemoveListener<VsCpuButtonClickedEvent>(VsCpuButtonClicked);
		
		//Number of player selection
		EventManager.Instance.RemoveListener<OnePlayerButtonClickedEvent>(OnePlayerButtonClicked);
		EventManager.Instance.RemoveListener<TwoPlayerButtonClickedEvent>(TwoPlayerButtonClicked);
		EventManager.Instance.RemoveListener<ThreePlayerButtonClickedEvent>(ThreePlayerButtonClicked);
		EventManager.Instance.RemoveListener<FourPlayerButtonClickedEvent>(FourPlayerButtonClicked);
		
		//Level Select
		EventManager.Instance.RemoveListener<LevelOneButtonClickedEvent>(LevelOneButtonClicked);
		EventManager.Instance.RemoveListener<LevelTwoButtonClickedEvent>(LevelTwoButtonClicked);
		EventManager.Instance.RemoveListener<LevelThreeButtonClickedEvent>(LevelThreeButtonClicked);

		//Enemy
		EventManager.Instance.RemoveListener<EnemyHasBeenDestroyedEvent>(EnemyHasBeenDestroyed);

		//Bomb
		EventManager.Instance.RemoveListener<BombIsDestroyingEvent>(BombIsDestroying);

		//Score Item
		EventManager.Instance.RemoveListener<ScoreItemEvent>(ScoreHasBeenGained);

		//Level
		EventManager.Instance.RemoveListener<LevelHasBeenInstantiatedEvent>(LevelHasBeenInstantiated);
		EventManager.Instance.RemoveListener<AllEnemiesOfLevelHaveBeenDestroyedEvent>(AllEnemiesOfLevelHaveBeenDestroyed);
		EventManager.Instance.RemoveListener<AllBombsHaveBeenDestroyedEvent>(AllBombsHaveBeenDestroyed);

		//Player
		EventManager.Instance.RemoveListener<PlayerHasBeenHitEvent>(PlayerHasBeenHit);

	}
	#endregion

	#region Manager implementation
	protected override IEnumerator InitCoroutine()
	{
		Menu();
		EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestScore = BestScore, eScore = 0, eNLives = 0, eNEnemiesLeftBeforeVictory = 0});
		yield break;
	}
	#endregion

	#region Game flow & Gameplay
	//Game initialization
	void InitNewGame(int levelNumber)
	{
		SetScore(0);
		SetNLives(m_NStartLives);
		SetNEnemiesLeftBeforeVictory(m_NEnemiesToDestroyForVictory);

		m_GameState = GameState.gameNextLevel; // le game state sera set à play après que le level est instantié
		EventManager.Instance.Raise(new GoToLevelEvent(){eLevelIndex = --levelNumber});
	}
	#endregion

	#region Callbacks to events issued by LevelManager
	private void LevelHasBeenInstantiated(LevelHasBeenInstantiatedEvent e)
	{
		SetTimeScale(1);
		m_GameState = GameState.gamePlay;
	}
	#endregion

	#region Callbacks to events issued by Player
	private void PlayerHasBeenHit(PlayerHasBeenHitEvent e)
	{
		DecrementNLives(1);

		if (m_NLives == 0)
		{

			Over();
		}
	}
	#endregion

	#region Callbacks to events issued by Bomb items
	private void BombIsDestroying(BombIsDestroyingEvent e)
	{
		/*DecrementNEnemiesLeftBeforeVictory(1);

		if (m_NEnemiesLeftBeforeVictory == 0)
		{
			Victory();
		}*/
	}

	private void AllBombsHaveBeenDestroyed(AllBombsHaveBeenDestroyedEvent e)
	{
		Debug.Log("ALL BOMBS OF THE LEVEL HAVE BEEN COLLECTED");
		if (IsPlaying)
		{
			m_GameState = GameState.gameNextLevel;
			SetTimeScale(0);
			EventManager.Instance.Raise(new AskToGoToNextLevelEvent());
		}
	}
	#endregion

	#region Callbacks to events issued by Score items
	private void ScoreHasBeenGained(ScoreItemEvent e)
	{
		IncrementScore(e.eScore.Score);
	}
	#endregion

	#region Callbacks to events issued by Enemy
	private void EnemyHasBeenDestroyed(EnemyHasBeenDestroyedEvent e)
	{
	}
	#endregion

	#region Callbacks to events issued by Level
	private void AllEnemiesOfLevelHaveBeenDestroyed(AllEnemiesOfLevelHaveBeenDestroyedEvent e)
	{
	}
	#endregion

	#region Callbacks to Events issued by MenuManager
	private void MainMenuButtonClicked(MainMenuButtonClickedEvent e)
	{
		Menu();
	}

	private void NextLevelButtonClicked(NextLevelButtonClickedEvent e)
	{
		EventManager.Instance.Raise(new GoToNextLevelEvent());
	}

	private void ResumeButtonClicked(ResumeButtonClickedEvent e)
	{
		Resume();
	}

	private void EscapeButtonClicked(EscapeButtonClickedEvent e)
	{
		if(IsPlaying)
			Pause();
	}
	
	private void LocalButtonClicked(LocalButtonClickedEvent e)
	{
	}
	
	private void MultiplayerButtonClicked(MultiplayerButtonClickedEvent e)
	{
	}
	
	private void VsHumanButtonClicked(VsHumanButtonClickedEvent e)
	{
	}
	
	private void VsCpuButtonClicked(VsCpuButtonClickedEvent e)
	{
	}
	
	private void OnePlayerButtonClicked(OnePlayerButtonClickedEvent e)
	{
	}
	
	private void TwoPlayerButtonClicked(TwoPlayerButtonClickedEvent e)
	{
	}
	
	private void ThreePlayerButtonClicked(ThreePlayerButtonClickedEvent e)
	{
	}
	
	private void FourPlayerButtonClicked(FourPlayerButtonClickedEvent e)
	{
	}
	
	private void LevelOneButtonClicked(LevelOneButtonClickedEvent e)
	{
		Play(1);
	}
	
	private void LevelTwoButtonClicked(LevelTwoButtonClickedEvent e)
	{
		Play(2);
	}
	
	private void LevelThreeButtonClicked(LevelThreeButtonClickedEvent e)
	{
		Play(3);
	}

	private void CreditsButtonClicked(CreditsButtonClickedEvent e)
	{
		Credits();
	}
	#endregion

	#region GameState methods
	private void Menu()
	{
		SetTimeScale(0);
		m_GameState = GameState.gameMenu;
		MusicLoopsManager.Instance.PlayMusic(Constants.MENU_MUSIC);
		EventManager.Instance.Raise(new GameMenuEvent());
	}

	private void Play(int levelNumber)
	{
		m_GameState = GameState.gamePlay;
		MusicLoopsManager.Instance.PlayMusic(Constants.GAMEPLAY_MUSIC);
		EventManager.Instance.Raise(new GamePlayEvent());
		InitNewGame(levelNumber);
	}

	private void Pause()
	{
		SetTimeScale(0);
		m_GameState = GameState.gamePause;
		EventManager.Instance.Raise(new GamePauseEvent());
	}

	private void Resume()
	{
		SetTimeScale(1);
		m_GameState = GameState.gamePlay;
		EventManager.Instance.Raise(new GameResumeEvent());
	}

	private void Over()
	{
		SetTimeScale(0);
		m_GameState = GameState.gameOver;
		SfxManager.Instance.PlaySfx(Constants.GAMEOVER_SFX);
		EventManager.Instance.Raise(new GameOverEvent());
	}

	private void Victory()
	{
		SetTimeScale(0);
		m_GameState = GameState.gameVictory;
		SfxManager.Instance.PlaySfx(Constants.VICTORY_SFX);
		EventManager.Instance.Raise(new GameVictoryEvent());
	}
	
	private void Credits()
	{
		SetTimeScale(1);
		m_GameState = GameState.gameCredits;
		EventManager.Instance.Raise(new GameCreditsEvent());
	}
	#endregion
}
