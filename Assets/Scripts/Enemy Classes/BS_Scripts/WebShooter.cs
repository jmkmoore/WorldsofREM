using UnityEngine;
using System.Collections;

public class WebShooter : MonoBehaviour {

    public float gravity = -25f;
    public float runSpeed = 8f;
    public float groundDamping = 20f; // how fast do we change direction? higher means faster
    public float inAirDamping = 5f;

    public float shotCooldown = 5f;
    public float shotTimer = 0f;

    private CharacterController2D _controller;
    private Animator _animator;
    private RaycastHit2D _lastControllerColliderHit;
    private Vector3 _velocity;

    public GameObject webBulletPrefab;

    #region On Start
    // Use this for initialization
	void Start () {
	
	}

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController2D>();

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;

    }

    #endregion

    #region Event Listeners

    void onControllerCollider(RaycastHit2D hit)
    {
        // bail out on plain old ground hits cause they arent very interesting
        if (hit.normal.y == 1f)
            return;

        // logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
        //Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
    }


    void onTriggerEnterEvent(Collider2D col)
    {
        //if (col.name.Equals("TienHitBox"))
        //   updateAttack(true);
        //if (col.name.Equals("Wall"))
        //{
        //   updateDirection();
        //}
        Debug.Log(gameObject.name + "onTriggerEnterEvent: " + col.gameObject.name);
    }


    void onTriggerExitEvent(Collider2D col)
    {
        // if (col.name.Equals("TienHitBox"))
        // {
        //     updateAttack(false);
        // }
        Debug.Log("onTriggerExitEvent: " + col.gameObject.name);
    }

    #endregion


	// Update is called once per frame
	void Update () {
        _velocity = _controller.velocity;

        if (shotTimer > 0)
        {
            shotTimer -= Time.deltaTime;
            if (shotTimer < 0)
            {
                shotTimer = 0;
            }
        }
        
        if (shotTimer == 0)
        {
            _animator.Play(Animator.StringToHash("Web_Shot"));
            shotTimer = shotCooldown;

            Instantiate(webBulletPrefab, webBulletPrefab.transform.position, webBulletPrefab.transform.rotation);
        }
        _velocity.x = 0f;
        _velocity.y += gravity * Time.deltaTime;

        _controller.move(_velocity * Time.deltaTime);


	
	}
}
