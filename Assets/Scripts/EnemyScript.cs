using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{

    [Header("Enemy Movement")]
    public NavMeshAgent enemyAgent;

    private float enemyHealth = 100f;
    public float presentHealth;
    public float damage = 10f;

    public GameObject[] walkPoints;
    int currentEnemyPosition = 0;
    public float enemySpeed;
    float walkingRadius = 2;

    public Transform playerBody;

    public LayerMask playerLayer;

    public float visionRadius;
    public float shootingRadius;
    public bool playerInvisionRadius;
    public bool playerInShootingRadius;

    public float timeBetweenShoot;
    public bool prevShoot;
    public Transform LookPoint;
    public Camera ShootingRayCastArea;

    public Animator anim;

    /* used to initialize variables before game start */
    private void Awake()
    {
        presentHealth = enemyHealth;
        playerBody = GameObject.Find("PlayerObject").transform;
        enemyAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    private void Update()
    {
        playerInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, playerLayer);
        playerInShootingRadius = Physics.CheckSphere(transform.position, shootingRadius, playerLayer);

        if(!playerInShootingRadius && !playerInvisionRadius)
        {
            /*Debug.Log("Player not in Shooting/Invision Radius...");*/
            Guard();
        }
        if(playerInvisionRadius && !playerInShootingRadius)
        {
            /*Debug.Log("Player Invision Radius...");*/
            PursuePlayer();
        }

        if(playerInShootingRadius && playerInvisionRadius)
        {
            ShootPlayer();
        }

        Vector3 pos = transform.position;
        pos.y = 1;
        transform.position = pos;
    }

    private void Guard()
    {
        // find distance b/w walkpoints and enemy position
        // if distance is less than walkingRadius then we update current enemy position with a random number
        // within a range of walk points array length
        if(Vector3.Distance(walkPoints[currentEnemyPosition].transform.position, 
            transform.position) < walkingRadius) {
            currentEnemyPosition = Random.Range(0, walkPoints.Length);
            if(currentEnemyPosition >= walkPoints.Length)
            {
                currentEnemyPosition = 0;
            }
        }
        // now it is moving towards the walk points
        transform.position = Vector3.MoveTowards(transform.position, 
            walkPoints[currentEnemyPosition].transform.position, 
            Time.deltaTime * enemySpeed);
        // enemy will face towards the walk point
        transform.LookAt(walkPoints[currentEnemyPosition].transform.position);
    }

    void PursuePlayer()
    {
        /*Debug.Log("Pursue Player...");*/
        if (enemyAgent.SetDestination(playerBody.position))
        {

            anim.SetBool("Walk", false);
            anim.SetBool("AimRun", true);
            anim.SetBool("Shoot", false);
            anim.SetBool("AimDie", false);

            /*Debug.Log("In shooting radius...");*/
            visionRadius = 80f;
            shootingRadius = 25f;
        }
        else
        {
            anim.SetBool("Walk", false);
            anim.SetBool("AimRun", false);
            anim.SetBool("Shoot", false);
            anim.SetBool("AimDie", true);
        }
    }

    void ShootPlayer()
    {
        enemyAgent.SetDestination(playerBody.position);
        transform.LookAt(LookPoint);
        if(!prevShoot)
        {
            RaycastHit hit;
            if(Physics.Raycast(ShootingRayCastArea.transform.position, ShootingRayCastArea.transform.forward, out hit, shootingRadius))
            {
                //Debug.Log("Shooting : " + hit.transform.name);
                PlayerScript playerBody = hit.transform.GetComponent<PlayerScript>();
                /*if(playerBody != null)
                {
                    playerBody.playerHitDamage();
                }*/
                /*anim.SetBool("Walk", false);
                anim.SetBool("AimRun", false);
                anim.SetBool("Shoot", true);
                anim.SetBool("AimDie", false);*/
            }
            prevShoot = true;
            Invoke(nameof(ActiveShooting), timeBetweenShoot);
        }
    }
    void ActiveShooting()
    {
        prevShoot = false;
    }

    public void enemyHealthDamage(float damage)
    {
        presentHealth -= damage;
        if(presentHealth <= 0)
        {
            anim.SetBool("Walk", false);
            anim.SetBool("AimRun", false);
            anim.SetBool("Shoot", false);
            anim.SetBool("AimDie", true);
            enemyDie();
        }
    }

    private void enemyDie()
    {
        enemyAgent.SetDestination(transform.position);
        enemySpeed = 0;
        shootingRadius = 0f;
        visionRadius = 0f;
        playerInShootingRadius = false;
        playerInvisionRadius = false;
        Object.Destroy(gameObject, 5.0f);
    }
}
