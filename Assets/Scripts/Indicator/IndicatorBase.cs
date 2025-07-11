using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IndicatorBase : MonoBehaviour
{

    public float time, currentTime;
    public float normalizedTime { get{ return currentTime/time; } }
    public Color mainColor;
    public bool isActive {get { return gameObject.activeInHierarchy; }}

    public void StartIndicator(float time, Vector2 position, Vector2 direction)
    {
        currentTime = time;
        this.time = time;

        transform.position = position;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

        Setup();
        gameObject.SetActive(true);

    }

    public void StartIndicator(float time, Transform parent)
    {
        currentTime = time;

        transform.SetParent(parent);
        transform.position = parent.position;
        transform.rotation = parent.rotation;

        Setup();
        gameObject.SetActive(true);
    }


    void Update()
    {
        if (currentTime > 0)
        {
            OnUpdate();
            currentTime -= Time.deltaTime;
        }
        else
        {
            End();
            gameObject.SetActive(false);
        }
    }

    public abstract void Setup();
    public abstract void OnUpdate();
    public virtual void End()
    {

    }

}
