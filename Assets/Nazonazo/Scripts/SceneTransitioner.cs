using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    public void ToNazonazo()
    {
        SceneManager.LoadScene("Nazonazo");
    }
    public void ToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}