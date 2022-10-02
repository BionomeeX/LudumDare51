using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LudumDare51.SO;
using UnityEngine;

namespace Assets.Scripts.Enemy.Salve
{

    [Serializable]
    public class Salve
    {
        [SerializeField]
        public int _start_round;

        [SerializeField]
        public int _end_round;

        [SerializeField]
        public GroupSalve[] _groups;

        public int start_round => _start_round;

        public int end_round => _end_round;

        public GroupSalve[] groups => _groups;

    }

    [Serializable]
    public class GroupSalve
    {
        [SerializeField]
        public int _quantity;

        [SerializeField]
        public EnemyInfo _info;

        public int quantity => _quantity;

        public EnemyInfo info => _info;
    }
}
