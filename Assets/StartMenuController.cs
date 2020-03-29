using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public GameObject quad;
    private Renderer quadRenderer;
    public void StartGameButtonTapped()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGameButtonTapped()
    {
        Application.Quit();
    }

    private void Start()
    {
        quadRenderer = quad.GetComponent<Renderer>();
    }

    private void Update()
    {
        quadRenderer.material.mainTextureOffset=new Vector2(Time.time*0.5f,0);
    }
}
