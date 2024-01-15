﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private GameObject _laserPrefab;

    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;

    [SerializeField]
    private float _fireRate = 2.0f;
    private float _canFire = -1f;
    private bool _canFireBool = true;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }

        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("The Enemy animator is NULL");
        }
        if (_audioSource == null)
        {
            Debug.LogError("The Audio Source on the enemy is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire && _canFireBool == true)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int laser_index = 0; laser_index < lasers.Length; laser_index++)
            {
                lasers[laser_index].AssignEnemyLaser();
            }
        }
    }
    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);


        if (transform.position.y < -5.4f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit: " + other.transform.name);
        
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage();
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject, 2.8f);
        }



        if (other.tag == "Laser")
        {
            Laser laser = other.transform.GetComponent<Laser>();
            if (laser != null && laser._isEnemyLaser == true)
            {
                goto DontDamage;
            }
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10); 
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            _canFireBool = false;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject, 2.5f);

            DontDamage:
                { }  //doesnt deal damage 'cause it hit an enemy while being an enemy laser
        }
        
    }

}

