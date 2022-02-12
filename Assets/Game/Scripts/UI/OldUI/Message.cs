using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Message : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    [SerializeField]private TMP_Text text;

    private Tween tween;
    private void Awake()
    {
        text = text == null ? GetComponent<TMP_Text>() : text;
        
    }

    public virtual void Initialize(string newText, RectTransform spawnArea)
    {
        text.text = newText;
        var lowerBound = new Vector2(spawnArea.rect.xMin, spawnArea.rect.yMin);
        var upperBound = new Vector2(spawnArea.rect.xMax, spawnArea.rect.yMax);

        var startingPosition = new Vector3();
        startingPosition.x = Random.Range(lowerBound.x, upperBound.x);
        startingPosition.y = Random.Range(lowerBound.y, upperBound.y);

        var endingPosition = new Vector3();
        endingPosition.x = Random.Range(lowerBound.x, upperBound.x);
        endingPosition.y = Random.Range(lowerBound.y, upperBound.y);

        transform.localPosition = startingPosition;
        var isEndingXSmaller = startingPosition.x > endingPosition.x;
        tween = DOVirtual.Vector3(startingPosition, endingPosition, duration, v =>
        {
            transform.localPosition = v;
        });
        DOVirtual.Float(1, 0, duration, v =>
        {
            text.alpha = v;
        });
        tween.onComplete += () => Destroy(gameObject);
    }

    private void OnDestroy()
    {
        tween.Kill();
    }
}
