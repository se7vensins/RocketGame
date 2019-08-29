using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    private Rigidbody rigidBody;

    private AudioSource audioSauce;

    [SerializeField] private float rcsThrust;

    [SerializeField] private float lift;

    [SerializeField] AudioClip _rocketSound, _deathSound, _successSound;

    [SerializeField] ParticleSystem _rocketParticle, _deathParticle, _successParticle;

    [SerializeField] float _loadDelay = 2f;

    bool _isTransition = false;

    [SerializeField]bool _collisionDisabled = false;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSauce = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isTransition)
        {
            RespondToThrust();
            RespondToRotate();
        }
        if (Debug.isDebugBuild)
        {
            DebugMode();
        }


    }

    private  void DebugMode()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNext();
        }

        else if (Input.GetKeyDown(KeyCode.C))
        {
            _collisionDisabled = !_collisionDisabled;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(_isTransition || _collisionDisabled)
        {
            return;
        }
        switch (other.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                NextLevelTransitionSFX();
                break;
            default:
                DeathSFX();
                break;
        }
      
    }

    private void DeathSFX()
    {
        _isTransition = true;
        audioSauce.Stop();
        audioSauce.PlayOneShot(_deathSound);
        _rocketParticle.Stop();
        _deathParticle.Play();
        Invoke("RestartLevelto1", _loadDelay);
    }

    private void NextLevelTransitionSFX()
    {
        _isTransition = true;
        audioSauce.Stop();
        audioSauce.PlayOneShot(_successSound);
        _rocketParticle.Stop();
        _successParticle.Play();
        Invoke("LoadNext", _loadDelay);
    }

    private void RestartLevelto1()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNext()
    {
        int _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int _nextSceneIndex = _currentSceneIndex + 1;
        if(_nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            _nextSceneIndex = 0;
        }
        SceneManager.LoadScene(_nextSceneIndex);
    }

    private void RespondToThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            StopThrust();
        }
    }

    private void StopThrust()
    {
        audioSauce.Stop();
        _rocketParticle.Stop();
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * lift * Time.deltaTime);
        if (!audioSauce.isPlaying)
        {
            audioSauce.PlayOneShot(_rocketSound);
            _rocketParticle.Play();
        }
        
    }

    private void RespondToRotate()
    {

        

        if (Input.GetKey(KeyCode.A))
        {
            ManualRotate(rcsThrust * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            ManualRotate(-rcsThrust * Time.deltaTime);
        }

    }

    private void ManualRotate(float rotationSpeed)
    {
        rigidBody.freezeRotation = true; // take manual control of rotation
        transform.Rotate(Vector3.forward * rotationSpeed);
        rigidBody.freezeRotation = false; // resume physics control
    }
}
