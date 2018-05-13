using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_controller : MonoBehaviour {

    [Header("Movement")]
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] float thrustStrength = 50f;

    [Header("Power Ups")]
    [Range(0f, 5f)]
    [SerializeField] float scalePlayer = 0.5f;

    [Header("Tags")]
    [SerializeField] string winningTag = "";
    [SerializeField] List<string> friendlyTags = new List<string>();

    [Header("Level Loading")]
    [SerializeField] float sceneLoadDelay = 1f;

    [Header("Sounds")]
    [SerializeField] AudioClip levelCompletedSound;
    [SerializeField] AudioClip mainEngineSound;
    [SerializeField] AudioClip explosionSound;

    [Header("Particle Effects")]
    [SerializeField] ParticleSystem explosionParticle;
    [SerializeField] ParticleSystem winParticle;
    [SerializeField] ParticleSystem thrustParticle;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    //private Vector3 playerStartingPos;
    //private Vector3 playerOriginalSize;
    //private int currentLevel;

    bool collisionsDisabled = false;

    Rigidbody rb;
    AudioSource audioSource;

    // Use this for initialization
    void Start ()
    {
        //playerStartingPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //playerOriginalSize = transform.localScale;
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        // TODO stop sound on death
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
        if (Debug.isDebugBuild) // only responds to debug keys if development build is ticked in build settings
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                collisionsDisabled = !collisionsDisabled;
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                LoadNextLevel();
            }
        }
    }


    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //thrusting
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            thrustParticle.Stop();
        }
    }

    private void ApplyThrust()
    {
        rb.AddRelativeForce(Vector3.up * thrustStrength);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngineSound);
        }
        thrustParticle.Play();
    }
    

    private void Rotate()
    {
        // taking manual control of rotation
        rb.freezeRotation = true;

        float rotationSpeedInFrame = rotationSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            //rotate left
            transform.Rotate(Vector3.forward * rotationSpeedInFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //rotate right
            transform.Rotate(-Vector3.forward * rotationSpeedInFrame);
        }
        //resuming physics control of rotation
        rb.freezeRotation = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // if player is not alive then exit the function to avoid the body of the function executing multiple times
        // ignores collisions when not alive
        if (state != State.Alive | collisionsDisabled)
        {
            return;
        }

        if (!friendlyTags.Contains(collision.gameObject.tag))
        {
            print("BOOM!");
            //TODO Add explosion effect
            //TODO Add explosion sound
            //Destroy(gameObject);
            //for now moving the player back to start
            //transform.position = playerStartingPos;
            //transform.localScale = playerOriginalSize;

            audioSource.Stop(); // making sure the thrust sound stops
            audioSource.PlayOneShot(explosionSound);

            explosionParticle.Play();

            state = State.Dying;
            Invoke("RestartGame", sceneLoadDelay);
        } else if (collision.gameObject.tag == winningTag)
        {
            audioSource.Stop(); // making sure the thrust sound stops
            audioSource.PlayOneShot(levelCompletedSound);

            winParticle.Play();

            print("HURRAY!!!");
            //Invoke allows for a delay in function call
            state = State.Transcending;
            Invoke("LoadNextLevel", sceneLoadDelay);
        }
    }

    private void LoadNextLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex; // returns the build index for the current scene.
        int nextLevel = currentLevel + 1; ;
        
        if (currentLevel == SceneManager.sceneCountInBuildSettings - 1)
        {
            nextLevel = 0;
        }

        SceneManager.LoadScene(nextLevel); // allow for more than 2 levels
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PowerUp")
        {
            transform.localScale += new Vector3(scalePlayer, scalePlayer, scalePlayer);
            Destroy(other.gameObject);
        }
    }

}
