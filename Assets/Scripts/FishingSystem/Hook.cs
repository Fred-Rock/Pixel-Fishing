using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] private Transform _searchArea;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Collider2D _collider2d;
    [SerializeField] private FishingSystem _fishingSystem;
    [SerializeField] private SpriteMask _spriteMask;
    private bool _isHookActive;
    private Fish _hookedFish;
    private float _hookDelay;
    //private IEnumerator _hookDelayCoroutine;

    public Transform SearchArea { get { return _searchArea; } private set { } }
    public Fish HookedFish { get { return _hookedFish; } set { _hookedFish = value; } }

    private void Start()
    {
        _hookDelay = _fishingSystem.HookDelay;
        //_hookDelayCoroutine = ActivateHookColliderAfterDelay(_hookDelay);
    }
    
    public void ActivateHook(bool activate)
    {
        _isHookActive = activate;
        if (_isHookActive)
        {
            _spriteRenderer.enabled = true;
            _spriteMask.enabled = false;
            _isHookActive = false;
            _collider2d.enabled = true; //
            //StartCoroutine(_hookDelayCoroutine);
        }
        else if (!_isHookActive)
        {
            _spriteRenderer.enabled = false;
            _collider2d.enabled = false;
            _spriteMask.enabled = true;
            //StopCoroutine(_hookDelayCoroutine);
        }
    }

    /*
    private IEnumerator ActivateHookColliderAfterDelay(float delay)
    {
        float hookdelay = delay;
        yield return new WaitForSeconds(hookdelay);
        _collider2d.enabled = true;
    }
    */
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _hookedFish = collision.GetComponent<Fish>();
    }
}
