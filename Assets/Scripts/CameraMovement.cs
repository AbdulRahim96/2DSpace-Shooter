using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    public float MoveSpeed = 0.1f;
    public float time = 1;
    public World[] worlds;
    public GameObject planets, selectionMenu, worldMenu;
    public int index;
    public GameObject whiteFade, unlockButton;
    public Text worldNameText;

    public float minFOV, maxFOV = 60, currentFOV;
    private Camera camera;
    private bool canSelect;
    private Coroutine smoothCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        canSelect = true;
        camera = GetComponent<Camera>();
        PlayerPrefs.SetInt(worlds[0].worldName, 0);
    }

    public void CameraMoveTo(Transform point)
    {
        if(smoothCoroutine != null)
            StopCoroutine(smoothCoroutine);

        smoothCoroutine = StartCoroutine(SmoothMove(point));
    }

    IEnumerator SmoothMove(Transform point)
    {
        float time = 0;
        while(time < this.time)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, point.position, MoveSpeed);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, point.eulerAngles, MoveSpeed/5);
            yield return null;
        }
    }

    public void SetActiveWorld(bool flag)
    {
        worlds[index].mesh.SetActive(flag);
        planets.SetActive(!flag);
        selectionMenu.SetActive(!flag);
        worldMenu.SetActive(flag);

    }

    public void setIndex(int v)
    {
        index = v;
    }

    public async void SelectWorld()
    {
        if (!canSelect) return;
        WhiteFade();
        await WaitAsync(0.5f);
        SetActiveWorld(true);
        StopAllCoroutines();
    }

    public void WhiteFade()
    {
        GameObject obj = Instantiate(whiteFade);
        Destroy(obj, 1);
    }

    public async void mainmenu()
    {
        WhiteFade();
        await WaitAsync(0.5f);
        SetActiveWorld(false);
    }

    public static System.Threading.Tasks.Task WaitAsync(float seconds)
    {
        return System.Threading.Tasks.Task.Delay((int)(seconds * 1000));
    }

    public void Switch(int val)
    {
        index += val;
        if(index < 0 || index >= worlds.Length) // out of bound
        {
            if (index < 0)
                index = worlds.Length - 1;
            else
                index = 0;
        }
        CameraMoveTo(worlds[index].point);
        unlockButton.SetActive(!worlds[index].Unlocked());
        worldNameText.text = worlds[index].worldName;
    }

    [System.Serializable]
    public class World
    {
        public string worldName;
        public GameObject mesh;
        public Transform point;

        public bool Unlocked()
        {
            return PlayerPrefs.HasKey(worldName);
        }
    }
}
