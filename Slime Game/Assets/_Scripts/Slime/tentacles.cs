using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class tentacles : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public List<GameObject> _nodes = new List<GameObject>();
    public float bottomPos;
    void Start()
    {
        //Debug.Log(transform.parent.GetComponent<SoftBody>().nodes.Count);
        // float num= transform.parent.GetComponent<SoftBody>().nodes.Count;
        // for(int i = 0; i < num; i++)
        // {
        //     _nodes.Add(transform.parent.GetComponent<SoftBody>().nodes[i]);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        findBottom();
    }

    public void findBottom()
    {
        GameObject lowestNode = _nodes[1];
        for(int i=1; i < _nodes.Count-1; i++)
        {
            if (_nodes[i].transform.position.y < lowestNode.transform.position.y)
                lowestNode = _nodes[i];
            
        }
        
        transform.position = new Vector2(transform.parent.position.x, lowestNode.transform.position.y+0.5f);
        
    }
}

