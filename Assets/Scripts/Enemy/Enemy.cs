using DefaultNamespace;
using UnityEngine;

public abstract class Enemy : SimpleGameStateObserver,IScore,IMoveable {

	public abstract float MoveDuration { get; }
	public bool IsMoving { get; set; }
	public char Symbol { get; set; }
	public Transform Transf { get; set; }

	[Header("Movement")]

	char[,] tilesState;
	
	[Header("Score")]
	[SerializeField] int score;
	public int Score { get { return score; } }

	protected override void Awake()
	{
		base.Awake();
		Transf = GetComponent<Transform>();
	}

	public virtual void FixedUpdate()
	{
		if (!GameManager.Instance.IsPlaying) return;
	}

	private void OnTriggerEnter(Collider col)
	{
		//Debug.Log( name+" Collision with " + col.gameObject.name);
		//Debug.Break();
		if(GameManager.Instance.IsPlaying
			&& col.gameObject.CompareTag("Player"))
		{
			/*if (IsCoin)
			{
				EventManager.Instance.Raise(new ScoreItemEvent() { eScore = this as IScore });
				EventManager.Instance.Raise(new EnemyHasBeenDestroyedEvent() { eEnemy = this, eDestroyedByPlayer = true });
				m_Destroyed = true;
				Destroy(gameObject);
			}*/
		}
	}
}
