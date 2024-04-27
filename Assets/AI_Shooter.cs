using UnityEngine;

public class AI_Shooter : MonoBehaviour
{
    public Transform target;
    public float AttackDistance = 10;
    public ParticleSystem shootingEffect;
    public float TimeToShootAtPlayer;
    private float time;
    protected bool canChase;
    public bool isBurst;
    public int amountOfBurst = 3;
    public float gap = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        time = TimeToShootAtPlayer;
        target = GameObject.FindGameObjectWithTag("Player").transform;

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, target.position) <= AttackDistance)
        {
            canChase = true;
            transform.LookAt(target);
            time -= 1 * Time.deltaTime;
            if (time <= 0)
            {
                shoot();
                time = TimeToShootAtPlayer;
            }

            if (TryGetComponent<FollowPath>(out FollowPath follow))
                follow.enabled = false;
        }
        else
            canChase = false;

    }
    async void shoot()
    {
        if(shootingEffect)
        {
            if(isBurst)
            {
                for (int i = 0; i < amountOfBurst; i++)
                {
                    shootingEffect.Play();
                    await GameManager.delay(gap);
                }
            }
            else
                shootingEffect.Play();

        }
        //  GetComponent<AudioSource>().Play();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, AttackDistance);
    }
    
}
