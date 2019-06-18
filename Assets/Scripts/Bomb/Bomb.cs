using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class Bomb : MonoBehaviour
{

	[SerializeField] private GameObject explosionPrefab;
	
	[SerializeField] private float offDuration;
	[SerializeField] private float litDuration;
	[SerializeField] private float explosionDuration;
	[SerializeField] private int baseTileExplosionRange;
	[SerializeField] private int baseWallExplosionRange;
	
	private char[,] tilesState;
	public Player Player { get; set; }

	private void Start()
	{
		tilesState = GameManager.Instance.Level.TilesState;
		StartCoroutine(TheBombIsOff());
	}

	#region BombLifeCycleCoroutines

	IEnumerator DestroyTheBombCoroutine()
	{
		yield return new WaitForSeconds(explosionDuration);
		Destroy(gameObject);
	}
	
	IEnumerator LightTheBombCoroutine()
	{
		yield return new WaitForSeconds(litDuration);
		explode();
		StartCoroutine(DestroyTheBombCoroutine());
	}

	IEnumerator TheBombIsOff()
	{
		yield return new WaitForSeconds(offDuration);
		StartCoroutine(LightTheBombCoroutine());
	}

	#endregion

	private void explode()
	{
		Vector3[] direction = new[] {Vector3.forward, -Vector3.forward, -Vector3.right, Vector3.right};
		int destroyedBlocksCount;

		foreach (Vector3 d in direction)
		{
			destroyedBlocksCount = 0;
			for (int i = 1; i <= baseTileExplosionRange; i++)
			{
				Vector3 newPosition = transform.position + d * i;
				if (tilesState[(int) newPosition.x, (int) newPosition.z] != 'X' 
				    && destroyedBlocksCount < baseWallExplosionRange){
					GameObject explosionGO = Instantiate(explosionPrefab, transform);
					explosionGO.SetActive(true);
					explosionGO.name = "Explosion " + i;
					explosionGO.transform.position = newPosition;
					if (tilesState[(int) newPosition.x, (int) newPosition.z] == 'D')
						destroyedBlocksCount++;
				}
				else break;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		/*if(GameManager.Instance.IsPlaying && other.CompareTag("Player"))
		{
			EventManager.Instance.Raise(new ScoreItemEvent() { eScore = this as IScore });
			EventManager.Instance.Raise(new BombIsDestroyingEvent() { eBomb = this });
			Destroy(gameObject);
		}*/
	}
}
