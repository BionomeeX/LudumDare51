using System;
using UnityEngine;
using UnityEngine.UI;

namespace LudumDare51
{
    public class ButtonInit : MonoBehaviour
    {
        [SerializeField]
        private Image _image;

        public void Init(Action callback, Sprite sprite)
        {
            _image.sprite = sprite;
            GetComponent<Button>().onClick.AddListener(new(callback));
        }
    }
}
