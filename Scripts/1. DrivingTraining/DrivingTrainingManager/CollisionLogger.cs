using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionLogger : MonoBehaviour
{
    private static int collisionNum;
    public int CollisionNum
    {
        get { collisionNum = collisionPositions.Count; return collisionNum; }
    }

    private static List<Vector3> collisionPositions = new List<Vector3>();
    public static List<Vector3> CollisionPositions => collisionPositions;
    
    // 이벤트 정의
    public delegate void MyEvent(Vector3 position);
    public static event MyEvent OnWheelchairCollision;
    
    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            collisionPositions.Add(contact.point);
            // 이벤트 발생
            if (OnWheelchairCollision != null) OnWheelchairCollision(contact.point);
        }
    }
}
