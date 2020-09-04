using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endCredits : MonoBehaviour
{
    public Animator animator;
    public Animator fade;
    public GameObject cache;
    public GameObject player;
    public GameObject refPlayer;
    public AK.Wwise.Event mainThemeMusic;

    public AK.Wwise.Event creditsTheme;

    private void Start()
    {
        mainThemeMusic.Post(gameObject);
        player.transform.position = refPlayer.transform.position;
        creditsTheme.Post(gameObject);
    }
    public void GetUp()
    {
        animator.SetTrigger("GetUp");
    }
    public void OutCred()
    {
        fade.SetTrigger("FadeOut");
    }
    public void InCred()
    {
        fade.SetTrigger("FadeIn");
    }

    public IEnumerator endTheGame()
    {
        cache.SetActive(true);
        yield return new WaitForSeconds(2f);
        creditsTheme.Stop(gameObject);
        SceneManager.LoadScene(1);
    }
}
