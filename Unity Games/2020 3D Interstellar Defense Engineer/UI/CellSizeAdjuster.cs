using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellSizeAdjuster : MonoBehaviour
{
    AutoExpandGridLayoutGroup grid;
    RectTransform rect;
    float width;
    [SerializeField] float widthModifyer = 0.35f;
    // Start is called before the first frame update
    void Update()
    {
        grid = GetComponent<AutoExpandGridLayoutGroup>();
        rect = GetComponent<RectTransform>();
        width = Mathf.Abs(rect.rect.xMax - rect.rect.xMin);
        grid.cellSize = new Vector2(width * widthModifyer, width * widthModifyer);
    }

}
