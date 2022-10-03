using System.Collections;
using TMPro;
using UnityEngine;

namespace LudumDare51.MiniGame
{
    public class Spawner : MonoBehaviour
    {
        public static Spawner Instance { private set; get; }

        [SerializeField] private GameObject _prefab;
        [SerializeField] private TMP_Text _text;

        private void Awake()
        {
            Instance = this;
            StartCoroutine(Spawn());
        }


        private int _kill;
        public void AddKill()
        {
            _kill++;
            _text.text = $"Score: {_kill}";
        }

        public void Loose()
        {
            _text.text = "Oh nion!";
        }

        private IEnumerator Spawn()
        {
            yield return new WaitForSeconds(2f);
            _text.text = "3";
            yield return new WaitForSeconds(1f);
            _text.text = "2";
            yield return new WaitForSeconds(2f);
            _text.text = "1";
            yield return new WaitForSeconds(2f);
            _text.text = "Score: 0";
            float delay = 3f;
            while (true)
            {
                Instantiate(_prefab, new Vector2(transform.position.x, Random.Range(-4f, 4f)), Quaternion.identity);
                yield return new WaitForSeconds(delay);
                if (delay > .5f)
                {
                    delay -= .1f;
                }
            }
        }
    }
}
