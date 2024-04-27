using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FollowPath : MonoBehaviour
{
    public Transform[] paths;
    public int currentIndex;
    public float speed = 5;
    public float delayTime = 2;

    private Coroutine loop;
    // Start is called before the first frame update
    void Start()
    {
        loop = StartCoroutine(follow());
    }

    IEnumerator follow()
    {
        while (true)
        {
            transform.DOMove(paths[currentIndex].position, speed).SetEase(Ease.Linear);

            yield return new WaitForSeconds(speed + delayTime);
            currentIndex++;
            if (currentIndex >= paths.Length)
                currentIndex = 0;
        }
    }

    private void OnDisable()
    {
        StopCoroutine(loop);
    }
}
