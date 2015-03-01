using UnityEngine;
using System.Collections;

public class WebShooter : MonoBehaviour {

    public float gravity = -25f;
    public float runSpeed = 8f;
    public float groundDamping = 20f; // how fast do we change direction? higher means faster
    public float inAirDamping = 5f;

    public float shotCooldown = 1.3f;
    public float shotTimer = 0f;
    public float playTimer= 1.5f;

    private CharacterController2D _controller;
    private Animator _animator;
    private RaycastHit2D _lastControllerColliderHit;
    private Vector3 _velocity;

    bool fired = false;

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

        if (shotTimer == 0)
        {
            _animator.StopPlayback();
            _animator.Play(Animator.StringToHash("Web_Shot"));
        }
         
        if (shotTimer < playTimer)
        {
            shotTimer += Time.deltaTime;
        }

        if (shotTimer >= shotCooldown && fired == false)
        {
            fired = true;
            shootWeb(transform.localScale.x);
        }
        
        if (shotTimer >playTimer)
        {
            shotTimer = 0;
            fired = false;
            _animator.StopPlayback();
            _animator.Play(Animator.StringToHash("Stand"));
        }
        

        _velocity.x = 0f;
        _velocity.y = 0f;

        _controller.move(_velocity * Time.deltaTime);

	}

    public void shootWeb(float direction)
    {
        Projectile web = (Projectile)webBulletPrefab.GetComponent("Projectile");
        web.setDirection(direction);
        Projectile bulletClone = (Projectile)Instantiate(web, new Vector3(transform.position.x + (2.5f * transform.localScale.x), transform.position.y + 1, transform.position.z), transform.rotation);
    }
}
