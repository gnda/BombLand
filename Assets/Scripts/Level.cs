using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using System;
using System.Linq;
using Common;
using DefaultNamespace;
using Random = UnityEngine.Random;

public class Level : MonoBehaviour, IEventHandler
{
    enum LevelState
    {
        none,
        enemiesAreEnemies,
        enemiesAreCoins
    };

    LevelState m_LevelState;

    [SerializeField] private GameObject[] playerPrefabs;
    [SerializeField] private GameObject[] enemyPrefabs;

    List<Player> players = new List<Player>();
    List<Enemy> enemies = new List<Enemy>();

    [SerializeField] float m_WaitDurationBeforeLightFirstWick;

    [SerializeField] float m_NBombPointsToBeCollectedForPowerCoin;
    float m_CollectedBombPoints = 0;

    [SerializeField] GameObject m_PowerCoinPrefab;
    [SerializeField] Transform[] m_PowerCoinSpawnPoints;

    [SerializeField] float m_EnemiesBecomeCoinDuration;

    [SerializeField] private Texture2D levelDesign;
    [SerializeField] private GameObject groundTilePrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject destroyableWallPrefab;

    public char[,] TilesState { get; set; }
    private GameObject levelElements;

    Vector3 RandomSpawnPos
    {
        get
        {
            List<Vector3> spawnPositions = m_PowerCoinSpawnPoints.Select(item => item.position)
                .Where(item => !Physics.CheckSphere(item, m_PowerCoinPrefab.GetComponent<SphereCollider>().radius))
                .ToList();
            spawnPositions.Sort((a, b) => Random.value.CompareTo(.5f));

            return spawnPositions[Random.Range(0, spawnPositions.Count)];
        }
    }

    public void SubscribeEvents()
    {
        //EventManager.Instance.AddListener<PlayerHasMovedEvent>(PlayerHasMoved);
        EventManager.Instance.AddListener<EnemyHasBeenDestroyedEvent>(EnemyHasBeenDestroyed);
        EventManager.Instance.AddListener<BombIsDestroyingEvent>(BombIsDestroying);
        //EventManager.Instance.AddListener<PowerCoinHasBeenHitEvent>(PowerCoinHasBeenHit);
    }

    public void UnsubscribeEvents()
    {
        //EventManager.Instance.RemoveListener<PlayerHasMovedEvent>(PlayerHasMoved);
        EventManager.Instance.RemoveListener<EnemyHasBeenDestroyedEvent>(EnemyHasBeenDestroyed);
        EventManager.Instance.RemoveListener<BombIsDestroyingEvent>(BombIsDestroying);
        //EventManager.Instance.RemoveListener<PowerCoinHasBeenHitEvent>(PowerCoinHasBeenHit);
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
        //m_Enemies = GetComponentsInChildren<Enemy>().ToList();

        m_LevelState = LevelState.enemiesAreEnemies;

        //Bomb.LightTheWickOfRandomBomb(m_WaitDurationBeforeLightFirstWick);

        SetBombPoints(0);
    }

    private void GenerateLevel()
    {
        int i, j;
        float x, z;
        TilesState = new char[levelDesign.width, levelDesign.height];
        levelElements = new GameObject("Level Scene Elements");
        GameObject tilesGO = new GameObject("Tiles");
        GameObject wallsGO = new GameObject("Walls");
        GameObject playersGO = new GameObject("Players");
        GameObject enemiesGO = new GameObject("Enemies");
        
        levelElements.transform.SetParent(transform);
        tilesGO.transform.SetParent(levelElements.transform);
        wallsGO.transform.SetParent(levelElements.transform);
        playersGO.transform.SetParent(transform);
        enemiesGO.transform.SetParent(transform);

        bool once = false;
            
        //Iterate over the texture's pixels in order to generate the level
        for (i = 0, x = 0; i < levelDesign.width; i++, x++)
        {
            for (j = 0, z = 0; j < levelDesign.height; j++, z++)
            {
                Color c = levelDesign.GetPixel(i, j);
                GameObject tileGO = GenerateTile(x, 0, z, Color.white, 
                    tilesGO.transform);

                if (c == Color.black)
                {
                    TilesState[i, j] = 'X';
                    Instantiate(wallPrefab, new Vector3(x, 0.5f, z), Quaternion.identity, 
                        wallsGO.transform);
                }
                else if (c == Color.yellow)
                {
                    TilesState[i, j] = 'D';
                    Instantiate(destroyableWallPrefab, new Vector3(x, 0.5f, z), 
                        Quaternion.identity, wallsGO.transform);
                } 
                else if (c == Color.red && !once)
                {
                    once = true;
                    TilesState[i, j] = 'E';
                    GameObject enemyGO = Instantiate(enemyPrefabs[0],
                        tileGO.transform.position + Vector3.up, 
                        Quaternion.identity, enemiesGO.transform);
                    enemyGO.name = "Enemy " + (enemies.Count + 1);
                    enemyGO.GetComponent<IMoveable>().Symbol = TilesState[i, j];
                    enemies.Add(enemyGO.GetComponent<Enemy>());
                }
                else if (c == Color.green)
                {
                    TilesState[i, j] = (players.Count + 1).ToString()[0];
                    GameObject playerGO = Instantiate(playerPrefabs[0],
                        tileGO.transform.position + Vector3.up, 
                        Quaternion.identity, playersGO.transform);
                    playerGO.name = "Player " + TilesState[i, j];
                    playerGO.GetComponent<IMoveable>().Symbol = TilesState[i, j];
                    players.Add(playerGO.GetComponent<Player>());
                }
                else if (c == Color.white)
                {
                    TilesState[i, j] = '.';
                }
            }
        }

        EventManager.Instance.Raise(new LevelHasBeenInstantiatedEvent() { eLevel = this });
    }

    private GameObject GenerateTile(float x, float y, float z, Color c, Transform transf)
    {
        GameObject tile = Instantiate(groundTilePrefab, new Vector3(x, y, z), Quaternion.identity, transf);
        tile.name = "Tile (" + x + ", " + z + ")";
        tile.GetComponent<Renderer>().material.color = c;

        return tile;
    }

    /*public int[] findElementInTiles(char element)
    {
        for (int i = 0; i < TilesState.GetLength(0); i++)
            for (int j = 0; j < TilesState.GetLength(1); j++)
                if(TilesState[i,j] == element) return new int[] {i,j};
        return null;
    }*/

   /* private void updateElementCoord(char e, int x, int z)
    {
        int[] previousCoord = findElementInTiles(e);

        TilesState[previousCoord[0], previousCoord[1]] = '.';
        TilesState[x, z] = e;
    }*/

    void SetBombPoints(float bombPoints)
    {
        m_CollectedBombPoints = bombPoints;
        EventManager.Instance.Raise(new BombPointsForPowerCoinsChangedEvent() {ePoints = m_CollectedBombPoints});
    }

    void BombIsDestroying(BombIsDestroyingEvent e)
    {
        /*if (Bomb.AreAllBombsDestroyed)
            EventManager.Instance.Raise(new AllBombsHaveBeenDestroyedEvent());
        else if (m_LevelState == LevelState.enemiesAreEnemies
                 && m_CollectedBombPoints < m_NBombPointsToBeCollectedForPowerCoin)
        {
            SetBombPoints(Mathf.Clamp(m_CollectedBombPoints + e.eBomb.PointsForPowerCoin, 0,
                m_NBombPointsToBeCollectedForPowerCoin));
            if (m_CollectedBombPoints == m_NBombPointsToBeCollectedForPowerCoin)
                Instantiate(m_PowerCoinPrefab, RandomSpawnPos, Quaternion.identity, transform);
        }*/
    }

    /*IEnumerator EnemiesBecomeCoinsCoroutine(float duration)
    {
        m_LevelState = LevelState.enemiesAreCoins;
        foreach (var item in enemies)
        {
            item.GetComponent<Enemy>().BeACoin(m_EnemiesBecomeCoinDuration);
        }

        yield return new WaitForSeconds(duration);
        m_LevelState = LevelState.enemiesAreEnemies;
        SetBombPoints(0);
    }*/

   /* void PowerCoinHasBeenHit(PowerCoinHasBeenHitEvent e)
    {
        StartCoroutine(EnemiesBecomeCoinsCoroutine(m_EnemiesBecomeCoinDuration));
    }*/

    void EnemyHasBeenDestroyed(EnemyHasBeenDestroyedEvent e)
    {
        enemies.RemoveAll(item => item.Equals(null));
        enemies.Remove(e.eEnemy);
    }
    
    /*void PlayerHasMoved(PlayerHasMovedEvent e)
    {
        int index = players.FindIndex(item => item.Equals(e.ePlayer)) + 1;
        updateElementCoord(index.ToString()[0], 
            (int) e.ePlayer.transform.position.x, (int) e.ePlayer.transform.position.z);
    }*/
}