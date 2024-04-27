using UnityEngine.AI;

public class Droid : AI_Shooter
{
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canChase)
            agent.SetDestination(target.position);
    }
}
