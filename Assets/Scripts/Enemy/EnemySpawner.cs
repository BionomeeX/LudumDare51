using System.Collections;
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

            foreach (var spawner in GameObject.FindGameObjectsWithTag("Spawner").Select(x => x.GetComponent<Node>()))
            {
                foreach (var salve in spawner.Salves.OrderBy(x => Random.Range(0f, 1f))) // Might as well randomize the first one
                {
                    if ((salve.start_round < 1 || salve.start_round <= round) && (salve.end_round < 1 || salve.end_round >= round))
                    {
                        foreach (var group in salve.groups)
                        {
                            for (int i = 0; i < group.quantity + (round - 1); i++)
                            {
                                if (nbSpawned < 50) // If somehow the player reach that, let's not just kill him right away
                                {
                                    var go = Instantiate(_enemyPrefab, transform);
                                    var offset = (Vector2)(Random.insideUnitSphere * .25f);
                                    go.transform.position = (Vector2)(spawner.transform.position) + offset;
                                    var enemy = go.GetComponent<EnemyAI>();
                                    enemy.Info = group.info;
                                    enemy.NextNode = spawner.NextNode;
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
                yield return Spawn();
                round++;
                yield return new WaitForSeconds(10f);
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
