using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.TowerCreation.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class TC_UI_FloatingToolTip : MonoBehaviour
    {
        Image _image;
        TMP_Text _tipText;
        RectTransform _textTransform;
        RectTransform _imageTransform;
        [SerializeField] int _maxWidth = 600;
        [SerializeField] int _widthPerCharacter = 28;
        [SerializeField] int _mousePositionBuffer = 10;
        [SerializeField] int _appearSpeed = 2;
        float _middleScreenPositionY;

        int length;
        float _imageHeight;

        void Update()
        {
            FollowMouse();
        }
        public void Initialize(string text)
        {
            _middleScreenPositionY = GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.height / 2;
            _tipText = GetComponentInChildren<TMP_Text>();
            _textTransform = _tipText.gameObject.GetComponent<RectTransform>();
            _image = GetComponentInChildren<Image>();
            _imageTransform = _image.GetComponent<RectTransform>();

            Color textColor = _tipText.color;
            textColor.a = 0;
            _tipText.color = textColor;
            _image.color = new Color(1,1,1,0);

            StartCoroutine(ActivateToolTip());
            _tipText.text = text;
            CalculateLength();
            FollowMouse();
        }

        // Calculates the display length depending on the text.
        void CalculateLength()
        {
            string[] words = _tipText.text.Split('\n');

            foreach (string word in words)
            {
                int wordLength = word.Length;
                if (word.Contains("<color=#")) { wordLength -= 15;}
                if (wordLength > length) { length = wordLength; }
            }

            length *= _widthPerCharacter;
            length = Mathf.Clamp(length, 1, _maxWidth);
        }

        void FollowMouse()
        {
            Vector2 mousePosition = Input.mousePosition;

            // Adjust the spawn position of the tool tip to keep it on screen near the edges.
            int direction = mousePosition.y > _middleScreenPositionY ? -1 : 1;
                mousePosition.y += (((_imageHeight) / 2) + _mousePositionBuffer) * direction;
            
            GetComponent<RectTransform>().position = mousePosition;
        }

        IEnumerator ActivateToolTip()
        {
            //adjust size of tool tip depending on the text.
            yield return new WaitForEndOfFrame();
            _textTransform.sizeDelta = new Vector2(length, 0);
            yield return new WaitForEndOfFrame();
            _imageHeight = _textTransform.rect.height * 1.5f;
            float imageWidth = _textTransform.rect.width * 1.5f;
            _imageTransform.sizeDelta = new Vector2(imageWidth, _imageHeight);
            Color imageColor = new Color(1, 1, 1, 0);
            Color textColor = _tipText.color;
            
            // Fade into existance
            while (imageColor.a < 1)
            {
                yield return new WaitForEndOfFrame();
                _tipText.color = textColor;
                _image.color = imageColor;
                imageColor.a += Time.deltaTime * _appearSpeed;
                textColor.a += Time.deltaTime * _appearSpeed;
            }
        }
    }
}
