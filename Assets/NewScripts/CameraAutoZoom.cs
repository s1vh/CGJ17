using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAutoZoom : MonoBehaviour
{
    Camera camera;
    float radar;
    float timer;
    [SerializeField]
    float timerMargin;
    bool focus;

    // Set up references.
    void Awake()
    {
        camera = GetComponent<Camera>();
    }

    // Use this for initialization
    void Start()
    {
        radar = 2f;
        timer = timerMargin;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = timerMargin;
            if (Physics2D.OverlapCircle(new Vector2(camera.transform.position.x, camera.transform.position.y), radar, 1 << LayerMask.NameToLayer("Enemy")))
            {
                focus = true;
                camera.orthographicSize = camera.orthographicSize - 0.01f;
                radar = radar - 0.01f;
                Debug.Log("There are enemies!");
            }
            else
            {
                focus = false;
                camera.orthographicSize = camera.orthographicSize + 0.01f;
                radar = radar + 0.01f;
                Debug.Log("There are not visible enemies!");
            }
        }
        else
        {
            if (focus && Physics2D.OverlapCircle(new Vector2(camera.transform.position.x, camera.transform.position.y), radar, 1 << LayerMask.NameToLayer("Enemy")))
            {
                camera.orthographicSize = camera.orthographicSize - 0.01f;
                radar = radar - 0.01f;
                Debug.Log("There are enemies!");
            }
            else if (!focus && !Physics2D.OverlapCircle(new Vector2(camera.transform.position.x, camera.transform.position.y), radar, 1 << LayerMask.NameToLayer("Enemy")))
            {
                camera.orthographicSize = camera.orthographicSize + 0.01f;
                radar = radar + 0.01f;
                Debug.Log("There are not visible enemies!");
            }
        }
        Debug.DrawRay(camera.transform.position, new Vector3(0f, radar, 0f), Color.green, 1f);
    }

}
