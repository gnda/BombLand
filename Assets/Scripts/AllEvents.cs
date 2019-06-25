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
    public Player ePlayer;
}

public class GameExitEvent : SDD.Events.Event
{
}

public class GameStatisticsChangedEvent : SDD.Events.Event
{
    public int eBestScore { get; set; }
    public int ePlayerNumber { get; set; }
    public int eScore { get; set; }
    public int eNMonstersLeft{ get; set; }
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

public class CreateARoomButtonClickedEvent : SDD.Events.Event
{
}

public class PseudoOkButtonClickedEvent : SDD.Events.Event
{
}

public class JoinRoomButtonClickedEvent : SDD.Events.Event
{
}

public class SendMessageButtonClickedEvent : SDD.Events.Event
{
}

public class StartMultiplayerGameButtonClickedEvent : SDD.Events.Event
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

public class ExitButtonClickedEvent : SDD.Events.Event
{
}
#endregion

#region Element Event
public class ElementMustBeDestroyedEvent : SDD.Events.Event
{
    public GameObject eElement;
}

public class ElementIsBeingDestroyedEvent : SDD.Events.Event
{
    public GameObject eElement;
}
#endregion

#region Score Event

public class ScoreItemEvent : SDD.Events.Event
{
    public GameObject eElement;
    public Player ePlayer;
}

#endregion

#region Game Manager Additional Event
public class GoToNextLevelEvent : SDD.Events.Event
{
    public int eLevelIndex;
}

public class GameHasStartedEvent : SDD.Events.Event
{
}

public class TimeIsUpEvent : SDD.Events.Event
{
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
public class LevelHasBeenDestroyedEvent : SDD.Events.Event
{
}

public class LevelHasBeenInstantiatedEvent : SDD.Events.Event
{
    public Level eLevel;
}
#endregion