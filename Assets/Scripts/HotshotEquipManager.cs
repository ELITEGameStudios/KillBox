using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class HotshotEquipManager : MonoBehaviour
{


    public GameObject hotshot_grenade;

    public void GamemodeStart()
    {
        GameObject clone = Instantiate(hotshot_grenade, transform);
        clone.transform.SetParent(null);
    }
}
