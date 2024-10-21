using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelchairManager: MonoBehaviour
{
    public float _torqueMagnitude = 4.2f;

    // Init()을 통해, PhysicsModel > WheelL or WheelR > TorqueWheel을 찾아와 설정.
    private TorqueWheel _leftWheel;
    private TorqueWheel _rightWheel;

    void Start()
    {
        Init();
    }

    void Init()
    {
        Transform physicalModel = transform.Find("PhysicsModel");

        if (physicalModel != null)
        {
            Transform wheelL = physicalModel.Find("WheelL");

            if (wheelL != null)
            {
                _leftWheel = wheelL.GetComponent<TorqueWheel>();
                _leftWheel.magnitude = _torqueMagnitude;
            }
            else
            {
                Debug.LogError("\"WheelL\" 오브젝트를 찾을 수 없습니다.");
            }
            Transform wheelR = physicalModel.Find("WheelR");

            if (wheelL != null)
            {
                _rightWheel = wheelR.GetComponent<TorqueWheel>();
                _rightWheel.magnitude = _torqueMagnitude;
            }
            else
            {
                Debug.LogError("\"WheelL\" 오브젝트를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("\"PhysicsModel\" 오브젝트를 찾을 수 없습니다.");
        }
    }

    public void AllowMove()
    {
        _leftWheel.magnitude = _torqueMagnitude;
        Debug.Log("휠체어를 움직일 수 있습니다.");
    }
    public void ProhibitMove()
    {
        _leftWheel.magnitude = 0;
    }

     void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) AllowMove();
        if (Input.GetKeyDown(KeyCode.W)) ProhibitMove();
    }
}
