using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAutoZoom : MonoBehaviour
{
    Camera camera;
    float radar;
    float timer;
    [SerializeField]
    bool auto = false;
    [SerializeField]
    float baseSize = 3f;
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
        radar = baseSize - 1f;
        timerMargin = 2f;
        timer = timerMargin;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Fire2"))
        {
            if (auto)
            {
                auto = false;
                //camera.orthographicSize = baseSize;
            }
            else
            {
                auto = true;
                radar = baseSize - 1f;
                timer = timerMargin;
            }
        }
        if (auto)
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
        else
        {
            if (camera.orthographicSize - 0.02f > baseSize)
            {
                camera.orthographicSize = camera.orthographicSize - 0.02f;
            }
            else if (camera.orthographicSize + 0.02f < baseSize)
            {
                camera.orthographicSize = camera.orthographicSize + 0.02f;
            }
        }
    }

}
