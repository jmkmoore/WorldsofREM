using UnityEngine;
using System.Collections;

public class IrvineController : MonoBehaviour {
    #region Stage 1 cooldowns
    public float slamCooldown = 3f;
    public float spitCooldown = 5f;

    public float slamTimer = 0f;
    public float spitTimer = 0f;
    #endregion

    #region stage 2 cooldowns
    public float seedCooldown = 3f;
    public float spitTwoCooldown = 6f;

    public float seedTimer = 0f;
    public float spitTwoTimer = 0f;
    #endregion

    #region Movement Values
    public float gravity = -25f;
    public float runSpeed = 0;
    public float groundDamping = 20f; // how fast do we change direction? higher means faster
    public float inAirDamping = 5f;

    [HideInInspector]
    private float normalizedHorizontalSpeed = 0;

    private CharacterController2D _controller;
    private Animator _animator;
    private RaycastHit2D _lastControllerColliderHit;
    private Vector3 _velocity;
    private bool left = true;
    #endregion

    private EnemyHealth myHealth;

    public float transitionTimer = 0f;

    public int stage = 1;
    public float currentAttackTimer = 0;
    public string currentAttack = "idle";
    public bool transitioned = false;

    public GameObject myFireball;

    public bool fireOne = false;
    public bool fireTwo = false;
    public bool fireThree = false;
    public bool thrownSeed = false;
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
        Debug.Log(col.name);
        if (col.gameObject.layer == 19)
        {
            left = !left;
        }
    }


    void onTriggerExitEvent(Collider2D col)
    {

    }

    #endregion


    public GameObject log;
    public GameObject mySolidBox;

	// Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController2D>();

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;

        currentAttack = "idle";

        myHealth = (EnemyHealth)gameObject.GetComponent<EnemyHealth>();
        log = GameObject.FindWithTag("Log");
        mySolidBox = GameObject.FindWithTag("Box");
	}
	
	// Update is called once per frame
    void Update()
    {
        #region Stage triggering
        if (myHealth.currentHealth < myHealth.maxHealth / 2)
        {
            stage = 2;
        }

        if (myHealth.currentHealth < 100)
        {
            stage = 4;
        }

        if (myHealth.currentHealth == 0)
        {
            stage = 5;
        }

        if (!transitioned)
        {
            if (stage == 2)
            {
                log.SetActive(false);
                _animator.Play(Animator.StringToHash("IrvineTransition"));
                transitionTimer += Time.deltaTime;
                if (transitionTimer > 1.45f)
                {
                    transitioned = true;
                }
            }
        }

        if (transitionTimer > 1.45f && stage == 2)
        {
            stage++;
        }

        #endregion

        #region Timers
        if (slamTimer != 0)
        {
            slamTimer += Time.deltaTime;
        }
        if (spitTimer != 0)
        {
            spitTimer += Time.deltaTime;
        }
        if (spitTimer > spitCooldown)
        {
            spitTimer = 0;
        }
        if (slamTimer > slamCooldown)
        {
            slamTimer = 0;
        }
        if (seedTimer > seedCooldown)
        {
            seedTimer = 0;
        }
        if (spitTwoTimer > spitTwoCooldown)
        {
            spitTwoTimer = 0;
        }

        if (spitTwoTimer != 0)
        {
            spitTwoTimer += Time.deltaTime;
        }
        if (seedTimer != 0)
        {
            seedTimer += Time.deltaTime;
        }
        #endregion


        if (currentAttackTimer != 0)
        {
            currentAttackTimer += Time.deltaTime;
        }

        if (currentAttack.Equals("spit") && currentAttackTimer > 1.2f && !fireOne)
        {
            fireSeedsOne();
            fireOne = true;
        }

        if(currentAttack.Equals("spit") && currentAttackTimer > 1.6f && !fireTwo){
            fireSeedsTwo();
            fireTwo = true;
        }

        if(currentAttack.Equals("spit") && currentAttackTimer > 2.37f && !fireThree){
            fireSeedsThree();
            fireThree = true;
        }

        if (currentAttack.Equals("seed") && currentAttackTimer > 1.0f && !thrownSeed)
        {
            throwSeed();
            thrownSeed = true;
        }

        #region stage 1 of fight
        if (stage == 1)
        {
            if ((currentAttackTimer > 1.5f && currentAttack.Equals("slam")) || (currentAttackTimer > 3.3f && currentAttack.Equals("spit")) || currentAttackTimer == 0)
            {
                if (slamTimer == 0)
                {
                    SlamAttack();
                    currentAttackTimer += Time.deltaTime;
                }
                else if (spitTimer == 0)
                {
                    spitStage1Attack();
                    currentAttackTimer += Time.deltaTime;
                    fireOne = false;
                    fireTwo = false;
                    fireThree = false;
                }
            }
        }
        #endregion

        #region stage 2 of fight
        if (stage == 3)
        {
            if ((currentAttackTimer > 1.7f && currentAttack.Equals("seed")) || currentAttackTimer == 0)
            {
                if (spitTwoTimer == 0)
                {
                    spitStage2Attack();
                    currentAttackTimer += Time.deltaTime;
                    fireOne = false;
                    fireTwo = false;
                    fireThree = false;
                }
                else if (seedTimer == 0)
                {
                    seedAttack();
                    currentAttackTimer += Time.deltaTime;
                    thrownSeed = false;
                }
            }
            else if (currentAttackTimer > 3.3f && currentAttack.Equals("spit"))
            {
                if (seedTimer == 0)
                {
                    seedAttack();
                    currentAttackTimer += Time.deltaTime;
                    thrownSeed = false;
                }
                else if (spitTwoTimer == 0)
                {
                    spitStage2Attack();
                    currentAttackTimer += Time.deltaTime;
                    fireOne = false;
                    fireTwo = false;
                    fireThree = false;
                }
            }
        }
        #endregion



        if (stage == 4)
        {
            _animator.Play(Animator.StringToHash("Pickup4"));
        }
        if (stage == 5)
        {
            _animator.Play(Animator.StringToHash("Death3"));
            mySolidBox.SetActive(false);
        }
        #region Movement
            _velocity = _controller.velocity;
            _velocity.x = 0;
            if (!_controller.isGrounded)
            {
                _velocity.y += gravity * Time.deltaTime;
            }
            else
                _velocity.y = 0;

            _controller.move(_velocity * Time.deltaTime);
            #endregion
    }

    void spitStage1Attack()
    {
        _animator.Play(Animator.StringToHash("FireSpitStage1"));
        spitTimer += Time.deltaTime;
        currentAttack = "spit";
        currentAttackTimer = 0;
    }

    void SlamAttack()
    {
        _animator.Play(Animator.StringToHash("IrvineSlam"));
        slamTimer += Time.deltaTime;
        currentAttack = "slam";
        currentAttackTimer = 0;
    }

    void spitStage2Attack()
    {
        _animator.Play(Animator.StringToHash("FireSpitStage2"));
        spitTwoTimer += Time.deltaTime;
        currentAttack = "spit";
        currentAttackTimer = 0;
    }

    void seedAttack()
    {
        _animator.Play(Animator.StringToHash("SeedThrow"));
        seedTimer += Time.deltaTime;
        currentAttack = "seed";
        currentAttackTimer = 0;
    }

    void fireSeedsOne()
    {
        Debug.Log("Seed one called");
        FireSeed newSeed = (FireSeed)myFireball.GetComponent("FireSeed");
        newSeed.setAngle(-.86f, -.5f);
        FireSeed bullet = (FireSeed)Instantiate(newSeed, new Vector3(transform.position.x + (-8f * transform.localScale.x), transform.position.y +25, transform.position.z), transform.rotation);
        newSeed.setAngle(-.7f, -.7f);
        FireSeed bullet2 = (FireSeed)Instantiate(newSeed, new Vector3(transform.position.x + (-8f * transform.localScale.x), transform.position.y + 25, transform.position.z), transform.rotation);
        newSeed.setAngle(-.5f, -.86f);
        FireSeed bullet3 = (FireSeed)Instantiate(newSeed, new Vector3(transform.position.x + (-8f * transform.localScale.x), transform.position.y + 25, transform.position.z), transform.rotation);
        
    }

    void fireSeedsTwo()
    {
        FireSeed newSeed = (FireSeed)myFireball.GetComponent("FireSeed");
        newSeed.setAngle(-.3f, -.6f);
        FireSeed bullet = (FireSeed)Instantiate(newSeed, new Vector3(transform.position.x + (-8f * transform.localScale.x), transform.position.y + 25, transform.position.z), transform.rotation);
        newSeed.setAngle(-.15f, -.9f);
        FireSeed bullet2 = (FireSeed)Instantiate(newSeed, new Vector3(transform.position.x + (-8f * transform.localScale.x), transform.position.y + 25, transform.position.z), transform.rotation);
        newSeed.setAngle(-.25f, -.26f);
        FireSeed bullet3 = (FireSeed)Instantiate(newSeed, new Vector3(transform.position.x + (-8f * transform.localScale.x), transform.position.y + 25, transform.position.z), transform.rotation);
    }
     
    void fireSeedsThree()
    {
        FireSeed newSeed = (FireSeed)myFireball.GetComponent("FireSeed");
        newSeed.setAngle(-.5f, -.32f);
        FireSeed bullet = (FireSeed)Instantiate(newSeed, new Vector3(transform.position.x + (-8f * transform.localScale.x), transform.position.y + 25, transform.position.z), transform.rotation);
        newSeed.setAngle(-.9f, -.25f);
        FireSeed bullet2 = (FireSeed)Instantiate(newSeed, new Vector3(transform.position.x + (-8f * transform.localScale.x), transform.position.y + 25, transform.position.z), transform.rotation);
        newSeed.setAngle(-.6f, -.4f);
        FireSeed bullet3 = (FireSeed)Instantiate(newSeed, new Vector3(transform.position.x + (-8f * transform.localScale.x), transform.position.y + 25, transform.position.z), transform.rotation);
    }

    void throwSeed()
    {
        Debug.Log("Throwing seed");
        FireSeed newSeed = (FireSeed)myFireball.GetComponent("FireSeed");
        newSeed.setAngle(Random.Range(-1f, 0), Random.Range(-1, 1));
        FireSeed floater = (FireSeed)Instantiate(newSeed, new Vector3(transform.position.x + (-8f * transform.localScale.x), transform.position.y + 30, transform.position.z), transform.rotation);
    }

}
