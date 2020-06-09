using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    NavMeshAgent  _navAgent;
    Transform player;
    Actions _actions;
    float health = 100f;
    bool hasDeathAnimationStarted = false;
    public EnemyRaycast enemyRaycast;

    private void Start() {
        _navAgent = this.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _actions = this.GetComponent<Actions>();
    }

    private void Update() 
    {
        if(health <= 0f)
        {
            if(!hasDeathAnimationStarted)
            _actions.Death();
            hasDeathAnimationStarted = true;
            return;
        }
        float dis = Vector3.Distance(this.transform.position, player.position);
        if(dis < 20f)
        {
            _actions.Run();
            _navAgent.SetDestination(player.position);
        }
        if(dis <= 10f)
        {
             Vector3 targetPostition = new Vector3(player.position.x, 
                                                    transform.position.y, 
                                                        player.position.z);
            transform.LookAt( targetPostition);
            _navAgent.ResetPath();
            _actions.Attack();
            enemyRaycast.Shoot();
        }
    }

    public void DecreaseHealth(float hitPoint)
    {
        health -= hitPoint;
        if(health <= 0)
        {
            _actions.Death();
            StartCoroutine(EnemyDead());
        }
        else
        {
            _actions.Damage();
        }
    }

    IEnumerator EnemyDead()
    {
        yield return new WaitForSeconds(5f);
        this.gameObject.SetActive(false);
    }
}
