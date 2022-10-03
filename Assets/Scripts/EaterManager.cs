using LudumDare51.Enemy;
using LudumDare51.Translation;
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
        private TMP_Text _progText, _scoreText;

        [SerializeField]
        private AudioSource _bgm;

        [SerializeField]
        private AudioClip[] _clips;

        public static EaterManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void FixedUpdate()
        {
            score += (50 - maxDiff) * Time.deltaTime;
            _scoreText.text = $"{Translate.Instance.Tr("score")}: {Mathf.CeilToInt(score)}";
        }

        private float score;

        private readonly Dictionary<Eater, int> _nbEaten = new();

        public void Register(Eater eater)
        {
            _nbEaten.Add(eater, 0);
        }

        private int oldLevel = 0;
        private int maxDiff = 0;
        public void AddEat(Eater eater)
        {
            _nbEaten[eater]++;

            var average = _nbEaten.Values.Sum() / _nbEaten.Count;
            var maxLevel = 0;
            maxDiff = 0;
            foreach (var e in _nbEaten.Keys)
            {
                var level = _nbEaten[e] - average;
                if (level > maxDiff)
                {
                    maxDiff = level;
                }
                var lR = level / 10;
                if (lR >= 5)
                {
                    DataKeeper.Instance.FinalScore = Mathf.CeilToInt(score);
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
                DataKeeper.Instance.FinalScore = Mathf.CeilToInt(score);
                SceneManager.LoadScene("GameOver");
            }
        }
    }
}
