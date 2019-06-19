using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class MenuManager : Manager<MenuManager>
{
    [Header("MenuManager")]

    #region Panels

    [Header("Panels")]
    [SerializeField]
    GameObject panelMainMenu;

    [SerializeField] GameObject panelGameTypeSelection;
    [SerializeField] GameObject panelPlayerTypeSelection;
    [SerializeField] GameObject panelPlayerNumberSelection;
    [SerializeField] GameObject panelLevelSelection;
    [SerializeField] GameObject panelInGameMenu;
    [SerializeField] GameObject panelNextLevel;
    [SerializeField] GameObject panelVictory;
    [SerializeField] GameObject panelGameOver;
    [SerializeField] GameObject panelCredits;

    List<GameObject> allPanels;

    #endregion

    #region Events' subscription

    public override void SubscribeEvents()
    {
        base.SubscribeEvents();

        //GameManager
        EventManager.Instance.AddListener<AskToGoToNextLevelEvent>(AskToGoToNextLevel);
        EventManager.Instance.AddListener<GoToNextLevelEvent>(GoToNextLevel);
        
        //Sub-menus
        EventManager.Instance.AddListener<PlayButtonClickedEvent>(PlayButtonClicked);
        
        EventManager.Instance.AddListener<LocalButtonClickedEvent>(LocalButtonClicked);
        EventManager.Instance.AddListener<MultiplayerButtonClickedEvent>(MultiplayerButtonClicked);
        
        EventManager.Instance.AddListener<VsHumanButtonClickedEvent>(VsHumanButtonClicked);
        EventManager.Instance.AddListener<VsCpuButtonClickedEvent>(VsCpuButtonClicked);
        
        EventManager.Instance.AddListener<OnePlayerButtonClickedEvent>(OnePlayerButtonClicked);
        EventManager.Instance.AddListener<TwoPlayerButtonClickedEvent>(TwoPlayerButtonClicked);
        EventManager.Instance.AddListener<ThreePlayerButtonClickedEvent>(ThreePlayerButtonClicked);
        EventManager.Instance.AddListener<FourPlayerButtonClickedEvent>(FourPlayerButtonClicked);
    }

    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();

        //GameManager
        EventManager.Instance.RemoveListener<AskToGoToNextLevelEvent>(AskToGoToNextLevel);
        EventManager.Instance.RemoveListener<GoToNextLevelEvent>(GoToNextLevel);
        
        //Sub-menus
        EventManager.Instance.RemoveListener<PlayButtonClickedEvent>(PlayButtonClicked);
        
        EventManager.Instance.RemoveListener<LocalButtonClickedEvent>(LocalButtonClicked);
        EventManager.Instance.RemoveListener<MultiplayerButtonClickedEvent>(MultiplayerButtonClicked);
        
        EventManager.Instance.RemoveListener<VsHumanButtonClickedEvent>(VsHumanButtonClicked);
        EventManager.Instance.RemoveListener<VsCpuButtonClickedEvent>(VsCpuButtonClicked);
        
        EventManager.Instance.RemoveListener<OnePlayerButtonClickedEvent>(OnePlayerButtonClicked);
        EventManager.Instance.RemoveListener<TwoPlayerButtonClickedEvent>(TwoPlayerButtonClicked);
        EventManager.Instance.RemoveListener<ThreePlayerButtonClickedEvent>(ThreePlayerButtonClicked);
        EventManager.Instance.RemoveListener<FourPlayerButtonClickedEvent>(FourPlayerButtonClicked);
    }

    #endregion

    #region Manager implementation

    protected override IEnumerator InitCoroutine()
    {
        yield break;
    }

    #endregion

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

    #region Monobehaviour lifecycle

    protected override void Awake()
    {
        base.Awake();
        RegisterPanels();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            EscapeButtonHasBeenClicked();
        }
    }

    #endregion

    #region Panel Methods

    void RegisterPanels()
    {
        allPanels = new List<GameObject>();
        allPanels.Add(panelMainMenu);
        allPanels.Add(panelGameTypeSelection);
        allPanels.Add(panelPlayerTypeSelection);
        allPanels.Add(panelPlayerNumberSelection);
        allPanels.Add(panelLevelSelection);
        allPanels.Add(panelInGameMenu);
        allPanels.Add(panelNextLevel);
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

    #region UI OnClick Events

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

    public void LocalButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new LocalButtonClickedEvent());
    }

    public void MultiplayerButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new MultiplayerButtonClickedEvent());
    }

    public void VsHumanButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new VsHumanButtonClickedEvent());
    }

    public void VsCpuButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new VsCpuButtonClickedEvent());
    }

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

    #endregion

    #region Callbacks to GameManager events

    private void AskToGoToNextLevel(AskToGoToNextLevelEvent e)
    {
        OpenPanel(panelNextLevel);
    }

    private void GoToNextLevel(GoToNextLevelEvent e)
    {
        OpenPanel(null);
    }

    private void PlayButtonClicked(PlayButtonClickedEvent e)
    {
        OpenPanel(panelGameTypeSelection);
    }

    private void LocalButtonClicked(LocalButtonClickedEvent e)
    {
        OpenPanel(panelPlayerTypeSelection);
    }

    private void MultiplayerButtonClicked(MultiplayerButtonClickedEvent e)
    {
        OpenPanel(panelPlayerNumberSelection);
    }

    private void VsHumanButtonClicked(VsHumanButtonClickedEvent e)
    {
        OpenPanel(panelPlayerNumberSelection);
    }

    private void VsCpuButtonClicked(VsCpuButtonClickedEvent e)
    {
        OpenPanel(panelPlayerNumberSelection);
    }

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
        OpenPanel(panelVictory);
    }

    protected override void GameCredits(GameCreditsEvent e)
    {
        OpenPanel(panelCredits);
        StartCoroutine(GoBackToMainMenuCoroutine(
            panelCredits.GetComponent<Animation>().clip.length));
    }

    #endregion
}