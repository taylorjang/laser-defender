using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float speed = 6.0f;
	public GameObject projectile;
	public AudioClip fireSound;
	public float padding = 1.0f;
	public float projectileSpeed = 10;
	public float firingRate = 0.2f;
	public float health = 750f;
	
	float xmin;
	float xmax;

    private PlayerHealthKeeper playerHealthKeeper;

    void OnTriggerEnter2D(Collider2D collider){
		Projectile missile = collider.gameObject.GetComponent<Projectile>();
		if(missile){
			health -= missile.GetDamage();
            //Debug.Log("Player Collided with missile, health= " + health);
            playerHealthKeeper.Health(health);
            missile.Hit();
			if (health <= 0) {
				Die();
			}
		}
	}
	
	void Die(){
		LevelManager man = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		man.LoadLevel("Win Screen");
		Destroy(gameObject);
	}

    private void Awake()
    {

        playerHealthKeeper = GameObject.Find("PlayerHealth").GetComponent<PlayerHealthKeeper>();
        playerHealthKeeper.Health(health);

    }

    void Start(){
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
		xmin = leftmost.x;
		xmax = rightmost.x;

    }
	
	void Fire(){
        //Vector3 offset = new Vector3(0f, 1f, 0f);
		GameObject beam = Instantiate(projectile, transform.position , Quaternion.identity) as GameObject;
		beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed, 0);
		AudioSource.PlayClipAtPoint(fireSound, transform.position);
	}

	void Update () {
        //print("speed: " + speed);
        //print("Time.deltaTime: " + Time.deltaTime);
        //print("Vector3.right * speed * Time.deltaTime: " + Vector3.right * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space)){
			InvokeRepeating("Fire", 0.0001f, firingRate);
		}
		if(Input.GetKeyUp(KeyCode.Space)){
			CancelInvoke("Fire");
		}

		if(Input.GetKey(KeyCode.LeftArrow)){
			transform.position += Vector3.left * speed * Time.deltaTime;
		}else if (Input.GetKey(KeyCode.RightArrow)){
			transform.position += Vector3.right * speed * Time.deltaTime; 
		}
		
		// restrict the player to the gamespace
		float newX = Mathf.Clamp(transform.position.x, xmin, xmax);

		transform.position = new Vector3(newX, transform.position.y, transform.position.z);
	}
	
	
	
}
