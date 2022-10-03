using LudumDare51.Enemy;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LudumDare51
{
    public class EaterManager : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _progText;

        [SerializeField]
        private AudioSource _bgm;

        [SerializeField]
        private AudioClip[] _clips;

        public static EaterManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private readonly Dictionary<Eater, int> _nbEaten = new();

        public void Register(Eater eater)
        {
            _nbEaten.Add(eater, 0);
        }

        private int oldLevel = 0;
        public void AddEat(Eater eater)
        {
            _nbEaten[eater]++;

            var average = _nbEaten.Values.Sum() / _nbEaten.Count;
            var maxLevel = 0;
            foreach (var e in _nbEaten.Keys)
            {
                var level = _nbEaten[e] - average;
                var lR = level / 10;
                if (lR >= 5)
                {
                    SceneManager.LoadScene("GameOver");
                    return;
                }
                if (lR > maxLevel) maxLevel = lR;
                e.UpdateSprite(level < 0 ? 0 : level);
            }
            UpdateNachoverflowValue(-1);

            if (maxLevel != oldLevel)
            {
                oldLevel = maxLevel;
                var pos = _bgm.time;
                _bgm.clip = _clips[oldLevel];
                _bgm.Play();
                _bgm.time = pos;
            }
        }

        public void UpdateNachoverflowValue(int modifier)
        {
            var length = GameObject.FindGameObjectsWithTag("Enemy").Length + modifier;
            _progText.text = $"Nachoverflow: {length}%";
            if (length >= 100)
            {
                SceneManager.LoadScene("GameOver");
            }
        }
    }
}
