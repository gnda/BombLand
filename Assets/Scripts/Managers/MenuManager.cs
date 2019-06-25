using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using UnityEngine.UI;

public class MenuManager : Manager<MenuManager>
{
    #region Panels
    [Header("Panels")]
    [SerializeField] GameObject panelMainMenu;
    [SerializeField] GameObject panelGameModeSelection;
    [SerializeField] GameObject panelMultiplayerRoomChoice;
    [SerializeField] GameObject panelMultiplayerLobby;
    [SerializeField] GameObject panelPlayerTypeSelection;
    [SerializeField] GameObject panelPlayerNumberSelection;
    [SerializeField] GameObject panelLevelSelection;
    [SerializeField] GameObject panelInGameMenu;
    [SerializeField] GameObject panelVictory;
    [SerializeField] GameObject panelGameOver;
    [SerializeField] GameObject panelCredits;
    
    [Header("Fields")]
    [SerializeField] Text txtVictoryPlayer;

    [Header("Settings")]
    [SerializeField] float creditsDuration;

    List<GameObject> allPanels;
    #endregion
    
    #region Modals
    [Header("Modals")]
    [SerializeField] GameObject modalPseudo;
    
    List<GameObject> allModals;
    #endregion

    #region Events' subscription
    public override void SubscribeEvents()
    {
        base.SubscribeEvents();
        
        //GameManager
        EventManager.Instance.AddListener<GoToNextLevelEvent>(GoToNextLevel);
        
        //Sub-menus
        EventManager.Instance.AddListener<PlayButtonClickedEvent>(PlayButtonClicked);

        //Game Type Choice
        EventManager.Instance.AddListener<LocalButtonClickedEvent>(LocalButtonClicked);
        EventManager.Instance.AddListener<MultiplayerButtonClickedEvent>(MultiplayerButtonClicked);
        
        //Multiplayer Type Choice
        EventManager.Instance.AddListener<CreateARoomButtonClickedEvent>(CreateARoomButtonClicked);
        EventManager.Instance.AddListener<JoinRoomButtonClickedEvent>(JoinRoomButtonClicked);
        
        //Multiplayer Lobby
        EventManager.Instance.AddListener<SendMessageButtonClickedEvent>(SendMessageButtonClicked);
        EventManager.Instance.AddListener<StartMultiplayerGameButtonClickedEvent>(StartMultiplayerGameButtonClicked);
        
        //Player Type Choice
        EventManager.Instance.AddListener<VsHumanButtonClickedEvent>(VsHumanButtonClicked);
        EventManager.Instance.AddListener<VsCpuButtonClickedEvent>(VsCpuButtonClicked);
        
        //Number of player selection
        EventManager.Instance.AddListener<OnePlayerButtonClickedEvent>(OnePlayerButtonClicked);
        EventManager.Instance.AddListener<TwoPlayerButtonClickedEvent>(TwoPlayerButtonClicked);
        EventManager.Instance.AddListener<ThreePlayerButtonClickedEvent>(ThreePlayerButtonClicked);
        EventManager.Instance.AddListener<FourPlayerButtonClickedEvent>(FourPlayerButtonClicked);
        
        //Modals
        EventManager.Instance.AddListener<PseudoOkButtonClickedEvent>(PseudoOkButtonClicked);
    }

    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
        
        //GameManager
        EventManager.Instance.RemoveListener<GoToNextLevelEvent>(GoToNextLevel);
        
        //Sub-menus
        EventManager.Instance.RemoveListener<PlayButtonClickedEvent>(PlayButtonClicked);
        
        //Game Type Choice
        EventManager.Instance.RemoveListener<LocalButtonClickedEvent>(LocalButtonClicked);
        EventManager.Instance.RemoveListener<MultiplayerButtonClickedEvent>(MultiplayerButtonClicked);
        
        //Multiplayer Type Choice
        EventManager.Instance.RemoveListener<CreateARoomButtonClickedEvent>(CreateARoomButtonClicked);
        EventManager.Instance.RemoveListener<JoinRoomButtonClickedEvent>(JoinRoomButtonClicked);
        
        //Multiplayer Lobby
        EventManager.Instance.RemoveListener<SendMessageButtonClickedEvent>(SendMessageButtonClicked);
        EventManager.Instance.RemoveListener<StartMultiplayerGameButtonClickedEvent>(StartMultiplayerGameButtonClicked);
        
        //Player Type Choice
        EventManager.Instance.RemoveListener<VsHumanButtonClickedEvent>(VsHumanButtonClicked);
        EventManager.Instance.RemoveListener<VsCpuButtonClickedEvent>(VsCpuButtonClicked);
        
        //Number of player selection
        EventManager.Instance.RemoveListener<OnePlayerButtonClickedEvent>(OnePlayerButtonClicked);
        EventManager.Instance.RemoveListener<TwoPlayerButtonClickedEvent>(TwoPlayerButtonClicked);
        EventManager.Instance.RemoveListener<ThreePlayerButtonClickedEvent>(ThreePlayerButtonClicked);
        EventManager.Instance.RemoveListener<FourPlayerButtonClickedEvent>(FourPlayerButtonClicked);
        
        //Modals
        EventManager.Instance.RemoveListener<PseudoOkButtonClickedEvent>(PseudoOkButtonClicked);
    }
    #endregion

    #region Manager implementation
    protected override IEnumerator InitCoroutine()
    {
        yield break;
    }
    #endregion

    #region MonoBehaviour lifecycle

    protected override void Awake()
    {
        base.Awake();
        RegisterPanels();
        RegisterModals();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            EscapeButtonHasBeenClicked();
        }
    }

    #endregion

    #region Additional coroutines
    private IEnumerator GoBackToMainMenuCoroutine(float time)
    {
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        EventManager.Instance.Raise(new GameMenuEvent());
    }
    #endregion
    
    #region Panel Methods
    void RegisterPanels()
    {
        allPanels = new List<GameObject>();
        allPanels.Add(panelMainMenu);
        allPanels.Add(panelGameModeSelection);
        allPanels.Add(panelMultiplayerRoomChoice);
        allPanels.Add(panelMultiplayerLobby);
        allPanels.Add(panelPlayerTypeSelection);
        allPanels.Add(panelPlayerNumberSelection);
        allPanels.Add(panelLevelSelection);
        allPanels.Add(panelInGameMenu);
        allPanels.Add(panelVictory);
        allPanels.Add(panelGameOver);
        allPanels.Add(panelCredits);
    }

    void OpenPanel(GameObject panel)
    {
        foreach (var item in allPanels)
            if (item)
                item.SetActive(item == panel);
    }
    #endregion
    
    #region Modal Methods
    void RegisterModals()
    {
        allModals = new List<GameObject>();
        allModals.Add(modalPseudo);
    }

    void OpenModal(GameObject modal)
    {
        foreach (var item in allModals)
            if (item)
                item.SetActive(item == modal);
    }
    #endregion

    
    // UI OnClick Events
    

    #region General UI OnClick Events
    public void EscapeButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new EscapeButtonClickedEvent());
    }

    public void MainMenuButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new MainMenuButtonClickedEvent());
    }

    public void PlayButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new PlayButtonClickedEvent());
    }
    
    public void ResumeButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new ResumeButtonClickedEvent());
    }

    public void NextLevelButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new NextLevelButtonClickedEvent());
    }

    public void CreditsButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new CreditsButtonClickedEvent());
    }

    public void ExitButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new ExitButtonClickedEvent());
    }
    #endregion
    
    #region Local UI OnClick Events
    public void LocalButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new LocalButtonClickedEvent());
    }
    
    public void VsHumanButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new VsHumanButtonClickedEvent());
    }

    public void VsCpuButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new VsCpuButtonClickedEvent());
    }
    #endregion

    #region Multiplayer UI OnClick Events
    public void MultiplayerButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new MultiplayerButtonClickedEvent());
    }
    
    public void CreateARoomButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new CreateARoomButtonClickedEvent());
    }
    
    public void PseudoOkButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new PseudoOkButtonClickedEvent());
    }
    
    public void JoinRoomButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new JoinRoomButtonClickedEvent());
    }
    
    public void SendMessageButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new SendMessageButtonClickedEvent());
    }
    
    public void StartMultiplayerGameButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new StartMultiplayerGameButtonClickedEvent());
    }
    #endregion

    #region Player Number UI OnClick Events
    public void OnePlayerButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new OnePlayerButtonClickedEvent());
    }

    public void TwoPlayerButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new TwoPlayerButtonClickedEvent());
    }

    public void ThreePlayerButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new ThreePlayerButtonClickedEvent());
    }

    public void FourPlayerButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new FourPlayerButtonClickedEvent());
    }
    #endregion
    
    #region Level Number UI OnClick Events
    public void LevelOneButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new LevelOneButtonClickedEvent());
    }

    public void LevelTwoButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new LevelTwoButtonClickedEvent());
    }

    public void LevelThreeButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new LevelThreeButtonClickedEvent());
    }
    #endregion
    
    
    // Callbacks to MenuManager UI events
    

    #region Callbacks to General UI events
    private void PlayButtonClicked(PlayButtonClickedEvent e)
    {
        OpenPanel(panelGameModeSelection);
    }
    #endregion
    
    #region Callbacks to Local UI events
    private void LocalButtonClicked(LocalButtonClickedEvent e)
    {
        OpenPanel(panelPlayerTypeSelection);
    }
    
    private void VsHumanButtonClicked(VsHumanButtonClickedEvent e)
    {
        OpenPanel(panelPlayerNumberSelection);
    }

    private void VsCpuButtonClicked(VsCpuButtonClickedEvent e)
    {
        OpenPanel(panelPlayerNumberSelection);
    }
    #endregion
    
    #region Callbacks to Multiplayer UI events
    private void MultiplayerButtonClicked(MultiplayerButtonClickedEvent e)
    {
        OpenPanel(panelMultiplayerRoomChoice);
    }
    
    private void CreateARoomButtonClicked(CreateARoomButtonClickedEvent e)
    {
        OpenPanel(panelMultiplayerLobby);
        OpenModal(modalPseudo);
    }
    
    private void PseudoOkButtonClicked(PseudoOkButtonClickedEvent e)
    {
        OpenModal(null);
    }
    
    private void JoinRoomButtonClicked(JoinRoomButtonClickedEvent e)
    {
        OpenPanel(panelMultiplayerLobby);
        OpenModal(modalPseudo);
    }
    
    private void SendMessageButtonClicked(SendMessageButtonClickedEvent e)
    {
    }
    
    private void StartMultiplayerGameButtonClicked(StartMultiplayerGameButtonClickedEvent e)
    {
        OpenPanel(null);
    }
    #endregion
    
    #region Callbacks to Player Number UI events
    private void OnePlayerButtonClicked(OnePlayerButtonClickedEvent e)
    {
        OpenPanel(panelLevelSelection);
    }

    private void TwoPlayerButtonClicked(TwoPlayerButtonClickedEvent e)
    {
        OpenPanel(panelLevelSelection);
    }

    private void ThreePlayerButtonClicked(ThreePlayerButtonClickedEvent e)
    {
        OpenPanel(panelLevelSelection);
    }

    private void FourPlayerButtonClicked(FourPlayerButtonClickedEvent e)
    {
        OpenPanel(panelLevelSelection);
    }
    #endregion
    
    
    // Callbacks to GameManager events
    
    
    #region Callbacks to GameManager events
    private void GoToNextLevel(GoToNextLevelEvent e)
    {
        OpenPanel(null);
    }

    protected override void GameMenu(GameMenuEvent e)
    {
        OpenPanel(panelMainMenu);
    }

    protected override void GamePlay(GamePlayEvent e)
    {
        OpenPanel(null);
    }

    protected override void GamePause(GamePauseEvent e)
    {
        OpenPanel(panelInGameMenu);
    }

    protected override void GameResume(GameResumeEvent e)
    {
        OpenPanel(null);
    }

    protected override void GameOver(GameOverEvent e)
    {
        OpenPanel(panelGameOver);
    }

    protected override void GameVictory(GameVictoryEvent e)
    {
        txtVictoryPlayer.text = e.ePlayer.PlayerNumber.ToString();
        OpenPanel(panelVictory);
    }

    protected override void GameCredits(GameCreditsEvent e)
    {
        OpenPanel(panelCredits);
        StartCoroutine(GoBackToMainMenuCoroutine(creditsDuration));
    }
    #endregion
}