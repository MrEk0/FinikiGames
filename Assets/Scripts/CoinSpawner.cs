using System;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] 
    private Coin coinPrefab;
    
    [SerializeField] 
    private float startSpawnTime = 1f;

    [SerializeField] 
    private float minSpawnTime = 0.1f;

    [SerializeField] 
    private float coinSpawnTimeRate = 1f;
    
    [SerializeField] 
    private bool collectionCheck = true;
    
    [SerializeField] 
    private int defaultCapacity = 20;
    
    [SerializeField] 
    private int maxSize = 100;

    [SerializeField] 
    private Transform[] spawnPoints = Array.Empty<Transform>();
    
    private IObjectPool<Coin> _objectPool;
    
    private float _spawnTime;
    private float _spawnTimer;
    
    private void Awake()
    {
        _objectPool = new ObjectPool<Coin>(CreateProjectile, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, collectionCheck, defaultCapacity, maxSize);
    }
    
    private void Start()
    {
        _spawnTime = startSpawnTime;
        _spawnTimer = startSpawnTime;
    }

    private void Update()
    {
        _spawnTimer += Time.deltaTime;

        _spawnTime = Mathf.Clamp(_spawnTime - Time.deltaTime * coinSpawnTimeRate, minSpawnTime, startSpawnTime);

        if (_spawnTimer <= _spawnTime)
            return;
        
        var spawner = spawnPoints[Random.Range(0, spawnPoints.Length)];
        
        var obj = _objectPool.Get();
        obj.transform.position = spawner.position;
       
        obj.Initialize();

        _spawnTimer = 0;
    }

    public void Restart()
    {
        _spawnTime = startSpawnTime;
        _spawnTimer = startSpawnTime;
    }

    private Coin CreateProjectile()
    {
        var coin = Instantiate(coinPrefab, transform);
        coin.ObjectPool = _objectPool;
        return coin;
    }
  
    private void OnReleaseToPool(Coin pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }
    
    private void OnGetFromPool(Coin pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }
   
    private void OnDestroyPooledObject(Coin pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }
}
