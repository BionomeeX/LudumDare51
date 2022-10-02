using LudumDare51.Enemy;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace LudumDare51
{
    public class EaterManager : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _progText;

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

        public void AddEat(Eater eater)
        {
            _nbEaten[eater]++;

            var average = _nbEaten.Values.Sum() / _nbEaten.Count;
            foreach (var e in _nbEaten.Keys)
            {
                var level = _nbEaten[e] - average;
                e.UpdateSprite(level < 0 ? 0 : level);
            }
            UpdateNachoverflowValue(-1);
        }

        public void UpdateNachoverflowValue(int modifier)
        {
            _progText.text = $"Nachoverflow: {GameObject.FindGameObjectsWithTag("Enemy").Length + modifier}%";
        }
    }
}
