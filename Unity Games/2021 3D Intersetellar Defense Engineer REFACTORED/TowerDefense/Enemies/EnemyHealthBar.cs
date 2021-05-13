using UnityEngine;

namespace TowerDefense.Enemies
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] GameObject _frontPlate;
        [SerializeField] SpriteRenderer _sprite;

        void Update() { transform.LookAt(GameObject.Find("Main Camera").transform.position); }

        public void SetStatus(float percent)
        {
            Vector3 localScale = transform.localScale;
            _frontPlate.transform.localScale = new Vector3(percent * 2, localScale.y, localScale.z);
            if (percent >= 0.5f)
            {
                float numerator = (percent * 100) - 50;
                float newPercent = numerator / 50;
                _sprite.color = new Color(1 - newPercent, 1, 0, 1);
            }
            else
            {
                float numurator = (percent * 100) + 50;
                float newPercent = numurator / 100;
                _sprite.color = new Color(1, newPercent, 0, 1);
            }
        }
    }
}