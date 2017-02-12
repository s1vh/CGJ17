using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour {

    public int Cuántos = 150, maxCuántos;
    public Vector2 max, min;

    private List<GameObject> AllTheEnemys = new List<GameObject>();
    [SerializeField]
    private GameObject Enemy, Wave;
    float vel;
    [SerializeField]
    private Slider Slider;
    private int howmany = 0;
    [SerializeField]
    private Text txt, initializeText;
    [SerializeField]
    private Animator txtanim;
    private bool starting, started;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float normalSize, speedToStart;
    
    
    void Start()
    {
        txtanim = txtanim.GetComponent<Animator>();
        Time.timeScale = 0;
    }
    void Initialize()
    {
        if (starting)
        {
            starting = false;


            Time.timeScale = 1f;

            InvokeRepeating("SpawnWater", 0f, 1f);
            InvokeRepeating("SpawnWater", 0.5f, 1.25f);
            InvokeRepeating("SpawnWater", 1, 1.5f);
        }
    }
    void SpawnWater()
    {
        Instantiate(Wave, new Vector2(Random.Range(max.x, min.x), Random.Range(max.y, min.y)), transform.rotation);
    }
    void Update()
    {
        if (Input.GetButtonDown("Escape") )
        {
            if (started == false && starting == false)
            {
                Application.Quit();
            }
            else if(started == true && starting == false)
            {
                SceneManager.LoadScene(0);
            }
        }
        if (Input.anyKeyDown && started == false && starting == false)
        {
            starting = true;
            txtanim.SetTrigger("FadeOut");
            SpawnerTIME();
        }
        if ((cam.orthographicSize < normalSize -0.1f) && starting)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, normalSize, speedToStart/100);
        }
        else if (cam.orthographicSize >= normalSize - 0.1f && starting)
        {
            started = true;

            Destroy(txt.gameObject);

            Initialize();
        }

        Cuántos = Mathf.FloorToInt(Slider.value * maxCuántos);
        txt.text = Cuántos.ToString();
    }
    public void SpawnerTIME()
    {
        for (int i = 1; i  <= Cuántos; i++)
        {
            GameObject enemy = (GameObject) Instantiate(Enemy, new Vector2(Random.Range(max.x, min.x), Random.Range(max.y, min.y)), transform.rotation)as GameObject;
            AllTheEnemys.Add(enemy);
        }
        howmany += Cuántos;
    }
    public void DeleteAll()
    {
        foreach (GameObject enemy in AllTheEnemys)
        {
            Destroy(enemy);
        }
        AllTheEnemys.Clear();
    }
}
