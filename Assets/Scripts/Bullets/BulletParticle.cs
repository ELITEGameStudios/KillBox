using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParticle : MonoBehaviour
{
    [SerializeField]
    private Transform bullet_transform;

    void ResetCoroutine()
    {
        StopCoroutine(PlayAnim());
        transform.SetParent(bullet_transform);
    }

    public IEnumerator PlayAnim()
    {
        transform.position = bullet_transform.position;
        gameObject.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSecondsRealtime(0.4f);

        ResetCoroutine();
    }
}
