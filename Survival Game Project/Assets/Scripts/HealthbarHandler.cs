using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public float fillAmount;
    public Image content;
    public float currentHealth;
    public float maxHealth;
    public Text healthText;
    public Entity entity;
    void Start()
    {
        entity = transform.root.GetComponent<Entity>();
        maxHealth = entity.maxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        if(entity != null)
        {
            currentHealth = entity.currentHealth;
            fillAmount = currentHealth / maxHealth;
            content.fillAmount = fillAmount;
        }
    }
}
