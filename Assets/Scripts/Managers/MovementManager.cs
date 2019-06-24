using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using JetBrains.Annotations;
using SDD.Events;
using UnityEngine;

public class MovementManager : Manager<MovementManager> 
{
	#region Variables
	private delegate float EasingFunctionDelegate(float start, float end, float value);
	private Dictionary<IMoveable, Coroutine> _coroutines = null;
	#endregion

	#region Events' subscription
    public override void SubscribeEvents()
    {
	    base.SubscribeEvents();
	    
	    //Element
	    EventManager.Instance.AddListener<MoveElementEvent>(MoveElement);
	    EventManager.Instance.AddListener<ElementMustBeDestroyedEvent>(DestroyElement);
	    
	    //Level
	    EventManager.Instance.AddListener<LevelHasBeenDestroyedEvent>(LevelHasBeenDestroyed);
    }

    public override void UnsubscribeEvents()
    {
	    base.UnsubscribeEvents();
	    
	    //Element
	    EventManager.Instance.RemoveListener<MoveElementEvent>(MoveElement);
	    EventManager.Instance.RemoveListener<ElementMustBeDestroyedEvent>(DestroyElement);
	    
	    //Level
	    EventManager.Instance.RemoveListener<LevelHasBeenDestroyedEvent>(LevelHasBeenDestroyed);
    }
    #endregion
    
    #region Manager implementation
    protected override IEnumerator InitCoroutine()
    {
	    _coroutines = new Dictionary<IMoveable, Coroutine>();
	    yield break;
    }
    #endregion

    #region Movement functions
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
		    _coroutines[element] =  
			    StartCoroutine(TranslationCoroutine(element, 
			    basePosition, basePosition + direc, null));
    }

    private bool CheckDirection(Vector3 basePosition, Vector3 direc)
    {
	    RaycastHit hit;

	    if (Physics.Raycast(basePosition, direc, out hit, 1f))
		    return !hit.transform.CompareTag("Platform");

	    return true;
    }

    private IEnumerator TranslationCoroutine(IMoveable element, 
	    Vector3 startPos, Vector3 endPos, 
	    [CanBeNull] EasingFunctionDelegate easingFunction)
    {
	    float elapsedTime = 0;
	    
	    element.IsMoving = true;
    
	    while (elapsedTime < element.MoveDuration)
	    {
		    float elapsedTimePerc = elapsedTime / element.MoveDuration;
		    element.Transf.position = Vector3.Lerp(startPos, endPos,
			    easingFunction != null ? 
				    easingFunction(0, 1, elapsedTimePerc) : elapsedTimePerc);

		    elapsedTime += Time.deltaTime;
		    yield return null;
	    }
	    
	    element.Transf.position = endPos;
	    _coroutines.Remove(element);
	    
	    yield return element.IsMoving = false;
    }
    
    private void MoveElement(MoveElementEvent e)
    {
	    Move(e.eMoveable, e.eDirection);
    }

    private void DestroyElement(ElementMustBeDestroyedEvent e)
    {
	    EventManager.Instance.Raise(new ElementIsBeingDestroyedEvent()
	    { eElement = e.eElement });
	    
	    IMoveable element = e.eElement.GetComponent<IMoveable>();
	    element.IsDestroyed = true;
	    
	    if (_coroutines.ContainsKey(element))
	    {
		    StopCoroutine(_coroutines[element]);
		    _coroutines.Remove(element);
		    Destroy(e.eElement);
	    } else Destroy(e.eElement);
    }
    #endregion

    #region Level Events Callbacks
    private void LevelHasBeenDestroyed(LevelHasBeenDestroyedEvent e)
    {
	    if(_coroutines != null)
		    foreach (var coroutine in _coroutines.Values)
			    StopCoroutine(coroutine);
    }
	#endregion
}
