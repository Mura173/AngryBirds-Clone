using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Drag : MonoBehaviour
{
    private Collider2D drag;
    public LayerMask layer;

    [SerializeField]
    private bool clicked;
    private Touch touch;

    public LineRenderer lineFront;
    public LineRenderer lineBack;

    private Ray leftCatapultRay;
    private CircleCollider2D birdCol;
    private Vector2 catapultToBird;
    private Vector3 pointL;

    private SpringJoint2D spring;
    private Vector2 prevVel;
    private Rigidbody2D rb;

    public ParticleSystem bomb;

    // Limit
    private Transform catapult;
    private Ray rayToMT;

    // Start is called before the first frame update
    void Start()
    {
        drag = GetComponent<Collider2D>();
        SetupLine();

        leftCatapultRay = new Ray(lineFront.transform.position, Vector3.zero);
        birdCol = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        spring = GetComponent<SpringJoint2D>();

        catapult = spring.connectedBody.transform;
        rayToMT = new Ray(catapult.position, Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        LineUpdate();
        SpringEffect();

        prevVel = rb.velocity;

#if UNITY_ANDROID

        if (Input.touchCount > 0)
        {
            // Determinando o index do toque
            touch = Input.GetTouch(0);

            Vector2 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            // Raio onde esta tocando
            RaycastHit2D hit = Physics2D.Raycast(wp, Vector2.zero, Mathf.Infinity, layer.value);

            if (hit.collider != null)
            {
                clicked = true;
            }

            if (clicked)
            {
                if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                {
                    Vector3 tPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));

                    catapultToBird = tPos - catapult.position;

                    if (catapultToBird.sqrMagnitude > 9f)
                    {
                        rayToMT.direction = catapultToBird;
                        tPos = rayToMT.GetPoint(3f);
                    }

                    transform.position = tPos;
                }
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                rb.isKinematic = false;
                clicked = false;
            }
        }

#endif

#if UNITY_EDITOR

        if(clicked)
        {
            Draggin();
        }
#endif

        if(clicked == false && rb.isKinematic == false)
        {
            KillBird();
        }
    }

    void SetupLine()
    {
        lineFront.SetPosition(0, lineFront.transform.position);
        lineBack.SetPosition(0, lineBack.transform.position);
    }

    void LineUpdate()
    {
        catapultToBird = transform.position - lineFront.transform.position;
        leftCatapultRay.direction = catapultToBird;

        // Magnitude: retorna o comprimento do vetor
        pointL = leftCatapultRay.GetPoint(catapultToBird.magnitude + birdCol.radius);

        lineFront.SetPosition(1, pointL);
        lineBack.SetPosition(1, pointL);
    }

    void SpringEffect()
    {
        if (spring != null)
        {
            if (rb.isKinematic == false)
            {
                // Retorno do comprimento do quadrado de um vetor
                if (prevVel.sqrMagnitude > rb.velocity.sqrMagnitude)
                {
                    lineFront.enabled = false;
                    lineBack.enabled = false;
                    Destroy(spring);
                    rb.velocity = prevVel;
                }
            }
        }
    }

    void KillBird()
    {
        if (rb.velocity.magnitude == 0 && rb.IsSleeping())
        {
            StartCoroutine(DeathTime());
        }
    }

    IEnumerator DeathTime()
    {
        yield return new WaitForSeconds(1);
        Instantiate(bomb, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        Destroy(gameObject);
    }

    // Mouse

    void Draggin()
    {
        Vector3 mouseWP = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWP.z = 0f;

        catapultToBird = mouseWP - catapult.position;

        if (catapultToBird.sqrMagnitude > 9f)
        {
            rayToMT.direction = catapultToBird;
            mouseWP = rayToMT.GetPoint(3f);
        }

        transform.position = mouseWP;
    }

    private void OnMouseDown()
    {
        clicked = true;
    }

    private void OnMouseUp()
    {
        rb.isKinematic = false;
        clicked = false;
    }
}