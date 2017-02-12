using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour {

    public int Cuántos, maxCuántos;
    public Vector2 max, min;

    private List<GameObject> AllTheEnemys = new List<GameObject>();
    [SerializeField]
    private GameObject Enemy;
    [SerializeField]
    private Slider Slider;
    private int howmany = 0;
    [SerializeField]
    private Text txt;
    
    void Update()
    {
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
