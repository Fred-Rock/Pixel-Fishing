using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    #region Fields
    [SerializeField] private FishType _fishType;
    [SerializeField] private float _leftBounds;
    [SerializeField] private float _rightBounds;
    [SerializeField] private float _topBounds;
    [SerializeField] private float _bottomBounds;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _waitTime = 4f;
    [SerializeField] private float _reelTime = 3f;
    #endregion

    #region Properties
    public FishType FishType { get { return _fishType; } private set { } }
    public float ReelTime { get { return _reelTime; } private set { } }
    #endregion
    private void Start()
    {
        StartCoroutine(MoveTowardRandomPoint());
    }

    private IEnumerator MoveTowardRandomPoint()
    {
        float xRandom = Random.Range(_leftBounds, _rightBounds);
        float yRandom = Random.Range(_topBounds, _bottomBounds);
        Vector3 randomPoint = new Vector3(xRandom, yRandom);
        if (randomPoint.x < transform.position.x)
        {
            // face left
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y);
        }
        else
        {
            // face right
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        
        while (transform.position != randomPoint)
        {
            transform.position = Vector3.MoveTowards(transform.position, randomPoint, _speed * Time.deltaTime);
            
            yield return null;
        }
        StartCoroutine(WaitAtPoint());
    }

    private IEnumerator WaitAtPoint()
    {
        yield return new WaitForSeconds(_waitTime);
        StartCoroutine(MoveTowardRandomPoint());
    }
}
