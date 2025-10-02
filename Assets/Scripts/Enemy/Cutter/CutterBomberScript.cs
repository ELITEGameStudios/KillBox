using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutterBomberScript : MonoBehaviour
{
    private bool phase_one = true;
    public SpriteRenderer renderer;
    public Collider2D collider;
    public ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("MainCoroutine");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MainCoroutine()
    {
        while (renderer.color.a < 1)
        {
            renderer.color += new Color(0f, 0f, 0f, 1 * Time.deltaTime);
            transform.localScale += new Vector3(0.5f * Time.deltaTime, 0.5f * Time.deltaTime, 0);
            yield return null;
        }

        renderer.color = Color.green;
        transform.localScale = new Vector3(2, 2, 1);

        collider.enabled = true;
        gameObject.GetComponent<EnemyDamage>().enabled = true;
        particles.Play();

        while(transform.localScale.x > 0)
        {
            transform.localScale -= new Vector3(2 * Time.deltaTime, 2 * Time.deltaTime, 0);
            renderer.color -= new Color(0f, 0f, 0f, 2f * Time.deltaTime);
            if(renderer.color.a < 0.8f && collider.enabled){
                collider.enabled = false;
                gameObject.GetComponent<EnemyDamage>().enabled = false;
            }
            yield return null;
        }

        Destroy(gameObject);



    }
}
