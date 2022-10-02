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
        private Node[] _spawnPoints;

        private int[] _inventory;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            StartCoroutine(NextWave());
            _inventory = new int[_buttonInfo.Length];
            _inventory[0] = 1;
            UpdateInventory();
        }

        private IEnumerator Spawn()
        {
            for (int i = 0; i < 10f; i++)
            {
                var targetInfo = _enemyInfo[Random.Range(0, _enemyInfo.Length)];
                var go = Instantiate(_enemyPrefab, transform);
                var offset = (Vector2)(Random.insideUnitSphere * .25f);
                var spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
                go.transform.position = (Vector2)(spawnPoint.transform.position) + offset;
                var enemy = go.GetComponent<EnemyAI>();
                enemy.Info = targetInfo;
                enemy.NextNode = spawnPoint.NextNode;
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
