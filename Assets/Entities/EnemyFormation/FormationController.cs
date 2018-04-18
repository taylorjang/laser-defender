using UnityEngine;
using System.Collections;

public class FormationController : MonoBehaviour {
	public GameObject enemyPrefab;
	public float width = 3f;
	public float height = 5f;
	public float speed = 5f;
	public float spawnDelay = 0.5f;
	public float padding = 1;
	
	private bool movingRight = true;
	private float xmax;
	private float xmin;

    private bool movingTop = true;
    private float ymax = 3f;
    private float ymin = 0.5f;

    // Use this for initialization
    void Start () {
		float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0,0, distanceToCamera));
		Vector3 rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1,0, distanceToCamera));
		xmax = rightEdge.x;
		xmin = leftBoundary.x;
        //ymax = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, distanceToCamera)).y;
        //ymin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceToCamera)).y;
        SpawnUntilFull();
        //foreach (Transform child in transform)
        //{
        //    GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity);
        //    enemy.transform.parent = child;
        //}
	}
	
	void SpawnUntilFull(){
		Transform freePosition = NextFreePosition();
		if(freePosition){
			GameObject enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = freePosition;
		}
		if(NextFreePosition()){
			Invoke ("SpawnUntilFull", spawnDelay);
		}
	}

    Transform NextFreePosition()
    {
        foreach (Transform childPositionGameObject in transform)
        {
            if (childPositionGameObject.childCount == 0)
            {
                return childPositionGameObject;
            }
        }
        return null;
    }

    public void OnDrawGizmos(){
		Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
	}
	
	// Update is called once per frame
	void Update () {
		if(movingRight){
			transform.position += Vector3.right * speed * Time.deltaTime;
		}else{
			transform.position += Vector3.left * speed * Time.deltaTime;
		}

    	// Check if the formation is going outside the playspace...
		float rightEdgeOfFormation = transform.position.x + (0.5f*width);
		float leftEdgeOfFormation = transform.position.x - (0.5f*width);
		if(leftEdgeOfFormation < xmin){
			movingRight = true;
		}
        else if (leftEdgeOfFormation > xmax)
        {
            movingRight = false;
        }

        if (movingTop)
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.down * speed * Time.deltaTime;
        }

        float bottom = transform.position.y + (0.5f * width);
        float top = transform.position.y - (0.5f * width);
        if (bottom < ymin)
        {
            movingTop = true;
        }
        else if (top > ymax)
        {
            movingTop = false;
        }

        if (AllMembersDead()){
			Debug.Log("Empty Formation");
			SpawnUntilFull();
		}
	}
	

	
	
	bool AllMembersDead(){
		foreach(Transform childPositionGameObject in transform){
			if (childPositionGameObject.childCount > 0){
				return false;
			}
		}
		return true;
	}
	
}
