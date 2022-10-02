using LudumDare51.SO;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
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

        private int[] _inventory;

        private int round;

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
                var index = Random.Range(0, OnClick.Instance.Info.Length);
                var randButton = OnClick.Instance.Info[index];
                go.GetComponent<ButtonInit>().Init(() =>
                {
                    _inventory[index]++;
                    _itemPick.SetActive(false);
                    UpdateInventory();
                }, randButton.Sprite);
            }

            foreach (var spawner in GameObject.FindGameObjectsWithTag("Spawner").Select(x => x.GetComponent<Node>()))
            {
                foreach (var salve in spawner.Salves)
                {
                    if ((salve.start_round < 1 || salve.start_round <= round) && (salve.end_round < 1 || salve.end_round >= round))
                    {
                        foreach (var group in salve.groups)
                        {
                            for (int i = 0; i < group.quantity; i++)
                            {
                                var go = Instantiate(_enemyPrefab, transform);
                                var offset = (Vector2)(Random.insideUnitSphere * .25f);
                                go.transform.position = (Vector2)(spawner.transform.position) + offset;
                                var enemy = go.GetComponent<EnemyAI>();
                                enemy.Info = group.info;
                                enemy.NextNode = spawner.NextNode;
                                enemy.Offset = offset;
                                EaterManager.Instance.UpdateNachoverflowValue(0);
                                yield return new WaitForSeconds(Random.Range(.1f, .3f));
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

        private void UpdateInventory()
        {
            for (int i = 0; i < _inventory.Length; i++)
            {
                _buttonInfo[i].gameObject.SetActive(_inventory[i] > 0);
            }
        }
    }
}
