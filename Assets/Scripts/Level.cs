using UnityEngine;
using SDD.Events;

public class Level : MonoBehaviour, IEventHandler
{
    #region Settings & Prefabs
    [Header(("LevelSettings"))]
    [SerializeField] private float levelDuration;

    public float LevelDuration
    {
        get => levelDuration;
        set => levelDuration = value;
    }
    
    [SerializeField] private Vector3 levelPosition;
    [SerializeField] private Texture2D levelDesign;
    
    [Header(("LevelPrefabs"))]
    [SerializeField] private GameObject[] playerPrefabs;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject[] bombPrefabs;
    [SerializeField] private GameObject[] bonusPrefabs;
    [SerializeField] private GameObject backgroundPrefab;
    [SerializeField] private GameObject groundTilePrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject destroyableWallPrefab;
    #endregion

    #region Events' subscription
    public void SubscribeEvents()
    {
    }

    public void UnsubscribeEvents()
    {
    }
    #endregion
    
    #region MonoBehaviour lifecycle
    private void Awake()
    {
        SubscribeEvents();
    }

    private void Start()
    {
        GenerateLevel();
    }
    
    private void OnDestroy()
    {
        UnsubscribeEvents();
    }
    #endregion

    #region Level generation
    private GameObject levelElements, tilesGO, wallsGO, bonusesGO, playersGO, monstersGO;
    
    private void GenerateLevel()
    {
        int i, j;
        float x, z;
        
        //Level static scene elements
        levelElements = new GameObject("Level Scene Elements");
        tilesGO = new GameObject("Tiles");
        wallsGO = new GameObject("Walls");
        bonusesGO = new GameObject("Bonuses");
        
        //Moveable elements
        playersGO = new GameObject("Players");
        monstersGO = new GameObject("Enemies");
        
        //Background
        GameObject backgroundGO = Instantiate(backgroundPrefab, transform);
        Vector3 cameraPosition = Camera.main.transform.position;
        backgroundGO.transform.position = 
            new Vector3(cameraPosition.x, -0.1f, 8.15f);
        
        levelElements.transform.SetParent(transform);
        tilesGO.transform.SetParent(levelElements.transform);
        wallsGO.transform.SetParent(levelElements.transform);
        bonusesGO.transform.SetParent(levelElements.transform);
        playersGO.transform.SetParent(transform);
        monstersGO.transform.SetParent(transform);

        //Iterate over the texture's pixels in order to generate the level
        for (i = 0, x = 0; i < levelDesign.width; i++, x++)
            for (j = 0, z = 0; j < levelDesign.height; j++, z++)
            {
                Color c = levelDesign.GetPixel(i, j);
                GameObject tileGO = GenerateTile(x, 0, z, tilesGO.transform);

                if (c == Color.black)
                {
                    Instantiate(wallPrefab, new Vector3(x, 0.5f, z), Quaternion.identity, 
                        wallsGO.transform);
                }
                else if (c == Color.yellow)
                {
                    Instantiate(destroyableWallPrefab, new Vector3(x, 0.5f, z), 
                        Quaternion.identity, wallsGO.transform);
                    if (Random.Range(0, 10) == 5)
                        Instantiate(bonusPrefabs[Random.Range(0, bonusPrefabs.Length)], 
                            new Vector3(x, 0.4f, z), Quaternion.identity,
                            bonusesGO.transform);
                } 
                else if (c == Color.red)
                    SpawnMonster(tileGO.transform.position + Vector3.up * 0.5f);
                else if (c == Color.green)
                    SpawnPlayer(tileGO.transform.position + Vector3.up * 0.5f);
            }

        levelElements.transform.position = levelPosition;
        playersGO.transform.position = levelPosition;
        monstersGO.transform.position = levelPosition;

        EventManager.Instance.Raise(new LevelHasBeenInstantiatedEvent() { eLevel = this });
    }

    private GameObject GenerateTile(float x, float y, float z, Transform transf)
    {
        GameObject tile = Instantiate(groundTilePrefab, 
            new Vector3(x, y, z), Quaternion.identity, transf);
        tile.name = "Tile (" + x + ", " + z + ")";

        return tile;
    }

    private void SpawnMonster(Vector3 position)
    {
        GameObject enemyGO = Instantiate(enemyPrefabs[0],position, 
            Quaternion.identity, monstersGO.transform);
        enemyGO.name = "Enemy " + 
                       monstersGO.GetComponentsInChildren<Monster>().Length;
    }

    private void SpawnPlayer(Vector3 position)
    {
        if (GameManager.Instance.GameMode != GameMode.multiplayer)
        {
            int indexPlayer = playersGO.GetComponentsInChildren<Player>().Length;
            
            if (indexPlayer < GameManager.Instance.NumberOfPlayer)
            {
                GameObject playerGO = Instantiate(playerPrefabs[indexPlayer], 
                    position, Quaternion.identity, playersGO.transform);
                
                if ((GameManager.Instance.PlayerType == PlayerType.human) ||
                    (playersGO.GetComponentsInChildren<Player>().Length == 1))
                    playerGO.AddComponent<PlayerController>();
                else
                    playerGO.AddComponent<EnemyAIPlayer>();
                
                playerGO.GetComponent<Player>().bombPrefab = bombPrefabs[0];
            }
            else SpawnMonster(position);
        }
    }
    #endregion
}