using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransitionController : MonoBehaviour
{
    public string sceneToLoad;
    private Image image;
    public float transitionSpeed = 1f;
    private bool shouldReveal;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        shouldReveal = true;
        image.material.SetFloat("_Cutoff",-0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        shouldReveal = !GameManager.isLevelOver;        

        if (shouldReveal)
        {
            image.material.SetFloat("_Cutoff", Mathf.MoveTowards(image.material.GetFloat("_Cutoff"), 1.1f, transitionSpeed * Time.deltaTime));
        }
        else
        {
            image.material.SetFloat("_Cutoff", Mathf.MoveTowards(image.material.GetFloat("_Cutoff"), -0.1f - image.material.GetFloat("_EdgeSmoothing"), transitionSpeed * Time.deltaTime));
            if (image.material.GetFloat("_Cutoff") - (-0.1f - image.material.GetFloat("_EdgeSmoothing")) < 0.1f)
            {                
                gameManager.LoadNextLevel();
            }
        }
    }
}
