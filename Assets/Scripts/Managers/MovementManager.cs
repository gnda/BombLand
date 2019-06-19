using System.Collections;
using DefaultNamespace;
using JetBrains.Annotations;
using SDD.Events;
using UnityEngine;

public class MovementManager : Manager<MovementManager> 
{
	private bool isMoving = false;
    char[,] tilesState;
    
    private delegate float EasingFunctionDelegate(float start, float end, float value);

    #region Events' subscription
    public override void SubscribeEvents()
    {
	    base.SubscribeEvents();
	    
	    EventManager.Instance.AddListener<LevelHasBeenInstantiatedEvent>(LevelHasBeenInstantiated);
	    EventManager.Instance.AddListener<MoveElementEvent>(MoveElement);
    }

    public override void UnsubscribeEvents()
    {
	    base.UnsubscribeEvents();
	    
	    EventManager.Instance.RemoveListener<LevelHasBeenInstantiatedEvent>(LevelHasBeenInstantiated);
	    EventManager.Instance.RemoveListener<MoveElementEvent>(MoveElement);
    }
    #endregion
    
    #region Manager implementation
    protected override IEnumerator InitCoroutine()
    {
	    yield break;
    }
    #endregion

    private void Move(IMoveable element, Vector3 direc)
    {
	    MoveWithEasing(element, direc, null);
    }
    
    private void MoveWithEasing(IMoveable element, Vector3 direc, 
	    [CanBeNull] EasingFunctionDelegate easingFunction)
    {
	    if (!element.IsMoving)
	    {
		    if (direc == Vector3.forward || direc == -Vector3.forward)
		    {
			    element.Transf.rotation =
				    Quaternion.AngleAxis(180 * 
				                         (Mathf.Sign(direc.z) > 0 ? 1f : 0f), Vector3.up);
			    
			    TriggerMovement(element, direc);
		    }
		    else if (direc == Vector3.right | direc == -Vector3.right)
		    {
			    element.Transf.rotation = 
				    Quaternion.AngleAxis(90 * Mathf.Sign(direc.x), Vector3.down);
			    
			    TriggerMovement(element, direc);
		    }
	    }
    }

    private void TriggerMovement(IMoveable element, Vector3 direc)
    {
	    Vector3 basePosition = element.Transf.position;

	    if(CheckDirection(basePosition, direc))
	    {
		    StartCoroutine(TranslationCoroutine(element, 
			    basePosition, basePosition + direc, null));
	    }
    }

    private bool CheckDirection(Vector3 basePostion, Vector3 direc)
    {
	    /*int x = (int) direc.x, z = (int) direc.z;

	    if (direc == Vector3.forward)
	    {
		    return z + 1 < tilesState.GetLength(1) 
		           && tilesState[x, z + 1] != 'X' && tilesState[x, z + 1] != 'D';
	    } 
	    else if (direc == -Vector3.forward)
	    {
		    return z - 1 < tilesState.GetLength(1) 
		           && tilesState[x, z - 1] != 'X' && tilesState[x, z - 1] != 'D';
	    }
	    else if (direc == Vector3.right)
	    {
		    return x + 1 < tilesState.GetLength(0) 
		           && tilesState[x + 1, z] != 'X' && tilesState[x + 1, z] != 'D';
	    }
	    else if (direc == -Vector3.right)
	    {
		    return x - 1 < tilesState.GetLength(0) 
		           && tilesState[x - 1, z] != 'X' && tilesState[x - 1, z] != 'D';
	    }*/

	    return tilesState[(int) (basePostion.x + direc.x), (int) (basePostion.z + direc.z)] != 'X'
	           && tilesState[(int) (basePostion.x + direc.x), (int) (basePostion.z + direc.z)] != 'D';
    }

    private IEnumerator TranslationCoroutine(IMoveable element, 
	    Vector3 startPos, Vector3 endPos, 
	    [CanBeNull] EasingFunctionDelegate easingFunction)
    {
	    float elapsedTime = 0;

	    element.IsMoving = true;
	    
	    while (elapsedTime < element.MoveDuration)
	    {
		    float elapsedTimePerc = elapsedTime /  element.MoveDuration;
		    element.Transf.position = Vector3.Lerp(startPos, endPos, 
			    easingFunction != null ? 
				    easingFunction(0,1,elapsedTimePerc) : elapsedTimePerc);

		    elapsedTime += Time.deltaTime;
		    yield return null;
	    }

	    element.Transf.position = endPos;
	    
	    tilesState[(int) startPos.x, (int) startPos.z] = '.';
	    tilesState[(int) endPos.x, (int) endPos.z] = element.Symbol;
		
	    yield return element.IsMoving = false;
    }

    private void LevelHasBeenInstantiated(LevelHasBeenInstantiatedEvent e)
    {
	    tilesState = e.eLevel.TilesState;
	    
    }
    private void MoveElement(MoveElementEvent e)
    {
	    Move(e.eMoveable, e.eDirection);
    }
}
