using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public Renderer fondo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fondo.material.mainTextureOffset = fondo.material.mainTextureOffset + new Vector2(0.02f,0)* Time.deltaTime;
    }
}
