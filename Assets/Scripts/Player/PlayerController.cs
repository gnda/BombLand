using DefaultNamespace;
using UnityEngine;
using SDD.Events;

public class PlayerController : SimpleGameStateObserver, IEventHandler, IMoveable
{
	#region Movement
	[Header("MovementSettings")]
	[SerializeField] private float moveDuration = 0.2f;
	
	public Transform Transf { get; set; }
	public bool IsMoving { get; set; } = false;
	public bool IsDestroyed { get; set; }

	public float MoveDuration
	{
		get { return moveDuration; }
		set { moveDuration = value; }
	}
	#endregion
	
	#region Bomb
	[Header("BombSettings")]
	[SerializeField] private float dropCoolDownDuration = 1;
	
	float nextDropTime;
	#endregion

	#region MonoBehaviour lifecycle
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
			EventManager.Instance.Raise(new MoveElementEvent()
			{
				eMoveable = this,
				eDirection = Mathf.Sign(hInput) * Vector3.right
			});
		else if (vInput != 0f)
			EventManager.Instance.Raise(new MoveElementEvent()
			{
				eMoveable = this,
				eDirection = Mathf.Sign(vInput) * Vector3.forward
			});

		if (Input.GetButtonDown("Jump") && Time.time > nextDropTime)
		{
			GameObject bombGO = Instantiate(GetComponent<Player>().bombPrefab,
				GetComponentInParent<Level>().transform);
			bombGO.GetComponent<Bomb>().Player = GetComponent<Player>();
			bombGO.transform.position = 
				new Vector3((int)Transf.position.x, 0.5f, (int)Transf.position.z);

			nextDropTime = Time.time + dropCoolDownDuration;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(GameManager.Instance.IsPlaying
			&& other.gameObject.CompareTag("Enemy"))
		{
			EventManager.Instance.Raise(new ElementMustBeDestroyedEvent()
					{ eElement = gameObject });
		}
	}
	#endregion
}