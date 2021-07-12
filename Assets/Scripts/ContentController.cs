using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentController : MonoBehaviour
{
    GameObject content;
    Transform contentTransform;

    public GameObject hitGameObject;

    public ColorStore colorStore;

    public ScrollRect scrollRect;

    void Start()
    {
        content = GameObject.Find("Content");
        contentTransform = content.GetComponent<Transform>();
    }

    void Update()
    {
        // Making scroll loop when reaching First or Last element
        if (scrollRect.horizontalNormalizedPosition >= 0.999f)
        {
            Debug.Log("Scrolled near End");
            
            Transform firstChild = contentTransform.GetChild(0);

            firstChild.transform.SetSiblingIndex(contentTransform.childCount - 1);

            scrollRect.horizontalNormalizedPosition = 0.995f;
        }
        else if (scrollRect.horizontalNormalizedPosition <= 0.001f)
        {
            Debug.Log("Scrolled near Start");
            
            Transform lastChild = contentTransform.GetChild(contentTransform.childCount - 1);

            lastChild.transform.SetSiblingIndex(0);

            scrollRect.horizontalNormalizedPosition = 0.005f;
        }
    }

    void LateUpdate()
    {
        // Finding the Panel which color must be changed
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if (hit)
            {
                // Debug.Log(hit.collider.gameObject.name);
                hitGameObject = hit.collider.gameObject;

                colorStore.newColor = hitGameObject.GetComponent<Image>().color;
            }
        }
    }
}
