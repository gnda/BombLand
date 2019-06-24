using System;
using System.Collections;
using DefaultNamespace;
using SDD.Events;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    #region Explosion lifecycle coroutines
    public Bomb Bomb { get; set; }
    
    IEnumerator ExplosionCoroutine()
    {
        yield return new WaitForSeconds(Bomb.ExplosionDuration);
        Destroy(gameObject);
    }
    #endregion
    
    #region MonoBehaviour lifecycle
    private void Start()
    {
        StartCoroutine(ExplosionCoroutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Destroyable>())
        {
            if (other.GetComponent<IMoveable>() != null)
            {
                if(Bomb.Player.gameObject != other.gameObject)
                    EventManager.Instance.Raise(
                        new ScoreItemEvent() {eElement = other.gameObject,
                            ePlayer = Bomb.Player});
                
                EventManager.Instance.Raise(
                    new ElementMustBeDestroyedEvent() 
                        {eElement = other.gameObject});
            }
            else Destroy(other.gameObject);
        }
    }
    #endregion
}