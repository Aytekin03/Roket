using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

// degiþiklik yapýldýysa evet de

public class Carpisma : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;


    AudioSource audioSource;

    bool isTransitioning;
    bool collisionDisable = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisable = !collisionDisable;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning )
        {
            return;
        }
        // Tag yerine yazdýgýmýz degiþkenlerle if kullanmak zorunda kalmadan swich deðikeni ile code yazma 
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("kalkisa haziriz efendim");
                break;
            case "Finish":
                StartSuccessSequence();
                break ;
            case "Fuel":
                Debug.Log("yakit yüklendi");
                break;
            default:
                StartCrashSequence();
                break;

        }
    }
    void StartSuccessSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        GetComponent<Moment>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
    }
    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(crash);
        crashParticles.Play();
        GetComponent<Moment>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
    }
    // çarptýktan sonra 1 saniye bekle daha sonra sahneyi yenilememizi saglar Invoke
    //bölüm bittiginde bir sonraki sahneye gitmemmizi saglayan code sahneler toplamlarý eþitleyerek if ifadesi kullanarak  
    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        
        SceneManager.LoadScene(nextSceneIndex);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
