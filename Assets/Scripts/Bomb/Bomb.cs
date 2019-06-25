using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    #region Prefabs & Settings
    [Header("BombPrefabs")]
    [SerializeField] private GameObject explosionPrefab;

    [Header("BombSettings")]
    [SerializeField] private float offDuration;
    [SerializeField] private float litDuration;
    [SerializeField] private float explosionDuration;
    [SerializeField] private int baseTileExplosionRange;
    [SerializeField] private int baseWallExplosionRange;

    public float ExplosionDuration { 
        get => explosionDuration;
        set => explosionDuration = value;
    }

    public int BaseWallExplosionRange
    {
        get => baseWallExplosionRange;
        set => baseWallExplosionRange = value;
    }
    public int BaseTileExplosionRange
    {
        get => baseTileExplosionRange;
        set => baseTileExplosionRange = value;
    }

    #endregion
    public Player Player { get; set; }

    #region MonoBehaviour lifecycle
    private void Start()
    {
        StartCoroutine(TheBombIsOff());
    }
    #endregion

    #region Bomb lifecycle coroutines
    IEnumerator LightTheBombCoroutine()
    {
        yield return new WaitForSeconds(litDuration);
        SfxManager.Instance.PlaySfx(Constants.EXPLOSION_SFX);
        explode();
        Destroy(gameObject);
    }

    IEnumerator TheBombIsOff()
    {
        SfxManager.Instance.PlaySfx(Constants.DROP_SFX);
        yield return new WaitForSeconds(offDuration);
        StartCoroutine(LightTheBombCoroutine());
    }
    #endregion

    #region Explosion methods
    private int destroyedBlocksCount;
    private void explode()
    {
        Vector3[] direction = new[] {Vector3.forward, -Vector3.forward, -Vector3.right, Vector3.right};

        createExplosion(transform.position);
        
        foreach (Vector3 d in direction)
        {
            destroyedBlocksCount = 0;

            for (int i = 1; i <= baseTileExplosionRange; i++)
            {
                Vector3 newPosition = transform.position + d * i;

                List<RaycastHit> hits =
                    Physics.RaycastAll(transform.position, d, 1f * i).ToList();
                
                if (hits.Count > 0)
                {
                    if (hits.TrueForAll(item => 
                            Vector3.Distance(newPosition, item.transform.position) > 0))
                        createExplosion(newPosition);
                    else
                    {
                        hits.RemoveAll(item => 
                            item.transform.GetComponent<Destroyable>() == null);

                        if (hits.Count > 0)
                        {
                            hits.Sort((a, b) =>
                                Vector3.Distance(transform.position, a.point)
                                    .CompareTo(Vector3.Distance(transform.position, b.point)));

                            createExplosion(newPosition);
                            
                            if (hits[0].transform.CompareTag("Platform"))
                                destroyedBlocksCount++;
                                
                            if (destroyedBlocksCount >= baseWallExplosionRange)
                                break;
                        }
                        else break;
                    }
                }
                else createExplosion(newPosition);
            }
        }
    }

    private void createExplosion(Vector3 newPosition)
    {
        GameObject explosionGO = Instantiate(explosionPrefab, 
            transform.position, Quaternion.identity);
        explosionGO.transform.SetParent(transform.parent);
        explosionGO.GetComponent<Explosion>().Bomb = GetComponent<Bomb>();
        
        explosionGO.SetActive(true);
        explosionGO.transform.position = newPosition;
    }
    #endregion
}