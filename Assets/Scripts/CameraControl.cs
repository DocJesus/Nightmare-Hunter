using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraControl : MonoBehaviour
{
    public float dampTime = 0.2f;
    public float screenBuffer = 4f;
    public float minSize = 6.5f;
    public Transform PlayerSpawn;
    public List<GameObject> targets;
    public PlayerSpawn playerSpawn;

    private Camera _camera;
    private float _zoomSpeed;
    private Vector3 _moveVelocity;
    private Vector3 _desiredPostion;

	// Use this for initialization
	void Start ()
    {
        targets = playerSpawn.targets;
        _camera = GetComponentInChildren<Camera>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Move();
        Zoom();
	}

    void Move()
    {
        FindAveragePosition();
        transform.position = Vector3.SmoothDamp(transform.position, _desiredPostion, ref _moveVelocity, dampTime);
    }

    void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTarget = 0;

        for (int i = 0; i < targets.Count; i++)
        {
            if (!targets[i].gameObject.activeSelf)
                continue;
            averagePos = targets[i].transform.position;
            numTarget += 1;
        }

        if (numTarget > 0)
            averagePos = averagePos / numTarget;
        averagePos.y = transform.position.y;
        _desiredPostion = averagePos;
    }

    void Zoom()
    {
        float requireSize = findRequireSize();
        _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, requireSize, ref _zoomSpeed, dampTime);
    }

    float findRequireSize()
    {
        Vector3 desiredLocalPos = transform.InverseTransformPoint(_desiredPostion);

        float size = 0f;
        for (int i = 0; i < targets.Count; i++)
        {
            if (!targets[i].gameObject.activeSelf)
                continue;

            Vector3 targetLocalpos = transform.InverseTransformPoint(targets[i].transform.position);
            Vector3 desiredDistance = targetLocalpos - desiredLocalPos;

            size = Mathf.Max(size, Mathf.Abs(desiredDistance.y));
            size = Mathf.Max(size, Mathf.Abs(desiredDistance.x) / _camera.aspect);

            
        }

        size += screenBuffer;
        size = Mathf.Max(size, minSize);
        return size;
    }

    public void setStartPosition()
    {
        FindAveragePosition();
        transform.position = _desiredPostion;
        _camera.orthographicSize = findRequireSize();
    }

    public bool allPlayerDead()
    {
        int j = 0;
        //Debug.Log("allpalyerDeaddebutfct");
        for (int i = 0; i < targets.Count; i++)
        {
            //Debug.Log("tour de allPlayerDead");
            if (targets[i].GetComponentInChildren<PlayerHealth>().currentHealth <= 0)
            {
                j = j + 1;
            }
        }

        if (j == targets.Count)
            return true;
        return false;
    }
}
