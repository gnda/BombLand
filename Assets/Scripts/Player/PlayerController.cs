using DefaultNamespace;
using UnityEngine;
using SDD.Events;

public class PlayerController : SimpleGameStateObserver, IEventHandler, IMoveable{
	
	public Transform Transf { get; set; }

	public float MoveDuration { get { return moveDuration;} }

	[SerializeField] private float moveDuration;
	[SerializeField] private GameObject bombPrefab;
	[SerializeField] private float dropCoolDownDuration;

	public bool IsMoving { get; set; }
	public char Symbol { get; set; }

	float nextDropTime;

	protected override void Awake()
	{
		base.Awake();
		Transf = GetComponent<Transform>();
	}

	// Use this for initialization
	void Start () {
		nextDropTime = Time.time;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (GameManager.Instance && !GameManager.Instance.IsPlaying) return;

		float hInput = Input.GetAxis("Horizontal");
		float vInput = Input.GetAxis("Vertical");

		if (hInput != 0f)
		{
			EventManager.Instance.Raise(new MoveElementEvent()
			{
				eMoveable = this,
				eDirection = Mathf.Sign(hInput) * Vector3.right
			});
		} 
		else if (vInput != 0f)
		{
			EventManager.Instance.Raise(new MoveElementEvent()
			{
				eMoveable = this,
				eDirection = Mathf.Sign(vInput) * Vector3.forward
			});
		}
		
		if (Input.GetButtonDown("Jump") && Time.time > nextDropTime)
		{
			GameObject bombGO = Instantiate(bombPrefab);
			bombGO.GetComponent<Bomb>().Player = GetComponent<Player>();
			bombGO.transform.position = 
				new Vector3((int)Transf.position.x, 0.5f, (int)Transf.position.z);

			nextDropTime = Time.time + dropCoolDownDuration;
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.CompareTag("Ground")
			|| collision.gameObject.CompareTag("Platform"))
		{
			//m_IsGrounded = false;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(GameManager.Instance.IsPlaying
			&& other.gameObject.CompareTag("Enemy"))
		{
			if(other.GetComponent<Enemy>())
				EventManager.Instance.Raise(new PlayerHasBeenHitEvent()
					{ eEnemy = GetComponent<Enemy>() });
		}
	}
}