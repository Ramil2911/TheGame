using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private EntityComponent entityComponent;
    private TextMeshProUGUI _healthText;
    
    // Start is called before the first frame update
    void Start()
    {
        entityComponent.HealthChangedEvent.AddListener(OnHealthChanged);
        _healthText = this.gameObject.GetComponent<TextMeshProUGUI>();
        _healthText.color = Color.red;
    }

    public void OnHealthChanged(float health)
    {
        _healthText.text = ((int) health).ToString();
    }
}
