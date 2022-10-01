﻿using UnityEngine;

namespace LudumDare51.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/TowerInfo", fileName = "TowerInfo")]
    public class TowerInfo : ScriptableObject
    {
        public int Range;
        public int ReloadTime;
        public GameObject Bullet;
    }
}