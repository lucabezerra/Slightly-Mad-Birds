using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Resetter : MonoBehaviour {

    public Rigidbody2D projectile;
    public float resetSpeed = 0.025f;

    private float resetSpeedSquare;
    private SpringJoint2D spring;

    void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

	// Use this for initialization
	void Start () {
        resetSpeedSquare = resetSpeed * resetSpeed;
        spring = projectile.GetComponent<SpringJoint2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }

        if (spring == null && projectile.velocity.sqrMagnitude < resetSpeedSquare)
        {
            Reset();
        }
	}

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.GetComponent<Rigidbody2D>() == projectile)
        {
            Reset();
        }
    }
}
