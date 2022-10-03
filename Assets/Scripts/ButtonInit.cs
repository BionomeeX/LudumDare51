using System;
using UnityEngine;
using UnityEngine.UI;

namespace LudumDare51
{
    public class ButtonInit : MonoBehaviour
    {
        [SerializeField]
        private Image _image;
        [SerializeField]
        private Image _weaponSprite;

        public void Init(Action callback, Sprite sprite, Sprite weaponSprite)
        {
            _image.sprite = sprite;
            _weaponSprite.sprite = weaponSprite;
            GetComponent<Button>().onClick.AddListener(new(callback));
        }
    }
}
