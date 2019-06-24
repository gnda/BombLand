using UnityEngine;

public class EnemyAIPlayer : SimpleGameStateObserver{
	
	public Transform Transf { get; set; }
	public bool IsMoving { get; set; }

	protected override void Awake()
	{
		base.Awake();
		Transf = GetComponent<Transform>();
	}

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (GameManager.Instance && !GameManager.Instance.IsPlaying) return;

		if (!IsMoving) Move();
	}

	private void Move()
	{
		
	}
}