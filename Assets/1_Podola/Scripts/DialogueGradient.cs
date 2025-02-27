using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class ProceduralGradient : Graphic {
    [SerializeField] private Color topColor = Color.white;
    [SerializeField] private Color bottomColor = Color.black;

    protected override void OnPopulateMesh(VertexHelper vh) {
        vh.Clear();

        // RectTransform 기준 위치/크기
        Rect rect = rectTransform.rect;
        Vector2 bottomLeft  = new Vector2(rect.xMin, rect.yMin);
        Vector2 topLeft     = new Vector2(rect.xMin, rect.yMax);
        Vector2 bottomRight = new Vector2(rect.xMax, rect.yMin);
        Vector2 topRight    = new Vector2(rect.xMax, rect.yMax);

        // 버텍스 4개 (사각형)
        UIVertex vertex = UIVertex.simpleVert;

        // 1) bottomLeft
        vertex.color = bottomColor;
        vertex.position = bottomLeft;
        vh.AddVert(vertex);

        // 2) topLeft
        vertex.color = topColor;
        vertex.position = topLeft;
        vh.AddVert(vertex);

        // 3) topRight
        vertex.color = topColor;
        vertex.position = topRight;
        vh.AddVert(vertex);

        // 4) bottomRight
        vertex.color = bottomColor;
        vertex.position = bottomRight;
        vh.AddVert(vertex);

        // 삼각형 인덱스(2개)
        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(2, 3, 0);
    }
}
