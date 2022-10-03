using LudumDare51.SO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LudumDare51.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        public static EnemySpawner Instance { private set; get; }

        [SerializeField]
        private GameObject _enemyPrefab;

        [SerializeField]
        private Button[] _buttonInfo;

        [SerializeField]
        private GameObject _itemPick, _itemPickContainer;

        [SerializeField]
        private GameObject _itemPrefab;

        [SerializeField]
        private Sprite _trashIcon, _hatIcon;

        private int[] _inventory;

        [SerializeField]
        private TMP_Text _textTimer;

        private int round;
        private int _nbDelete = 0;
        private int _nbHat = 0;

        private void Awake()
        {
            Instance = this;
            round = 1;
        }

        private void Start()
        {
            StartCoroutine(NextWave());
            _inventory = new int[_buttonInfo.Length];
            _inventory[0] = 3;
            UpdateInventory();
        }

        class IntWrapper // WTF C#?
        {
            public IntWrapper(int a)
            {
                Int = a;
            }

            public int Int;
        }

        private IEnumerator Spawn()
        {
            _itemPick.SetActive(true);
            for (int i = 0; i < _itemPickContainer.transform.childCount; i++) Destroy(_itemPickContainer.transform.GetChild(i).gameObject);

            for (int i = 0; i < 3; i++)
            {
                var go = Instantiate(_itemPrefab, _itemPickContainer.transform);
                var index = Random.Range(-2, OnClick.Instance.Info.Length);
                if (index >= 0)
                {
                    var randButton = OnClick.Instance.Info[index];
                    go.GetComponent<ButtonInit>().Init(() =>
                    {
                        _inventory[index]++;
                        _itemPick.SetActive(false);
                        UpdateInventory();
                    }, randButton.Sprite, randButton.WeaponSprite);
                }
                else if (index == -1)
                {
                    go.GetComponent<ButtonInit>().Init(() =>
                    {
                        _nbDelete++;
                        _itemPick.SetActive(false);
                        UpdateInventory();
                    }, _trashIcon, null);
                }
                else if (index == -2)
                {
                    go.GetComponent<ButtonInit>().Init(() =>
                    {
                        _nbHat++;
                        _itemPick.SetActive(false);
                        UpdateInventory();
                    }, _hatIcon, null);
                }
            }

            var nbSpawned = 0;

            Dictionary<Node, Dictionary<EnemyInfo, IntWrapper>> _spawnRemainings
                = GameObject.FindGameObjectsWithTag("Spawner").Select(x => x.GetComponent<Node>()).ToDictionary(x => x,
                x => x.Salves.Where(salve => (salve.start_round < 1 || salve.start_round <= round) && (salve.end_round < 1 || salve.end_round >= round))
                .Select(y => y.groups.Select(z => (z._info, z.quantity))).SelectMany(y => y)
                .OrderBy(x => Random.Range(0f, 1f)).ToDictionary(y => y._info, y => {
                    var target = y.quantity * (1 + round / 6);
                    var iW = new IntWrapper(Random.Range(target / 2, target));
                    return iW;
                    }));

            while (_spawnRemainings.Values.Sum(y => y.Values.Sum(x => x.Int)) > 0)
            {
                foreach (var a in _spawnRemainings.Keys)
                {
                    var elem = _spawnRemainings[a];
                    foreach (var type in elem.Keys)
                    {
                        if (elem[type].Int > 0)
                        {
                            elem[type].Int--;
                            if (nbSpawned < 50) // If somehow the player reach that, let's not just kill him right away
                            {
                                var dice = Random.Range(0, 100);
                                if (dice < 50)
                                {
                                    var go = Instantiate(_enemyPrefab, transform);
                                    var offset = (Vector2)(Random.insideUnitSphere * .25f);
                                    go.transform.position = (Vector2)(a.transform.position) + offset;
                                    var enemy = go.GetComponent<EnemyAI>();
                                    enemy.Info = type;
                                    enemy.NextNode = a.NextNode;
                                    enemy.Offset = offset;
                                    EaterManager.Instance.UpdateNachoverflowValue(0);
                                    nbSpawned++;
                                    yield return new WaitForSeconds(Random.Range(.1f, .3f));
                                }
                            }
                        }
                    }
                }
            }
        }

        private IEnumerator NextWave()
        {
            while (true)
            {
                StartCoroutine(Spawn());
                round++;
                for (int i = 10; i >= 0; i--)
                {
                    _textTimer.text = $"{i}";
                    yield return new WaitForSeconds(1);
                }
                _textTimer.text = "0";
            }
        }

        public void RemoveTower(int index)
        {
            _inventory[index]--;
            UpdateInventory();
        }

        public void RemoveDelete()
        {
            _nbDelete--;
            UpdateInventory();
        }

        public void RemoveHat()
        {
            _nbHat--;
            UpdateInventory();
        }

        private void UpdateInventory()
        {
            for (int i = 0; i < _inventory.Length; i++)
            {
                _buttonInfo[i].gameObject.SetActive(_inventory[i] > 0);
                _buttonInfo[i].GetComponentInChildren<TMP_Text>().text = $"{_inventory[i]}";
            }
            var iM = OnClick.Instance.Info.Length;
            _buttonInfo[iM].gameObject.SetActive(_nbDelete > 0);
            if (_nbDelete > 0)
            {
                _buttonInfo[iM].GetComponentInChildren<TMP_Text>().text = $"{_nbDelete}";
            }
            _buttonInfo[iM + 1].gameObject.SetActive(_nbHat > 0);
            if (_nbHat > 0)
            {
                _buttonInfo[iM + 1].GetComponentInChildren<TMP_Text>().text = $"{_nbHat}";
            }
        }
    }
}
