using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class LockRotationToPlayer : MonoBehaviour
{
    [SerializeField]
    private LookAtConstraint constraint;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = player.transform;
        source.weight = 1;
        constraint.AddSource(source);
    }
    
    // Update is called once per frame
    void Update()
    {

    }
}
