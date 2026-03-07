using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectionItemUI : MonoBehaviour
{
    [SerializeField] private GameObject _isAllyObject;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _heartText;
    [SerializeField] private GameObject _unlockObject;

    private string _monsterId;

    public void SetUI(CollectionData data)
    {
        _monsterId = data.MonsterID;

        _isAllyObject.SetActive(data.HasBeenAlly);

        _nameText.text = data.MonsterName;
        _heartText.text = data.MaxHeart.ToString();

        _unlockObject.SetActive(!data.IsFound);
    }

    public void Refresh()
    {
        bool isFound = DataManager.Instance.IsMonsterFound(_monsterId);
        bool isAlly = DataManager.Instance.IsMonsterAlly(_monsterId);

        _isAllyObject.SetActive(isAlly);
        _unlockObject.SetActive(!isFound);
    }
}
