using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    [SerializeField] private List<Transform> _cloudPrefabs = new List<Transform>();
    [SerializeField] private Transform _cloudsParent;
    [SerializeField] float _verticalMin = 3f;
    [SerializeField] float _verticalMax = 6f;
    [SerializeField] int _cloudPoolSize = 6;
    [SerializeField] int _initialCloudDensity = 4;
    [SerializeField] float _timeBetweenCloudSpawns = 5f;
    [SerializeField] private float _speed = .25f;
    private List<Transform> _cloudPool = new List<Transform>();
    private Camera _cam;
    private Vector3 _viewportRight;
    private Vector3 _viewportLeft;
    private float _horizontalMin;
    private float _horizontalMax;

    private void Start()
    {
        _cam = Camera.main;
        
        GetSpawnAndDespawnPoints();

        GenerateCloudPool();

        SetInitialCloudPositions();

        StartCoroutine(SpawnClouds());

        StartCoroutine(DespawnClouds());
    }

    private void Update()
    {
        GetSpawnAndDespawnPoints();

        foreach (var cloud in _cloudPool)
        {
            cloud.Translate(Vector3.left * Time.deltaTime * _speed);
        }
    }

    private void GetSpawnAndDespawnPoints()
    {
        _viewportRight = _cam.ViewportToWorldPoint(new Vector3(1, 1, _cam.nearClipPlane));
        _viewportLeft = _cam.ViewportToWorldPoint(new Vector3(0, 1, _cam.nearClipPlane));
        _horizontalMax = _viewportRight.x;
        _horizontalMin = _viewportLeft.x;
    }

    private IEnumerator SpawnClouds()
    {
        int index = 0;
        while (true)
        {
            if (index < _cloudPool.Count)
            {
                var cloud = _cloudPool[index];
                if (!cloud.gameObject.activeInHierarchy)
                {
                    cloud.gameObject.SetActive(true);
                    cloud.transform.position = new Vector3(_horizontalMax + cloud.localScale.x, Random.Range(_verticalMin, _verticalMax));
                }
                index++;
            }
            else
            {
                index = 0;
            }
            yield return new WaitForSeconds(_timeBetweenCloudSpawns);
        }
    }

    private IEnumerator DespawnClouds()
    {
        while (true)
        {
            foreach (var cloud in _cloudPool)
            {
                if (cloud.transform.position.x < _horizontalMin - cloud.localScale.x)
                {
                    cloud.gameObject.SetActive(false);
                }
            }
            yield return null;
        }
    }

    private void GenerateCloudPool()
    {
        int randomIndex;
        for (int i = 0; i < _cloudPoolSize; i++)
        {
            randomIndex = Random.Range(0, _cloudPrefabs.Count);
            var cloud = Instantiate(_cloudPrefabs[randomIndex], _cloudsParent);
            _cloudPool.Add(cloud);
        }
    }

    private void SetInitialCloudPositions()
    {
        for (int i = 0; i < _initialCloudDensity; i++)
        {
            var cloud = _cloudPool[i];
            cloud.gameObject.SetActive(true);
            cloud.transform.position = new Vector3(Random.Range(_viewportLeft.x, _viewportRight.x + cloud.localScale.x), Random.Range(_verticalMin, _verticalMax));
        }
    }
}