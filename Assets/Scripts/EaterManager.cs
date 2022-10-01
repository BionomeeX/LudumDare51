using LudumDare51.Enemy;
using System.Collections.Generic;
using System.Linq;

namespace LudumDare51
{
    public class EaterManager
    {
        private static EaterManager _instance;

        public static EaterManager Instance
        {
            get
            {
                _instance ??= new();
                return _instance;
            }
        }

        private readonly Dictionary<Eater, int> _nbEaten = new();

        public void AddEat(Eater eater)
        {
            if (!_nbEaten.ContainsKey(eater))
            {
                _nbEaten.Add(eater, 0);
            }
            _nbEaten[eater]++;

            var average = _nbEaten.Values.Sum() / _nbEaten.Count;
            foreach (var e in _nbEaten.Keys)
            {
                var level = _nbEaten[e] - average;
                e.UpdateSprite(level < 0 ? 0 : level);
            }
        }
    }
}
