using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RuneSpawnsScript : MonoBehaviour
{
    [SerializeField] Transform[] runeSpawnPositions;
    [SerializeField] GameObject[] runeGameObjects;
    public bool runesScattered;


    public void ScatterRunes()
    {
        List<Transform> transforms = runeSpawnPositions.ToList();
        foreach (GameObject rune in runeGameObjects)
        {
            int randomIndex = Random.Range(0, transforms.Count);

            rune.transform.SetParent(transforms[randomIndex]);
            rune.transform.localPosition = Vector3.zero;

            transforms.RemoveAt(randomIndex);
        }

        runesScattered = true;
    }
}
