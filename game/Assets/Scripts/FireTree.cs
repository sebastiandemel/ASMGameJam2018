using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTree : MonoBehaviour {

    public Color fireColor;

    private Color storedColor;
    private MeshRenderer m_Renderer;

    public bool isOnFire;

    private void Awake()
    {
        m_Renderer = GetComponent<MeshRenderer>();
    }

    // Use this for initialization
    void Start () {
        
        storedColor = m_Renderer.material.color;
	}
	
	// Update is called once per frame
	void Update () {
        m_Renderer.material.color = isOnFire ? fireColor : storedColor;
	}
}
