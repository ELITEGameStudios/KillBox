using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SweepingIndicator : IndicatorBase
{
    [SerializeField] private SpriteRenderer mainGraphic, sweepingGraphic;
    [SerializeField] private Transform sweepingTransform;
    [SerializeField] private float sweepOpacity = 0.9f;

    Vector3 target_position = new Vector3(0, 100, 0);
    Vector3 initial_position = new Vector3(0, -100, 0);
    [SerializeField] private AnimationCurve baseOpacityCurve;

    // Start is called before the first frame update

    public override void OnUpdate()
    {
        sweepingGraphic.transform.localPosition = Vector3.Lerp(initial_position, target_position, 1 - normalizedTime);
        mainGraphic.color = Color.Lerp(Color.clear, mainColor, baseOpacityCurve.Evaluate(1 - normalizedTime));
        sweepingGraphic.color = Color.Lerp(Color.clear, mainColor, sweepOpacity);
    }

    public override void Setup()
    {
        sweepingGraphic.transform.localPosition = initial_position;
        mainGraphic.color = Color.clear;
        sweepingGraphic.color = Color.clear;
    }
}
