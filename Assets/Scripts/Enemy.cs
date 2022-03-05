using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    public int starthealth;
    public int currenthealth;
    public ParticleSystem deathParticles;

    public NavMeshAgent _agent;
    public GameObject _target;

    public Animator anim;
    private void Awake()
    {
        currenthealth = starthealth;
    }
    public void TakeDamage(int damage)
    {
        currenthealth -= damage;
        if (currenthealth <= 0)
        {
            Die();
        }
    }

    private void Update()
    {
        _agent.SetDestination(_target.transform.position);
        float distance = Vector3.Distance(this.transform.position, _target.transform.position);
            transform.LookAt(_target.transform,Vector3.up);
        if(distance<5)
        {
            anim.SetBool("Attack", true);
        }
        else
        {
            anim.SetBool("Attack", false);
            
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            TakeDamage(10);
        }
    }
    private void Die()
    {

        gameObject.SetActive(false);
        Instantiate(deathParticles, transform.position, Quaternion.identity);
    }

}
