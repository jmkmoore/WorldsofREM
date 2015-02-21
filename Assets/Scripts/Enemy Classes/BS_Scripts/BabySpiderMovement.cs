using UnityEngine;
using System.Collections;

public class BabySpiderMovement : MonoBehaviour {

    public float gravity = -25f;
    public float runSpeed = 8f;
    public float groundDamping = 20f; // how fast do we change direction? higher means faster
    public float inAirDamping = 5f;

    [HideInInspector]
    private float normalizedHorizontalSpeed = 0;

    private CharacterController2D _controller;
    private Animator _animator;
    private RaycastHit2D _lastControllerColliderHit;
    private Vector3 _velocity;

    private float jumpCooldown=.5f;
    private float jumpTimer = 0f;

    private bool left = true;
    private bool right = false;


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
        
        if (_controller.isGrounded)
        {
            if (jumpTimer == 0)
            {
                _velocity.y = Mathf.Sqrt(15f * -gravity);
                _animator.StopPlayback();
                _animator.Play(Animator.StringToHash("Jump_Forward"));
                jumpTimer = jumpCooldown;
            }
        }
        else
            if (jumpTimer > 0)
            {
                jumpTimer -= Time.deltaTime;
                if (jumpTimer < 0)
                {
                    jumpTimer = 0;
                }
            }
        if (left)
        {
            normalizedHorizontalSpeed = -1;
            if (transform.localScale.x > 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            if (transform.localScale.x < 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            normalizedHorizontalSpeed = 1;
        }
        
        var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping;
        _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime);
        _velocity.y += gravity * Time.deltaTime;
        _controller.move(_velocity * Time.deltaTime);
	
	}
}
