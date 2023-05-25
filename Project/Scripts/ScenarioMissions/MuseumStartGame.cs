using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MuseumStartGame : MonoBehaviour
{
    [SerializeField]
    private string changeSceneName;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            SceneManager.LoadScene(changeSceneName);
        }
    }
}
