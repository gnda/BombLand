using DefaultNamespace;
using UnityEngine;

#region GameManager Events

public class GameMenuEvent : SDD.Events.Event
{
}

public class GamePlayEvent : SDD.Events.Event
{
}

public class GamePauseEvent : SDD.Events.Event
{
}

public class GameResumeEvent : SDD.Events.Event
{
}

public class GameOverEvent : SDD.Events.Event
{
}

public class GameCreditsEvent : SDD.Events.Event
{
}

public class GameVictoryEvent : SDD.Events.Event
{
}

public class GameStatisticsChangedEvent : SDD.Events.Event
{
    public int eBestScore { get; set; }
    public int eScore { get; set; }
    public int eNLives { get; set; }
    public int eNEnemiesLeftBeforeVictory { get; set; }
}

#endregion

#region MenuManager Events

public class EscapeButtonClickedEvent : SDD.Events.Event
{
}

public class ResumeButtonClickedEvent : SDD.Events.Event
{
}

public class MainMenuButtonClickedEvent : SDD.Events.Event
{
}

public class PlayButtonClickedEvent : SDD.Events.Event
{
}

public class NextLevelButtonClickedEvent : SDD.Events.Event
{
}

public class LocalButtonClickedEvent : SDD.Events.Event
{
}

public class MultiplayerButtonClickedEvent : SDD.Events.Event
{
}

public class VsHumanButtonClickedEvent : SDD.Events.Event
{
}

public class VsCpuButtonClickedEvent : SDD.Events.Event
{
}

public class OnePlayerButtonClickedEvent : SDD.Events.Event
{
}

public class TwoPlayerButtonClickedEvent : SDD.Events.Event
{
}

public class ThreePlayerButtonClickedEvent : SDD.Events.Event
{
}

public class FourPlayerButtonClickedEvent : SDD.Events.Event
{
}

public class LevelOneButtonClickedEvent : SDD.Events.Event
{
}

public class LevelTwoButtonClickedEvent : SDD.Events.Event
{
}

public class LevelThreeButtonClickedEvent : SDD.Events.Event
{
}

public class CreditsButtonClickedEvent : SDD.Events.Event
{
}

#endregion

#region Enemy Event

public class EnemyHasBeenDestroyedEvent : SDD.Events.Event
{
    public Enemy eEnemy;
    public bool eDestroyedByPlayer;
}

#endregion

#region Bomb Events

class BombIsDestroyingEvent : SDD.Events.Event
{
    public Bomb eBomb;
}

class AllBombsHaveBeenDestroyedEvent : SDD.Events.Event
{
}

#endregion

#region Score Event

public class ScoreItemEvent : SDD.Events.Event
{
    public IScore eScore;
}

#endregion

#region Game Manager Additional Event

public class AskToGoToNextLevelEvent : SDD.Events.Event
{
}

public class GoToNextLevelEvent : SDD.Events.Event
{
}

public class GoToLevelEvent : SDD.Events.Event
{
    public int eLevelIndex;
}

#endregion

#region Movement Manager Event

public class MoveElementEvent : SDD.Events.Event
{
    public IMoveable eMoveable;
    public Vector3 eDirection;
}

#endregion

#region Level Events

public class AllEnemiesOfLevelHaveBeenDestroyedEvent : SDD.Events.Event
{
}

public class BombPointsForPowerCoinsChangedEvent : SDD.Events.Event
{
    public float ePoints { get; set; }
}

#endregion

#region LevelsManager Events

public class LevelHasBeenInstantiatedEvent : SDD.Events.Event
{
    public Level eLevel;
}

#endregion

#region Player

public class PlayerHasBeenHitEvent : SDD.Events.Event
{
    public Enemy eEnemy;
}

#endregion

/*public class PlayerHasMovedEvent : SDD.Events.Event
{
	public Player ePlayer;
}*/

#region PowerCoin

public class PowerCoinHasBeenHitEvent : SDD.Events.Event
{
}

#endregion