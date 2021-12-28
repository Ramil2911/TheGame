using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GreenHP : MonoBehaviour
{
    public GameObject Player;
    public TextMeshProUGUI text;
    
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.TryGetComponent<RadiationSourceComponent>(out var rad) && rad.inheritedDamage > 0.0f)
        {
            text.color = Color.green;
        }
        else
        {
            text.color = Color.red;
        }
    }
}
