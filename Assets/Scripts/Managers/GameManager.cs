using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using System.Linq;
using DefaultNamespace;

#region Game states, mode and types
public enum GameState
{
    gameMenu,
    gamePlay,
    gameNextLevel,
    gamePause,
    gameOver,
    gameVictory,
    gameCredits
}

public enum GameMode
{
    local,
    multiplayer
}

public enum PlayerType
{
    human,
    computer
}
#endregion

public class GameManager : Manager<GameManager>
{
    #region Time

    void SetTimeScale(float newTimeScale)
    {
        Time.timeScale = newTimeScale;
    }

    private float timer = 0;

    public float Timer
    {
        get => timer;
        set => timer = value;
    }

    private void FixedUpdate()
    {
        if (IsPlaying && timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
                EventManager.Instance.Raise(new TimeIsUpEvent());
        }
    }

    #endregion

    #region Game State, Mode and Types
    private GameState gameState;
    private GameMode gameMode;
    private PlayerType playerType;

    public GameState GameState { get { return gameState; }} 
    public GameMode GameMode { get { return gameMode; }} 
    public PlayerType PlayerType { get { return playerType; }}
    public int NumberOfPlayer { get; set; } = 0;

    public bool IsPlaying
    {
        get { return gameState == GameState.gamePlay; }
    }
    #endregion

    #region Score

    public int BestScore
    {
        get { return PlayerPrefs.GetInt("BEST_SCORE", 0); }
        set { PlayerPrefs.SetInt("BEST_SCORE", value); }
    }

    void IncScore(Player player, int score)
    {
        SetScore(player, player.GainedScore + score);
    }

    void SetScore(Player player, int score)
    {
        player.GainedScore = score;

        EventManager.Instance.Raise(new GameStatisticsChangedEvent()
        {eBestScore = BestScore, ePlayerNumber = player.PlayerNumber, 
            eScore = score, eNMonstersLeft = nMonstersLeft});
    }

    #endregion

    #region Monsters to be destroyed
    private int nMonstersLeft;

    void DecrementNMonstersLeft(int decrement)
    {
        SetNMonstersLeft(nMonstersLeft - decrement);
    }

    void SetNMonstersLeft(int nMonsters)
    {
        nMonstersLeft = nMonsters;
        
        EventManager.Instance.Raise(new GameStatisticsChangedEvent()
        { eNMonstersLeft = nMonstersLeft });
    }
    #endregion

    #region Elements Instances

    public Transform[] PlayerTransforms
    {
        get { return Players.Select(item => item.transform).ToArray(); }
    }
    
    public List<Player> Players
    {
        get { return GameObject.FindObjectsOfType<Player>().ToList(); }
    }
    
    public List<Monster> Monsters
    {
        get { return GameObject.FindObjectsOfType<Monster>().ToList(); }
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
        
        //Exit
        EventManager.Instance.AddListener<ExitButtonClickedEvent>(ExitButtonClicked);

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

        //Element
        EventManager.Instance.AddListener<ElementIsBeingDestroyedEvent>(ElementIsBeingDestroyed);

        //Score Item
        EventManager.Instance.AddListener<ScoreItemEvent>(ScoreHasBeenGained);

        //Level
        EventManager.Instance.AddListener<LevelHasBeenInstantiatedEvent>(LevelHasBeenInstantiated);
        EventManager.Instance.AddListener<TimeIsUpEvent>(TimeIsUp);
    }

    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();

        //MainMenuManager
        EventManager.Instance.RemoveListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
        EventManager.Instance.RemoveListener<NextLevelButtonClickedEvent>(NextLevelButtonClicked);
        EventManager.Instance.RemoveListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
        EventManager.Instance.RemoveListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
        
        //Exit
        EventManager.Instance.AddListener<ExitButtonClickedEvent>(ExitButtonClicked);

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

        //Element
        EventManager.Instance.RemoveListener<ElementIsBeingDestroyedEvent>(ElementIsBeingDestroyed);

        //Score Item
        EventManager.Instance.RemoveListener<ScoreItemEvent>(ScoreHasBeenGained);

        //Level
        EventManager.Instance.RemoveListener<LevelHasBeenInstantiatedEvent>(LevelHasBeenInstantiated);
        EventManager.Instance.RemoveListener<TimeIsUpEvent>(TimeIsUp);
    }
    
    #endregion

    #region Manager implementation

    protected override IEnumerator InitCoroutine()
    {
        Menu();

        yield break;
    }

    #endregion
    
    
    //Callbacks to events
    
    
    #region Callbacks to events issued by GameManager
    
    private void TimeIsUp(TimeIsUpEvent e)
    {
        StartCoroutine(CheckVictory());
    }
    
    #endregion

    #region Callbacks to events issued by LevelManager

    private void LevelHasBeenInstantiated(LevelHasBeenInstantiatedEvent e)
    {
        timer = e.eLevel.LevelDuration;
        nMonstersLeft = Monsters.Count;

        EventManager.Instance.Raise(new GameHasStartedEvent());
        EventManager.Instance.Raise(new GameStatisticsChangedEvent() 
            { eBestScore = BestScore, ePlayerNumber = -1, 
                eNMonstersLeft = Monsters.Count});
        
        SetTimeScale(1);
        gameState = GameState.gamePlay;
    }

    #endregion

    #region Callbacks to events issued by Score items

    private void ScoreHasBeenGained(ScoreItemEvent e)
    {
        
        IScore elementWithScore = e.eElement.GetComponent<IScore>();
        IMoveable moveableElement = e.eElement.GetComponent<IMoveable>();

        if ((moveableElement != null && elementWithScore != null) &&
            !moveableElement.IsDestroyed)
            IncScore(e.ePlayer, elementWithScore.Score);
    }

    private IEnumerator CheckVictory()
    {
        if (Players.Count > 0)
        {
            Players.Sort((a,b) => a.GainedScore - b.GainedScore);

            if (Players[0].GainedScore != 0)
            {
                // Delay to Check if both last player and last enemy are dead
                yield return new WaitForSeconds(0.1f);

                if (Players.Count > 0 && Monsters.Count == 0) 
                    Victory(Players[0]);
            } else Over();
        }else Over();
    }

    #endregion

    #region Callbacks to events issued by Element

    private void ElementIsBeingDestroyed(ElementIsBeingDestroyedEvent e)
    {
        IMoveable element = e.eElement.GetComponent<IMoveable>();
        
        if (element != null && !element.IsDestroyed)
        {
            if (e.eElement.CompareTag("Player"))
            {
                NumberOfPlayer--;

                if (NumberOfPlayer == 0) Over();
            } 
            else if (e.eElement.CompareTag("Enemy")) DecrementNMonstersLeft(1);
            
            if (nMonstersLeft == 0) StartCoroutine(CheckVictory());
        }
    }

    #endregion

    #region Callbacks to events issued by Level

    #endregion

    
    // Callbacks to MenuManager UI events
    
    
    #region Callbacks to General UI Events
    private void EscapeButtonClicked(EscapeButtonClickedEvent e)
    {
        if (IsPlaying) Pause();
    }
    
    private void MainMenuButtonClicked(MainMenuButtonClickedEvent e)
    {
        EventManager.Instance.Raise(new GameStatisticsChangedEvent() 
            {eBestScore = BestScore, ePlayerNumber = -1});
        Menu();
    }

    private void NextLevelButtonClicked(NextLevelButtonClickedEvent e)
    {
        EventManager.Instance.Raise(new GameStatisticsChangedEvent() 
            {eBestScore = BestScore, ePlayerNumber = -1});
        EventManager.Instance.Raise(new GoToNextLevelEvent()
            {eLevelIndex = -1});
    }

    private void ResumeButtonClicked(ResumeButtonClickedEvent e)
    {
        Resume();
    }

    private void CreditsButtonClicked(CreditsButtonClickedEvent e)
    {
        Credits();
    }

    private void ExitButtonClicked(ExitButtonClickedEvent e)
    {
        Exit();
    }
    #endregion

    #region Callbacks to Local UI Events
    private void LocalButtonClicked(LocalButtonClickedEvent e)
    {
        gameMode = GameMode.local;
    }
    
    void VsHumanButtonClicked(VsHumanButtonClickedEvent e)
    {
        playerType = PlayerType.human;
    }

    void VsCpuButtonClicked(VsCpuButtonClickedEvent e)
    {
        playerType = PlayerType.computer;
    }
    #endregion

    #region Callbacks to Multiplayer UI Events
    private void MultiplayerButtonClicked(MultiplayerButtonClickedEvent e)
    {
        gameMode = GameMode.multiplayer;
    }
    #endregion

    #region Callbacks to Player Number UI Events
    void OnePlayerButtonClicked(OnePlayerButtonClickedEvent e)
    {
        NumberOfPlayer = 1;
    }

    void TwoPlayerButtonClicked(TwoPlayerButtonClickedEvent e)
    {
        NumberOfPlayer = 2;
    }

    void ThreePlayerButtonClicked(ThreePlayerButtonClickedEvent e)
    {
        NumberOfPlayer = 3;
    }

    void FourPlayerButtonClicked(FourPlayerButtonClickedEvent e)
    {
        NumberOfPlayer = 4;
    }
    #endregion

    #region Callbacks to Level UI Events
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
    #endregion
    
    
    //Methods linked to callbacks
    

    #region Game flow methods
    void InitNewGame(int levelNumber)
    {
        gameState = GameState.gameNextLevel;
        EventManager.Instance.Raise(new GoToNextLevelEvent() 
            {eLevelIndex = --levelNumber});
    }
    
    private void Menu()
    {
        SetTimeScale(1);
        gameState = GameState.gameMenu;
        MusicLoopsManager.Instance.PlayMusic(Constants.MENU_MUSIC);
        EventManager.Instance.Raise(new GameMenuEvent());
    }

    private void Play(int levelNumber)
    {
        EventManager.Instance.Raise(new GamePlayEvent());
        MusicLoopsManager.Instance.PlayMusic(levelNumber);
        InitNewGame(levelNumber);
    }

    private void Pause()
    {
        SetTimeScale(0);
        gameState = GameState.gamePause;
        EventManager.Instance.Raise(new GamePauseEvent());
    }

    private void Resume()
    {
        SetTimeScale(1);
        gameState = GameState.gamePlay;
        EventManager.Instance.Raise(new GameResumeEvent());
    }

    private void Over()
    {
        SetTimeScale(0);
        gameState = GameState.gameOver;
        SfxManager.Instance.PlaySfx(Constants.GAMEOVER_SFX);
        EventManager.Instance.Raise(new GameOverEvent());
    }

    private void Victory(Player player)
    {
        if (player.GainedScore > BestScore)
            BestScore = player.GainedScore;
        
        SetTimeScale(0);
        gameState = GameState.gameVictory;
        SfxManager.Instance.PlaySfx(Constants.VICTORY_SFX);
        EventManager.Instance.Raise(new GameVictoryEvent() {ePlayer = player});
    }

    private void Credits()
    {
        SetTimeScale(1);
        gameState = GameState.gameCredits;
        MusicLoopsManager.Instance.PlayMusic(Constants.MENU_MUSIC);
        EventManager.Instance.Raise(new GameCreditsEvent());
    }

    private void Exit()
    {
        SetTimeScale(0);
        Application.Quit();
    }
    #endregion
}