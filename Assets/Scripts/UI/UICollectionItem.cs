using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICollectionItem : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI itemText;
    [SerializeField] public Image itemIcon;
    public FishData fishData;
}