using LudumDare51.SO;
using System.Collections;
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
        private EnemyInfo[] _enemyInfo;
        [SerializeField]
        private Button[] _buttonInfo;

        [SerializeField]
        private Transform _spawnPoint;

        [SerializeField]
        private Node _firstNode;

        [SerializeField]
        private GameObject _itemPick, _itemPickContainer;

        [SerializeField]
        private GameObject _itemPrefab;

        private int[] _inventory;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            StartCoroutine(NextWave());
            _inventory = new int[_buttonInfo.Length];
            for (int i = 0; i < _buttonInfo.Length; i++)
            {
                _inventory[i] = 1;
            }
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

            for (int i = 0; i < 10f; i++)
            {
                var targetInfo = _enemyInfo[Random.Range(0, _enemyInfo.Length)];
                var go = Instantiate(_enemyPrefab, transform);
                var offset = (Vector2)(Random.insideUnitSphere * .25f);
                go.transform.position = (Vector2)(_spawnPoint.position) + offset;
                var enemy = go.GetComponent<EnemyAI>();
                enemy.Info = targetInfo;
                enemy.NextNode = _firstNode;
                enemy.Offset = offset;
                EaterManager.Instance.UpdateNachoverflowValue(0);
                yield return new WaitForSeconds(Random.Range(.1f, .3f));
            }
        }

        private IEnumerator NextWave()
        {
            while (true)
            {
                yield return Spawn();
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
