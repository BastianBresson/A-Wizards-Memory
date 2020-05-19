using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private int defaultScene = default;
    [SerializeField] private Animator animator = default;

    private int fireSceneIndex = 2;
    private int earthSceneIndex = 1;
    private int waterSceneIndex = 3;

    private float sceneLoadDelay = 1f;


    public void OnLoadScene(Element element)
    {
        int buildIndex = ElementBuildIndex(element);

        StartCoroutine(LoadSceneCoroutine(buildIndex));
    }


    public void OnLoadScene()
    {
        StartCoroutine(LoadSceneCoroutine(defaultScene));
    }


    private int ElementBuildIndex(Element element)
    {
        int levelIndex = defaultScene;

        switch (element.ElementType)
        {
            case Element.ElementEnum.Normal:
                break;
            case Element.ElementEnum.Fire:
                levelIndex = fireSceneIndex;
                break;
            case Element.ElementEnum.Water:
                levelIndex = waterSceneIndex;
                break;
            case Element.ElementEnum.Earth:
                levelIndex = earthSceneIndex;
                break;
            default: levelIndex = defaultScene;
                break;
        }

        return levelIndex;
    }


    IEnumerator LoadSceneCoroutine(int buildIndex)
    {
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(sceneLoadDelay);

        SceneManager.LoadScene(buildIndex);
    }
}
