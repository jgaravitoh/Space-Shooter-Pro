using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;
    
    //LASERS
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;
    
    private bool isTripleShotActive = false;
    //private bool isSpeedBoostActive = false;
    private bool isShieldsActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;
    [SerializeField]
    private GameObject _leftEngine, _rightEngine;

    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioSource _audioSource;



    void Start()
    {  
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }
        if (_audioSource == null)
        {
            Debug.LogError("The AudioSource on the player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
        
        
    }



    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput);

            transform.Translate(direction * _speed * Time.deltaTime);


        // y bounds
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y,-3.8f,0f), 0);
        
        // x bounds
        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }



    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (isTripleShotActive == true)   // = true then Triple Shot
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }

        else   //single fire
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
        _audioSource.Play();
        
    }

    
        
    
    public void TakeDamage()
    {
        if (isShieldsActive == true)
        {
            isShieldsActive = false;
            _shieldVisualizer.SetActive(false);
            return; 
        }

        _lives -= 1;

        if (_lives == 2)
        {
            int _randomEngine = Random.Range(0, 2);
            if (_randomEngine == 0){
                _leftEngine.SetActive(true);
            }
            else
            {
                _rightEngine.SetActive(true);
            }
        }
        if (_lives == 1)
        {
            if (_rightEngine.activeSelf == true)
            {
                _leftEngine.SetActive(true);
            }
            else
            {
                _rightEngine.SetActive(true);
            }
        }


        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }

    }
    public void TripleShotActive()
    {
        
        isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
        
        
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        isTripleShotActive = false;
    }


    public void SpeedBoostActive()
    {
        
        //isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }
    
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        //isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldsActive()
    {
        isShieldsActive = true;
        _shieldVisualizer.SetActive(true);
    }
    
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}

