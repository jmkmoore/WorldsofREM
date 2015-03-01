using UnityEngine;
using System.Collections;

public class ProjectileRaven : MonoBehaviour {

    private float normalizedHorizontalSpeed = 0;

    private CharacterController2D _controller;
    private Animator _animator;
    private RaycastHit2D _lastControllerColliderHit;
    private Vector3 _velocity;

    public float flightSpeed = 5f;
    public float verticalChangeSpeed = 2f;
    public float waveTimer = 1f;
    public float moveTimer = 0f;
    public bool flyUp = true;

    public float gravity = -25f;
    public float runSpeed = 8f;

    public float projectileTimer = 2.5f;
    public float projectileCooldown = 3f;
    public float projectileAnimationTimer = 0.667f;
    private bool firedProjectile = true;

    public GameObject FeatherProjectile;
    Random rand;

    // Use this for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController2D>();

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;
        rand = new Random();
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

        projectileTimer += Time.deltaTime;

        if (firedProjectile)
        {
            if (projectileTimer > 1.75f && _velocity.x == 0)
            {
                _animator.StopPlayback();
                _animator.Play(Animator.StringToHash("Flying"));
                _velocity.x = flightSpeed;
            }

            if (projectileTimer > projectileCooldown)
            {
                projectileTimer = 0;
                firedProjectile = false;
            }

        }
        
        else if (!firedProjectile)
        {
            if (projectileTimer > 1.0f)
            {
                ShootFeather();
                firedProjectile = true;
                _animator.Play(Animator.StringToHash("DoubleWing"));
            }
            if (projectileTimer < 0.5f)
            {
                _animator.StopPlayback();
                _animator.Play(Animator.StringToHash("Idle"));
                firedProjectile = false;
                _velocity.x = 0;
            }
        }

        _velocity.y = 0;
        _controller.move(_velocity * Time.deltaTime);
	}

    void ShootFeather()
    {
        NonNormalProjectile feather = (NonNormalProjectile)FeatherProjectile.GetComponent("NonNormalProjectile");
       // feather.setDirection(transform.localScale.x);
        feather.setAngle(2 * transform.localScale.x, -1);
        NonNormalProjectile bulletClone = (NonNormalProjectile)Instantiate(feather, new Vector3(transform.position.x + (2.5f * transform.localScale.x), transform.position.y, transform.position.z), transform.rotation);

    }
}
