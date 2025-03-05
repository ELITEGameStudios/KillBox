using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DestroyOnPlayerContact : MonoBehaviour
{
    [SerializeField]
    private GameObject instantiated_object;

    [SerializeField]
    private bool indirectly_activated;

    public void TriggerEvent()
    {
        if (instantiated_object != null)
        {
            GameObject clone = Instantiate(instantiated_object, transform);
            clone.transform.SetParent(null);
            clone.transform.localScale = new Vector3(1, 1, 1);
        }

        Destroy(gameObject);
    }
}
