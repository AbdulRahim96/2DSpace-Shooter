using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TriggerCustom : MonoBehaviour
{

    public LayerMask layer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "obstacles")
        {
            other.transform.DOMove(transform.position, 1f);
            other.transform.DOScale(0, 0.5f).OnComplete(() => Destroy(other.gameObject));
        }
    }
}
