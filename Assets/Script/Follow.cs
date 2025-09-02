using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;

public class Follow : MonoBehaviour
{
    RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        var playerPos = GameManager.Instance.player.transform.position;
        rect.position = Camera.main.WorldToScreenPoint(playerPos);
    }
}
