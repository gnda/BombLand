using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace;
using SDD.Events;
using System.Linq;

public class EnemyAIPlayer : SimpleGameStateObserver, IEventHandler, IMoveable{

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

    private Vector3 NextDirection
    {
        get
        {
            List<Vector3> directions = new List<Vector3>()
                {Vector3.right, Vector3.forward, -Vector3.right, -Vector3.forward};

            for (int i = directions.Count - 1; i >= 0; i--)
            {
                List<RaycastHit> hits;
                hits = Physics.RaycastAll(Transf.position, directions[i]).ToList();

                hits.Sort((a, b) => Vector3
                    .Distance(Transf.position, a.transform.position).CompareTo(
                    Vector3.Distance(Transf.position, b.transform.position)));

                if (hits[0].transform.GetComponent<Destroyable>() &&
                               hits[0].transform.CompareTag("Platform"))
                {
                    GameObject bombGO = Instantiate(GetComponent<Player>().bombPrefab,
                        GetComponentInParent<Level>().transform);
                    bombGO.GetComponent<Bomb>().Player = GetComponent<Player>();
                    bombGO.transform.position =
                        new Vector3((int)Transf.position.x, 0.5f, (int)Transf.position.z);

                    nextDropTime = Time.time + dropCoolDownDuration;

                    directions.RemoveAt(i);
                    break;
                }

                foreach (var hit in hits)
                {
                    Debug.DrawLine(Transf.position, hit.point, Color.red, 100f);

                    if (hit.transform.GetComponent<Player>() ||
                        hit.transform.GetComponent<Monster>())
                        return directions[i];
                    else if (hit.transform.GetComponent<Bomb>() ||
                             hit.transform.GetComponent<Explosion>())
                    {
                        directions.RemoveAt(i);
                        break;
                    }
                }

                /*if (hit.transform.GetComponent<Player>())
                    return directions[i];*/

                /*if (i < directions.Count - 1)
                    directions.RemoveAt(Random.Range(0, 2) == 0 ? i + 1 : i);*/
            }

            return directions[0];
            //return Vector3.zero;
        }
    }
    #endregion

    #region Bomb
    [Header("BombSettings")]
    [SerializeField] private float dropCoolDownDuration = 1;

    float nextDropTime;
    #endregion

    #region MonoBehaviour lifecycle
    protected void Awake()
    {
        Transf = GetComponent<Transform>();
    }

    public void FixedUpdate()
    {
        if (!GameManager.Instance.IsPlaying) return;

        if (!IsMoving)
            EventManager.Instance.Raise(new MoveElementEvent()
            { eMoveable = this, eDirection = NextDirection });
    }
    #endregion
}