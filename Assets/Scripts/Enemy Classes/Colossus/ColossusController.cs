using UnityEngine;
using System.Collections;

public class ColossusController : MonoBehaviour {

    
    #region has hoe timer;
    public float overhandCooldown = 8f;
    public float pokeCooldown = 3f;
    public float swingCooldown = 6f;

    public float overhandTimer = 0f;
    public float pokeTimer = 0f;
    public float swingTimer = 0f;

    private bool canOverHand = false;
    private bool canPoke = false;
    private bool canSwing = false;
#endregion

    #region no hoe cooldowns
    public float shoulderCooldown = 4f;
    public float stompCooldown = 4f;

    public float shoulderTimer = 0f;
    public float stompTimer = 0f;

    private bool canShoulder = false;
    private bool canStomp = false;
    #endregion

    #region Movement Values
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
    private bool left = true;
    #endregion

    public float currentAttackTimer = 0;
    public string currentAttack = "idle";

    // Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController2D>();

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;

        currentAttack = "idle";
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
	
	// Update is called once per frame
	void Update () {
        _velocity = _controller.velocity;

        if (currentAttackTimer != 0)
        {
            currentAttackTimer += Time.deltaTime;
        }

        if (overhandTimer != 0)
        {
            overhandTimer += Time.deltaTime;
            if (overhandTimer > overhandCooldown)
            {
                overhandTimer = 0;
            }
        }

        if (swingTimer != 0)
        {
            swingTimer += Time.deltaTime;
            if (swingTimer > swingCooldown)
            {
                swingTimer = 0;
            }
        }

        if (pokeTimer != 0)
        {
            pokeTimer += Time.deltaTime;
            if (pokeTimer > pokeCooldown)
            {
                pokeTimer = 0;
            }
        }

       
        #region attackLogic
        if (currentAttackTimer > 1.1f || currentAttackTimer == 0) {
            
            if (currentAttackTimer > 1.1f)
            {
                currentAttackTimer = 0;
                currentAttack = "idle";
            }
            if (isInRange())
            {
                if (canOverHand && overhandTimer == 0)
                {
                    overheadAttack();
                }
                else if (canSwing && swingTimer == 0)
                {
                    swingAttack();
                }
                else if (canPoke && pokeTimer == 0)
                {
                    pokeAttack();
                }
                else
                {
                    currentAttack = "idle";
                }
                normalizedHorizontalSpeed = 0;
            }
            else
            {
                if (left)
                {
                    normalizedHorizontalSpeed = -1;
                    if (transform.localScale.x > 0f)
                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    _velocity.x = normalizedHorizontalSpeed * runSpeed * Time.deltaTime;

                }
                else
                {
                    if (transform.localScale.x < 0f)
                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    normalizedHorizontalSpeed = 1;
                    _velocity.x = normalizedHorizontalSpeed * runSpeed * Time.deltaTime;
               }
            }
        }
        _velocity.x = normalizedHorizontalSpeed * runSpeed;

        
        #endregion

        #region Verticle movement
        if (!_controller.isGrounded)
        {
            _velocity.y += gravity * Time.deltaTime;
        }else
        { 
            _velocity.y = 0;
        }

       if (_velocity.x != 0)
        {
            _animator.Play(Animator.StringToHash("Walk"));
        }
        else if(currentAttack.Equals("idle"))
        {
            _animator.Play(Animator.StringToHash("ColossusIdle"));
        }
        
        
        _controller.move(_velocity * Time.deltaTime);
        #endregion
    }

    #region Attack methods
    void overheadAttack()
    {
        _animator.Play(Animator.StringToHash("Overhand"));
        overhandTimer += Time.deltaTime;
        currentAttack = "overhand";
        currentAttackTimer += Time.deltaTime;
    }

    void swingAttack()
    {
        Debug.Log("Called swing");
        _animator.Play(Animator.StringToHash("ColossusSwing"));
        swingTimer += Time.deltaTime;
        currentAttack = "swing";
        currentAttackTimer += Time.deltaTime;
    }

    void pokeAttack()
    {
        _animator.Play(Animator.StringToHash("Poke"));
        pokeTimer += Time.deltaTime;
        currentAttack = "poke";
        currentAttackTimer += Time.deltaTime;
    }

    public void updateCanAttack(string attackName, bool canUse){
        switch (attackName){
        case"overhand":    
            canOverHand = canUse;
            break;
        case "poke":
            canPoke = canUse;
            break;
        case "swing":
            canSwing = canUse;
            break;
        default:
            break;
              
        }
    }
    #endregion

    bool isInRange()
    {
        return canOverHand || canPoke || canSwing;
    }

    public int getCurrentAttackValue()
    {
        switch (currentAttack)
        {
            case "swing":
                return 30;
            case "overhand":
                return 20;
            case "poke":
                return 10;
            case "idle":
                return 0;
            default:
                return 0;
        }
    }
}
