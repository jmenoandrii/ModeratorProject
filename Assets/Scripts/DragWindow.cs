using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragWindow : MonoBehaviour, IDragHandler, IPointerDownHandler {

    [SerializeField] private RectTransform _dragRectTransform;
    [SerializeField] private Canvas _canvas;

    private RectTransform _canvasRectTransform;

    private void Awake() {
        // protection
        if (_dragRectTransform == null) {
            _dragRectTransform = transform.parent.GetComponent<RectTransform>();
        }

        // protection
        if (_canvas == null) {
            Transform testCanvasTransform = transform.parent;
            while (testCanvasTransform != null) {
                _canvas = testCanvasTransform.GetComponent<Canvas>();
                if (_canvas != null) {
                    break;
                }
                testCanvasTransform = testCanvasTransform.parent;
            }
        }

        if (_canvas != null)
        {
            _canvasRectTransform = _canvas.GetComponent<RectTransform>();
        }
    }

    public void OnDrag(PointerEventData eventData) {
        if (_dragRectTransform == null || _canvasRectTransform == null) return;

        Vector2 newPosition = _dragRectTransform.anchoredPosition + eventData.delta / _canvas.scaleFactor;

        Vector2 canvasSize = _canvasRectTransform.rect.size;
        Vector2 objectSize = _dragRectTransform.rect.size;

        float minX = -canvasSize.x / 2 + objectSize.x / 2;
        float maxX = canvasSize.x / 2 - objectSize.x / 2;
        float minY = -canvasSize.y / 2 + objectSize.y / 2;
        float maxY = canvasSize.y / 2 - objectSize.y / 2;

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        _dragRectTransform.anchoredPosition = newPosition;
    }

    public void OnPointerDown(PointerEventData eventData) {
        _dragRectTransform.SetAsLastSibling();
    }
}
