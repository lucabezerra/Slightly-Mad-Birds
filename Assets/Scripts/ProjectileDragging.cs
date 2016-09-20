using UnityEngine;
using System.Collections;

public class ProjectileDragging : MonoBehaviour {

    public float maxStretch = 3f;
    public LineRenderer catapultLineFront;
    public LineRenderer catapultLineBack;

    private SpringJoint2D spring;
    private Transform catapult;
    private Rigidbody2D rigid2d;
    private bool clickedOn;
    private Ray rayToMouse;
    private Ray leftCatapultToProjectile;
    private float maxStretchSquare;
    private float circleRadius;

    private Vector2 prevVelocity;

    void Awake()
    {
        spring = GetComponent<SpringJoint2D>();
        rigid2d = GetComponent<Rigidbody2D>();
        catapult = spring.connectedBody.transform;
    }

	void Start () {
        LineRendererSetup();
        rayToMouse = new Ray(catapult.position, Vector3.zero);
        leftCatapultToProjectile = new Ray(catapultLineFront.transform.position, Vector3.zero);
        maxStretchSquare = maxStretch * maxStretch;
        CircleCollider2D circle = GetComponent<CircleCollider2D>();
	}
	
	void Update () {
        if (clickedOn)
        {
            Dragging();
        }

        if (spring != null)
        {
            if (!rigid2d.isKinematic && prevVelocity.sqrMagnitude > rigid2d.velocity.sqrMagnitude)
            {
                Destroy(spring);
                rigid2d.velocity = prevVelocity;
            }

            if (!clickedOn)
            {
                prevVelocity = rigid2d.velocity;
            }

            LineRendererUpdate();
        } else {
            catapultLineFront.enabled = false;
            catapultLineBack.enabled = false;
            if (transform.position.x >= Camera.main.transform.position.x)
            {
                Camera.main.GetComponent<ProjectileFollow>().enabled = true;
            }
        }
	}

    void LineRendererSetup()
    {
        catapultLineFront.SetPosition(0, catapultLineFront.transform.position);
        catapultLineBack.SetPosition(0, catapultLineBack.transform.position);

        catapultLineFront.sortingLayerName = "Foreground";
        catapultLineBack.sortingLayerName = "Foreground";

        catapultLineFront.sortingOrder = 3;
        catapultLineFront.sortingOrder = 1;
    }

    void OnMouseDown()
    {
        spring.enabled = false;
        clickedOn = true;
    }

    void OnMouseUp()
    {
        spring.enabled = true;
        rigid2d.isKinematic = false;
        clickedOn = false;
    }

    void Dragging()
    {
        Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 catapultToMouse = mouseWorldPoint - catapult.position;

        if (catapultToMouse.sqrMagnitude > maxStretchSquare)
        {
            rayToMouse.direction = catapultToMouse;
            mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
        }

        mouseWorldPoint.z = 0f;
        transform.position = mouseWorldPoint;
    }

    void LineRendererUpdate()
    {
        Vector2 catapultToProjectile = transform.position - catapultLineFront.transform.position;
        leftCatapultToProjectile.direction = catapultToProjectile;
        Vector3 holdPoint = leftCatapultToProjectile.GetPoint(catapultToProjectile.magnitude + circleRadius);

        catapultLineFront.SetPosition(1, holdPoint);
        catapultLineBack.SetPosition(1, holdPoint);
    }

}
