﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragWindow : MonoBehaviour, IDragHandler, IPointerDownHandler {

    [SerializeField] private RectTransform _dragRectTransform;
    [SerializeField] private Canvas _canvas;

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
    }

    public void OnDrag(PointerEventData eventData) {
        _dragRectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnPointerDown(PointerEventData eventData) {
        _dragRectTransform.SetAsLastSibling();
    }
}
