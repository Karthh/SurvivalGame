using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    // Start is called before the first frame update
    public string enitityName;
    public float maxHealth;
    public float currentHealth;
    public float strength;
    public float magic;
    public float physicalDefense;
    public float magicalDefence;
    public float moveSpeed;
    public float resource;

    public Animator animator;
   // public Dictionary<string, float> stats;
    public virtual void Start()
    {
        InitializeStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void InitializeStats()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }
    /*
    public virtual void InitializeStats()
    {
        stats = new Dictionary<string, float>();
        stats.Add("Health", 0.0f);
        stats.Add("Strength", 0.0f);
        stats.Add("Magic", 0.0f);
        stats.Add("Physical Defense", 0.0f);
        stats.Add("Magical Defense", 0.0f);
        stats.Add("Movement Speed", 0.0f);
        stats.Add("Resource", 0.0f);
    }
    void InitializeStats(float health, float strength, float magic, float physicalDefense, float magicalDefense, float moveSpeed, float resource)
    {
        stats = new Dictionary<string, float>();
        stats.Add("Health", health);
        stats.Add("Strength", strength);
        stats.Add("Magic", magic);
        stats.Add("Physical Defense", physicalDefense);
        stats.Add("Magical Defense", magicalDefense);
        stats.Add("Movement Speed", moveSpeed);
        stats.Add("Resource", resource);
    }
    */
}
