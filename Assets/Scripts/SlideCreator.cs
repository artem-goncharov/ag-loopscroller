using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideCreator : MonoBehaviour
{
    public GameObject imgObject;
    
    GameObject content;
    Transform contentTransform;
    
    void Start()
    {
        content = GameObject.Find("Content");
        contentTransform = content.transform;
    }

    public void InstantiateImgPrefab()
    {
        GameObject newImg = Instantiate(imgObject, contentTransform) as GameObject;
    }
}
