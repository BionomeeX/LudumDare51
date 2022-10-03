using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class SFXManager : MonoBehaviour
    {
        public static SFXManager Instance
        {
            private set; get;
        }

        [SerializeField]
        private AudioSource _source;

        private void Awake()
        {
            Instance = this;
        }

        public void PlaySound(AudioClip audio)
        {
            _source.PlayOneShot(audio);
        }
    }
}