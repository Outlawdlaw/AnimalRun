using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private int MinPoolSize { get; set; }

    private void OnValidate()
    {
        MinPoolSize = numberOfStartingTiles * 2;
    }

    [FoldoutGroup("Tiles")] [SerializeField]
    private List<GameObject> tilePrefabs;

    [FoldoutGroup("Tiles")] [MinValue(1)] [MaxValue(50)] [SerializeField]
    private int numberOfStartingTiles;

    [FoldoutGroup("Tiles")] [SerializeField]
    private Vector3 tilePositionOffset = Vector3.zero;

    [FoldoutGroup("Tile Object Pool")]
    [Tooltip("The number of complete tilePrefabs to preload into the scene")]
    [MinValue("MinPoolSize")]
    [SerializeField]
    private int poolSize;

    [FoldoutGroup("Tile Object Pool")] [SerializeField]
    private Vector3 poolSpawnOffset = Vector3.zero;

    private List<GameObject> _activeTiles;
    private List<GameObject> _pooledTiles;

    [FoldoutGroup("Tile Speed")] [MinValue(0.1)] [SerializeField]
    private float initialTileSpeed;
    [MinValue(0)]
    [SerializeField]
    private float speedIncrementAmount;
    [FoldoutGroup("Tile Speed")]
    [MinValue(1)]
    [SerializeField]
    private float speedIncrementInterval;

    private void Awake()
    {
        _activeTiles = new List<GameObject>();
        _pooledTiles = new List<GameObject>();
    }

    private void Start()
    {
        PopulateTilePool();
        InitializeStartingTiles();

        SetInitialTileSpeed();
        
        InvokeRepeating("IncreaseTileSpeed", speedIncrementInterval, speedIncrementInterval);
    }

    private void SetInitialTileSpeed()
    {
        foreach (var tile in _pooledTiles)
        {
            tile.GetComponent<Tile>().movementSpeed = initialTileSpeed;
        }
    }

    private void IncreaseTileSpeed()
    {
        foreach (var tile in _pooledTiles)
        {
            tile.GetComponent<Tile>().movementSpeed += speedIncrementAmount;
        }
    }

    private void InitializeStartingTiles()
    {
        for (var i = 0; i < numberOfStartingTiles; i++) ActivateTileFromPool();
    }

    private void ActivateTileFromPool()
    {
        int randomNumber;
        var count = 0;

        do
        {
            randomNumber = Random.Range(0, _pooledTiles.Count);
            count++;
        } while (_pooledTiles[randomNumber].activeInHierarchy || count >= _pooledTiles.Count - 1);

        var placeableTile = _pooledTiles[randomNumber];
        Vector3 position = Vector3.zero;

        if (_activeTiles.Count > 0) CalculateTilePositionOffset();

        position += tilePositionOffset;
        placeableTile.transform.position = position;

        placeableTile.SetActive(true);
        _activeTiles.Add(placeableTile);
    }

    private void CalculateTilePositionOffset()
    {
        var lastPlacedTile = _activeTiles.Last();

        if (_activeTiles.Count > 0)
        {
            var lastPlacedTilePosition = lastPlacedTile.transform.position;
            var tileLength = new Vector3(0, 0, lastPlacedTile.GetComponent<Renderer>().bounds.size.z);
            tilePositionOffset = lastPlacedTilePosition + tileLength;
        }
        else
        {
            tilePositionOffset += Vector3.forward * lastPlacedTile.GetComponent<Renderer>().bounds.size.z;
        }
    }

    private void PopulateTilePool()
    {
        var tilePrefabIndex = 0;

        for (var i = 0; i < poolSize; i++)
        {
            var parent = transform;
            var tile = Instantiate(tilePrefabs[tilePrefabIndex], parent.forward + poolSpawnOffset,
                Quaternion.identity, parent);
            tile.SetActive(false);
            _pooledTiles.Add(tile);

            if (tilePrefabIndex < tilePrefabs.Count - 1)
                tilePrefabIndex++;
            else
                tilePrefabIndex = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Tile>()) DeactivateTile(other.gameObject);
    }

    private void DeactivateTile(GameObject tile)
    {
        if (_activeTiles.Contains(tile))
        {
            _activeTiles.Remove(tile);
            tile.SetActive(false);
            ActivateTileFromPool();
        }
    }
}