using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisiblityController : MonoBehaviour
{
    public bool invisible = true;
    
    // Start is called before the first frame update
    void Start()
    {
        SetChildrenTransparency(transform);
    }

    // Update is called once per frame
    void SetChildrenTransparency(Transform parent)
    {
        // 부모의 자식들을 반복하며 처리
        foreach (Transform child in parent)
        {
            // Renderer 컴포넌트가 있는지 확인
            Renderer renderer = child.GetComponent<Renderer>();
            
            if (renderer != null)
            {
                renderer.enabled = false;
            }

            // 자식의 자식들도 처리 (재귀 호출)
            SetChildrenTransparency(child);
        }
    }
}
