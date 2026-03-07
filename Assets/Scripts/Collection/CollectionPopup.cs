using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionPopup : MonoBehaviour
{
    [SerializeField] private GameObject _collectionItemPrefab;
    [SerializeField] private Transform _itemParent;

    [SerializeField] private Button _closeButton;

    private List<CollectionItemUI> _collectionItemList = new();

    private void Awake()
    {
        _collectionItemList = new();

        CreateCollectionItem();
    }

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(() => gameObject.SetActive(false));

        UpdateCollectionUI();
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveAllListeners();
    }

    private void CreateCollectionItem()
    {
        foreach(var data in DataManager.Instance.MonsterDataList)
        {
            var obj = Instantiate(_collectionItemPrefab, _itemParent);
            var item = obj.GetComponent<CollectionItemUI>();
            item.SetUI(
                new CollectionData(
                     data.MonsterId,
                     data.MonsterName,
                     data.MaxHeart,
                     DataManager.Instance.IsMonsterFound(data.MonsterId),
                     DataManager.Instance.IsMonsterAlly(data.MonsterId)
                 )
            );

            _collectionItemList.Add(item);
        }
    }

    private void UpdateCollectionUI()
    {
        foreach (var item in _collectionItemList)
        {
            item.Refresh();
        }
    }
}
