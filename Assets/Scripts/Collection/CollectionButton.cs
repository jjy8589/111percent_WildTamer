using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionButton : MonoBehaviour
{
    [SerializeField] private Button _collectionButton;
    [SerializeField] private GameObject _collectionPopup;

    private void OnEnable()
    {
        _collectionButton.onClick.AddListener(OpenCollection);
    }

    private void OnDisable()
    {
        _collectionButton.onClick.RemoveListener(OpenCollection);
    }

    private void OpenCollection()
    {
        _collectionPopup.SetActive(true);
    }
}
