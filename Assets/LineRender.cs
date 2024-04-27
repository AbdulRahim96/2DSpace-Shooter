using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRender : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public ParticleSystem particle;
    public float lineWidth = 0.1f;
    public float removalRate = 2f;
    public float movementSpeed = 5f;
    public Vector3 mousePosition;
    private Camera mainCamera;
    private bool isMousePressed;
    private int pointsCount;
    private Vector3 previousPosition;
    public float minDistance = 0.1f;

    private Coroutine coroutine;
    void Start()
    {
        mainCamera = Camera.main;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 1;
        previousPosition = transform.position;
        StopCoroutine(coroutine);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMousePressed = true;
            Start();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMousePressed = false;
            ReverseLineRendererPositions();
            coroutine = StartCoroutine(MovePointsSmoothly());
        }

        if (Input.GetMouseButton(0))
        {
            DrawLine();
        }
    }

    void DrawLine()
    {
        Vector3 currentPosition;

        currentPosition = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(currentPosition);
        RaycastHit hit;

        // Check if the ray hits something
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            currentPosition = hit.point + mousePosition;
        }

        if (Vector3.Distance(currentPosition, previousPosition) > minDistance)
        {
            if (previousPosition == transform.position)
            {
                lineRenderer.SetPosition(0, currentPosition);
            }
            else
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentPosition);
            }

            previousPosition = currentPosition;
        }
    }

    IEnumerator MovePointsSmoothly()
    {
        int lineLength = lineRenderer.positionCount;
        particle.Play();
        for (int i = lineLength - 1; i >= 0; i--)
        {
            Vector3 targetPosition = lineRenderer.GetPosition(i - 1);

            while (Vector3.Distance(lineRenderer.GetPosition(i), targetPosition) > 0.01f)
            {
                lineRenderer.SetPosition(i, Vector3.MoveTowards(lineRenderer.GetPosition(i), targetPosition, Time.deltaTime * movementSpeed));
                particle.transform.position = targetPosition;
                yield return null;
            }
            lineRenderer.positionCount--;
        }
        particle.Stop();

    }

    void ReverseLineRendererPositions()
    {
        int lineLength = lineRenderer.positionCount;
        Vector3[] positions = new Vector3[lineLength];

        for (int i = 0; i < lineLength; i++)
        {
            positions[i] = lineRenderer.GetPosition(lineLength - 1 - i);
        }

        lineRenderer.SetPositions(positions);
    }
}
