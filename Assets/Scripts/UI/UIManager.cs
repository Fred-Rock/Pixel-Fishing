using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    #region Fields
    [Header("Main Menu")]
    [SerializeField] private float _mainMenuScaleTime = 1f;
    [SerializeField] private RectTransform _menuParentRect;
    [SerializeField] private RectTransform _menuBackgroundRect;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _howToPlayButton;
    [SerializeField] private Button _creditsButton;
    [SerializeField] private RectTransform _howToPlayRect;
    [SerializeField] private RectTransform _creditsRect;
    [SerializeField] private float _dialogueBoxScaleTime = .25f;
    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI _reelText;
    [SerializeField] private Button _collectionButton;
    [SerializeField] private Button _rodButton;
    [SerializeField] private RectTransform _caughtFishRect;
    [SerializeField] private TextMeshProUGUI _caughtFishText;
    [SerializeField] private Image _caughtFishIcon;
    [SerializeField] private float _caughtFishFadeTime = 5f;
    [SerializeField] private float _caughtFishRectScaleTime = .25f;
    [Header("Fish Collection")]
    [SerializeField] private RectTransform _collectionMenuRect;
    [SerializeField] private RectTransform _fishCollectionRect;
    [SerializeField] private float _collectionMenuScaleTime = .25f;
    [SerializeField] private UICollectionItem _collectionItemPrefab;
    private List<UICollectionItem> _collectionItemPrefabPool = new List<UICollectionItem>();
    [SerializeField] private Color _lockedItemColor;
    [SerializeField] private string _lockedItemPlaceholder;
    [Header("Fish UI Data")]
    [SerializeField] private List<FishData> _fishSourceData = new List<FishData>();
    private Dictionary<FishType, FishData> _fishLookupDict = new Dictionary<FishType, FishData>();
    private List<FishData> _caughtList = new List<FishData>();
    #endregion

    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        EventHandler.FishCaughtEvent += ShowFishCaughtDialogue;
        EventHandler.FishCaughtEvent += UnlockInCollection;
        EventHandler.ReelEvent += ShowReelAlert;
        EventHandler.HUDActiveEvent += ToggleHUDActive;
    }

    private void OnDisable()
    {
        EventHandler.FishCaughtEvent -= ShowFishCaughtDialogue;
        EventHandler.FishCaughtEvent -= UnlockInCollection;
        EventHandler.ReelEvent -= ShowReelAlert;
        EventHandler.HUDActiveEvent -= ToggleHUDActive;
    }

    private void Init()
    {
        ActivateMainMenu();
        DeactivateHUD();
        InitializeCollection();
        DeactivateCollectionMenu();
        GenerateFishLookupDict();
    }

    private void GenerateFishLookupDict()
    {
        foreach (FishData data in _fishSourceData)
        {
            _fishLookupDict.Add(data._fishType, data);
        }
    }

    #region Buttons
    public void OnStartButton()
    {
        StartGame();
        EventHandler.CallMenuButtonClickedEvent();
    }

    public void OnHowToPlayButton()
    {
        ToggleHowToPlayRect();
        EventHandler.CallMenuButtonClickedEvent();
    }

    public void OnCreditsButton()
    {
        ToggleCreditsRect();
        EventHandler.CallMenuButtonClickedEvent();
    }

    public void OnCollectionButton()
    {
        ShowCollectionMenu();

        EventHandler.CallMenuButtonClickedEvent();
    }

    public void OnCastButton()
    {
        EventHandler.CallPlayerCastAttemptEvent();
    }

    public void OnReelButton()
    {
        EventHandler.CallPlayerReelAttemptEvent();
    }

    public void OnCloseCollectionMenuButton()
    {
        HideCollectionMenu();

        EventHandler.CallMenuButtonClickedEvent();
    }

    public void OnQuitButton()
    {
        Application.OpenURL("https://www.adamfrederick.net/");
    }
    #endregion

    #region Menus
    private void ShowCollectionMenu()
    {
        ScaleUpRect(_collectionMenuRect, _collectionMenuScaleTime);
    }

    private void HideCollectionMenu()
    {
        ScaleDownRect(_collectionMenuRect, _collectionMenuScaleTime);
    }

    private void ActivateMainMenu()
    {
        _menuParentRect.gameObject.SetActive(true);
        
        _titleText.gameObject.SetActive(true);
        _startButton.gameObject.SetActive(true);
        
        //_howToPlayButton.gameObject.SetActive(true);
        //_howToPlayRect.gameObject.SetActive(true);
        //_howToPlayRect.localScale = new Vector3(0, 0);
        
        _creditsButton.gameObject.SetActive(true);
        _creditsRect.gameObject.SetActive(true);
        _creditsRect.localScale = new Vector3(0, 0);
    }

    private void DeactivateMainMenu()
    {
        _menuParentRect.gameObject.SetActive(false);
    }

    private void ActivateCollectionMenu()
    {
        _collectionMenuRect.gameObject.SetActive(true);
        _collectionMenuRect.localScale = new Vector3(0, 0);
        HideCollectionMenu();
    }

    private void DeactivateCollectionMenu()
    {
        _collectionMenuRect.gameObject.SetActive(false);
    }
    #endregion

    #region Main menu dialogue box toggles
    private void ToggleHowToPlayRect()
    {
        if (_howToPlayRect.localScale.x == 0)
        {
            ScaleUpRect(_howToPlayRect, _dialogueBoxScaleTime);
        }
        else
        {
            ScaleDownRect(_howToPlayRect, _dialogueBoxScaleTime);
        }
    }

    private void ToggleCreditsRect()
    {
        if (_creditsRect.localScale.x == 0)
        {
            ScaleUpRect(_creditsRect, _dialogueBoxScaleTime);
        }
        else
        {
            ScaleDownRect(_creditsRect, _dialogueBoxScaleTime);
        }
    }
    #endregion

    #region Start game coroutines
    private void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }
    
    private IEnumerator StartGameCoroutine()
    {
        StartCoroutine(AnimateDismissMainMenu());
        yield return null;
    }

    private IEnumerator AnimateDismissMainMenu()
    {
        ScaleDownRect(_menuBackgroundRect, _mainMenuScaleTime);
        _backgroundImage.DOFade(0, _mainMenuScaleTime);
        yield return new WaitForSeconds(_mainMenuScaleTime);
        StartCoroutine(DeactivateMainMenuAndStartGame());
    }

    private IEnumerator DeactivateMainMenuAndStartGame()
    {
        Debug.Log($"Starting game 2 seconds...");
        DeactivateMainMenu();
        yield return new WaitForSeconds(2);
        EventHandler.CallStartGameEvent();
    }
    #endregion

    #region Game HUD
    public void ToggleHUDActive(bool isActive)
    {
        _caughtFishRect.gameObject.SetActive(true);
        _caughtFishRect.localScale = new Vector3(0, 0);

        if (isActive)
        {
            _collectionButton.gameObject.SetActive(true);
            _rodButton.gameObject.SetActive(true);

            ActivateCollectionMenu();
        }
        else
        {
            _collectionButton.gameObject.SetActive(false);
            _rodButton.gameObject.SetActive(false);

            DeactivateCollectionMenu();
        }
    }

    public void ActivateHUD()
    {
        _collectionButton.gameObject.SetActive(true);
        _rodButton.gameObject.SetActive(true);

        _caughtFishRect.gameObject.SetActive(true);
        _caughtFishRect.localScale = new Vector3(0, 0);

        ActivateCollectionMenu();
    }

    public void DeactivateHUD()
    {
        _collectionButton.gameObject.SetActive(false);
        _rodButton.gameObject.SetActive(false);
        
        _caughtFishRect.gameObject.SetActive(false);
        _reelText.gameObject.SetActive(false);

        DeactivateCollectionMenu();
    }

    public void ShowReelAlert(float reelTime)
    {
        _reelText.gameObject.SetActive(true);
        StartCoroutine(FlashReelText(reelTime));
    }

    private IEnumerator FlashReelText(float flashTime)
    {
        //Tween flash = _reelText.DOColor(new Color(4, 5, 4), flashTime);
        //yield return flash.WaitForCompletion();
        yield return new WaitForSeconds(flashTime);
        _reelText.gameObject.SetActive(false);
    }

    private void HideReelAlert()
    {
        if (_reelText.enabled)
        {
            _reelText.gameObject.SetActive(false);
        }
    }

    public void ShowFishCaughtDialogue(Fish fish)
    {
        HideReelAlert();
        ScaleUpRect(_caughtFishRect, _caughtFishRectScaleTime);

        FishType fishType = fish.FishType;
        FishData fishData;
        _fishLookupDict.TryGetValue(fishType, out fishData);
        
        _caughtFishText.text = fishData._fishType.ToString();
        _caughtFishIcon.sprite = fishData._icon;
        StartCoroutine(HideFishCaughtDialogue());
    }

    private IEnumerator HideFishCaughtDialogue()
    {
        yield return new WaitForSeconds(_caughtFishFadeTime + _caughtFishRectScaleTime);
        ScaleDownRect(_caughtFishRect, _caughtFishRectScaleTime);
        //_caughtFishRect.gameObject.SetActive(false);
    }
    #endregion

    #region Collection menu
    private void InitializeCollection()
    {
        for (int i = 0; i < _fishSourceData.Count; i++)
        {
            UICollectionItem collectionItem = Instantiate(_collectionItemPrefab, _fishCollectionRect) as UICollectionItem;
            FishData itemData = _fishSourceData[i];
            collectionItem.fishData = itemData;
            collectionItem.itemText.text = _lockedItemPlaceholder;
            collectionItem.itemIcon.sprite = itemData._icon;
            collectionItem.itemIcon.color = _lockedItemColor;
            
            _collectionItemPrefabPool.Add(collectionItem);
        }
    }

    public void UnlockInCollection(Fish fish)
    {
        FishType fishType = fish.FishType;
        FishData fishToUnlock;
        _fishLookupDict.TryGetValue(fishType, out fishToUnlock);
        _caughtList.Add(fishToUnlock);

        RefreshCollection();
    }

    private void RefreshCollection()
    {
        foreach (UICollectionItem item in _collectionItemPrefabPool)
        {
            FishData fishData = item.fishData;

            if (!_caughtList.Contains(fishData))
            {
                item.itemText.text = _lockedItemPlaceholder;
                item.itemIcon.color = _lockedItemColor;
            }
            else if (_caughtList.Contains(fishData))
            {
                item.itemText.text = fishData._fishType.ToString();
                item.itemIcon.color = Color.white;
            }
        }
    }
    #endregion

    #region UI effects
    private void ScaleUpRect(RectTransform rect, float scaleTime)
    {
        rect.localScale = new Vector3(0, 0);
        rect.DOScale(1, scaleTime);
    }

    private void ScaleDownRect(RectTransform rect, float scaleTime)
    {
        rect.DOScale(0, scaleTime);
    }
    #endregion
}
