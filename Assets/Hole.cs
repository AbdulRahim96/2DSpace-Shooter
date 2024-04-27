using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hole : MonoBehaviour
{
    public float range = 5, swollowRange = 1;
    public static float distance, pullStrength;
    public float playerDistance;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        if (!player) return;
        playerDistance = Vector3.Distance(transform.position, player.position);

        if (playerDistance <= range)
        {
            // Calculate the direction from the object to the black hole
            Vector3 pullDirection = transform.position - player.position;
            distance = pullDirection.magnitude;

            // Apply gravitational force (inverse square law)
            float pullForce = pullStrength / Mathf.Pow(distance, 2);
            player.GetComponent<Rigidbody>().AddForce(pullDirection.normalized * pullForce);
            if (playerDistance < swollowRange)
                Swollow();
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, swollowRange);
    }

    void Swollow()
    {
        PlayerControls.canControl = false;
        player.DOMove(transform.position, 0.5f);
        player.DOScale(0, 0.5f).OnComplete(() => Destroy(player.gameObject));
    }
}
