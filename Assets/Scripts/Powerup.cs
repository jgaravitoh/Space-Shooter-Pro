using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private float _speed = 3.0f;
    
    //ID for Powerups
    //0 = Triple Shots
    //1 = Speed
    //2 = Shields

    [SerializeField]
    private int powerupID;
    [SerializeField]
    private AudioClip _powerUpClip;





    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -4.5f)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            print(player);
            if (player != null)
            {

                AudioSource.PlayClipAtPoint(_powerUpClip, player.transform.position);

                switch (powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldsActive();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }
            
            Destroy(gameObject);
        }
    }
}
