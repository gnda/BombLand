using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using System;
using System.Linq;
using Random = UnityEngine.Random;

public class Level : MonoBehaviour,IEventHandler {

    enum LevelState { none, enemiesAreEnemies,enemiesAreCoins};

    LevelState m_LevelState;

    List<Enemy> m_Enemies = new List<Enemy>();

    [SerializeField] float m_WaitDurationBeforeLightFirstWick;

    [SerializeField] float m_NBombPointsToBeCollectedForPowerCoin;
    float m_CollectedBombPoints = 0;

    [SerializeField] GameObject m_PowerCoinPrefab;
    [SerializeField] Transform[] m_PowerCoinSpawnPoints;

    [SerializeField] float m_EnemiesBecomeCoinDuration;

    [SerializeField] private Texture2D levelDesign;
    [SerializeField] private GameObject groundTilePrefab;

    private bool[,] tilesState;
    private GameObject groundTiles;

    Vector3 RandomSpawnPos { get {
            List<Vector3> spawnPositions = m_PowerCoinSpawnPoints.Select(item => item.position).Where(item=>!Physics.CheckSphere(item,m_PowerCoinPrefab.GetComponent<SphereCollider>().radius)).ToList();
            spawnPositions.Sort((a, b) => Random.value.CompareTo(.5f));

            return spawnPositions[Random.Range(0, spawnPositions.Count)]; }
    }

    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<EnemyHasBeenDestroyedEvent>(EnemyHasBeenDestroyed);
        EventManager.Instance.AddListener<BombHasBeenDestroyedEvent>(BombHasBeenDestroyed);
        EventManager.Instance.AddListener <PowerCoinHasBeenHitEvent>(PowerCoinHasBeenHit);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<EnemyHasBeenDestroyedEvent>(EnemyHasBeenDestroyed);
        EventManager.Instance.RemoveListener<BombHasBeenDestroyedEvent>(BombHasBeenDestroyed);
        EventManager.Instance.RemoveListener<PowerCoinHasBeenHitEvent>(PowerCoinHasBeenHit);
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void Awake()
    {
        SubscribeEvents();
    }

    private void Start()
    {
        GenerateLevel();

        //enemies
        m_Enemies = GetComponentsInChildren<Enemy>().ToList();

        m_LevelState = LevelState.enemiesAreEnemies;

        Bomb.LightTheWickOfRandomBomb(m_WaitDurationBeforeLightFirstWick);

        SetBombPoints(0);
    }

    private void GenerateLevel()
    {
        int i, j;
        float x, z;
        tilesState = new bool[levelDesign.width, levelDesign.height];
        groundTiles = new GameObject("Ground Tiles");
        groundTiles.transform.SetParent(this.transform);

        // Iterate through it's pixel
        for (i = 0, x = 0; i < levelDesign.width; i++, x++)
        {
            for (j = 0, z = 0; j < levelDesign.height; j++, z++)
            {
                Color c = levelDesign.GetPixel(i, j);

                if (c == Color.black)
                {
                    GameObject tile = GenerateTile(x, 0, z, c);
                    tile.transform.SetParent(groundTiles.transform);
                }
                else if (c == Color.red) 
                {
                    GameObject tile = GenerateTile(x, 0, z, c);
                    tile.transform.SetParent(groundTiles.transform);
                }
                else if (c == Color.green)
                {
                    GameObject tile = GenerateTile(x, 0, z, c);
                    tile.transform.SetParent(groundTiles.transform);
                }
                else if (c == Color.white)
                {
                    GameObject tile = GenerateTile(x, 0, z, c);
                    tile.transform.SetParent(groundTiles.transform);
                }
            }
        }
    }

    private GameObject GenerateTile(float x, float y, float z, Color c)
    {
        GameObject tile = Instantiate(groundTilePrefab, new Vector3(x, y, z), Quaternion.identity);
        tile.name = "Tile (" + x + ", " + z + ")";
        tile.GetComponent<Renderer>().material.color = c;

        return tile;
    }

    void SetBombPoints(float bombPoints)
    {
        m_CollectedBombPoints = bombPoints;
        EventManager.Instance.Raise(new BombPointsForPowerCoinsChangedEvent() { ePoints = m_CollectedBombPoints });
    }

    void BombHasBeenDestroyed(BombHasBeenDestroyedEvent e)
    {
        if (Bomb.AreAllBombsDestroyed)
            EventManager.Instance.Raise(new AllBombsHaveBeenDestroyedEvent());
        else if(m_LevelState == LevelState.enemiesAreEnemies
                && m_CollectedBombPoints < m_NBombPointsToBeCollectedForPowerCoin)
        {
            SetBombPoints(Mathf.Clamp(m_CollectedBombPoints + e.eBomb.PointsForPowerCoin, 0, m_NBombPointsToBeCollectedForPowerCoin));
            if (m_CollectedBombPoints == m_NBombPointsToBeCollectedForPowerCoin)
                Instantiate(m_PowerCoinPrefab, RandomSpawnPos,Quaternion.identity,  transform);
        }
    }

    IEnumerator EnemiesBecomeCoinsCoroutine(float duration)
    {
        m_LevelState = LevelState.enemiesAreCoins;
        foreach (var item in m_Enemies)
        {
            item.BeACoin(m_EnemiesBecomeCoinDuration);
        }

        yield return new WaitForSeconds(duration);
        m_LevelState = LevelState.enemiesAreEnemies;
        SetBombPoints(0);
    }

    void PowerCoinHasBeenHit(PowerCoinHasBeenHitEvent e)
    {
        StartCoroutine(EnemiesBecomeCoinsCoroutine(m_EnemiesBecomeCoinDuration));
    }

    void EnemyHasBeenDestroyed(EnemyHasBeenDestroyedEvent e)
    {
        m_Enemies.RemoveAll(item => item.Equals(null));
        m_Enemies.Remove(e.eEnemy);
    }
}