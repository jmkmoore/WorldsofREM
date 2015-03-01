using UnityEngine;
using System.Collections;


public class DemoScene : MonoBehaviour
{
	// movement config
	public float gravity = -25f;
	public float runSpeed = 8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;
    public float dashBoost = 15f;
    private int dashCount = 0;
    private int dashMax = 2;

    private float airDashTime = 0.167f;
    public float comboTime = 2f;
    public float comboCountdown = 0f;
    public int attackCount = 0;

	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;

    private CharacterController2D _controller;
    private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;

    private bool left = false;
    private bool right = true;

    public float ButtonDelay;
    float lastJump = 0;
    float lastUse = 0;
    private bool isDashing = false;
    BoxCollider2D myBoxCollider;

    public GameObject attackBox;

    void Awake()
	{
		_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController2D>();

		// listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;
        myBoxCollider = gameObject.GetComponent<BoxCollider2D>();
    }


	#region Event Listeners

	void onControllerCollider( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
			return;

		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
		//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}


	void onTriggerEnterEvent( Collider2D col )
	{
		Debug.Log( "onTriggerEnterEvent: " + col.tag);
        if (col.tag.Equals("DestructPlat"))
        {
            FallApart obj = (FallApart)col.gameObject.GetComponent<FallApart>();
            obj.destroyTimer += Time.deltaTime;
        }
        if (col.tag.Equals("DeathWall"))
        {
            gameObject.GetComponent<PlayerHealth>().adjustCurrentHealth(-100000);
        }

	}


	void onTriggerExitEvent( Collider2D col )
	{
		Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
	}

	#endregion


	// the Update loop contains a very simple example of moving the character around and controlling the animation
	void Update()
	{
        // grab our current _velocity to use as a base for all calculations
		_velocity = _controller.velocity;
        if (airDashTime != 0)
        {
            airDashTime += Time.deltaTime;
        }

        if (!isDashing)
        {
            if (_controller.isGrounded)
            {
                _velocity.y = 0;
                if (dashCount > 0)
                {
                    dashCount = 0;
                }
            }
        }

        if (airDashTime > 0.7f)
        {
            myBoxCollider.size = new Vector3(1.75f, 10);
            isDashing = false;
        }

        if (comboCountdown > comboTime)
        {
            attackCount = 0;
            comboCountdown = 0;
        }
        if (comboCountdown != 0 && attackCount != 0)
        {
            comboCountdown += Time.deltaTime;
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(attackCount == 0 || attackCount == 3){
                _animator.Play(Animator.StringToHash("Jab"));
                attackCount++;
                comboCountdown = 0;
                comboCountdown += Time.deltaTime;
                if (attackCount >= 3)
                {
                    attackCount = 1;
                }
                attack(10);
            }
            else if (attackCount == 1 && comboCountdown < comboTime)
            {
                _animator.Play(Animator.StringToHash("Cross"));
                attackCount++;
                comboCountdown = 0;
                comboCountdown += Time.deltaTime;
                attack(15);
            }
            else if(attackCount == 2 && comboCountdown < comboTime)
            {
                _animator.Play(Animator.StringToHash("Kick"));
                attackCount++;
                comboCountdown = 0;
                comboCountdown += Time.deltaTime;
                attack(20);
            }
        }

        
		else if( Input.GetKey( KeyCode.RightArrow ) )
		{
			normalizedHorizontalSpeed = 1;
			if( transform.localScale.x < 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
            right = true;
            left = false;

            if (!_controller.isGrounded)
                normalizedHorizontalSpeed = .75f;
        }
		else if( Input.GetKey( KeyCode.LeftArrow ) )
		{
			normalizedHorizontalSpeed = -1;
			if( transform.localScale.x > 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

			if( _controller.isGrounded )
			
            right = false;
            left = true;
            if (!_controller.isGrounded)
                normalizedHorizontalSpeed = -.75f;
		}
		else
		{
			normalizedHorizontalSpeed = 0;
        }

        #region Movement Animation
        if (_controller.isGrounded && normalizedHorizontalSpeed != 0 && !isDashing)
        {
                _animator.Play(Animator.StringToHash("Run"));
        }
        else if (_controller.isGrounded && normalizedHorizontalSpeed == 0 && !isDashing && comboCountdown == 0)
        {
            _animator.Play(Animator.StringToHash("Idle"));
        }
        #endregion

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (dashCount < dashMax)
            {
                if (left)
                {
                    normalizedHorizontalSpeed = -1;
                }
                else
                    normalizedHorizontalSpeed = 1;
                isDashing = true;
                dashCount++;
                _animator.Play(Animator.StringToHash("Airdash"));
            }
        }

        if (isDashing)
        {
            if (left)
            {
                normalizedHorizontalSpeed = -1;
            }
            else
                normalizedHorizontalSpeed = 1;

        }

		// we can only jump whilst grounded
		if( _controller.isGrounded && Input.GetKeyDown( KeyCode.UpArrow ) )
		{
			_velocity.y = Mathf.Sqrt( 2f * jumpHeight * -gravity );
			_animator.Play( Animator.StringToHash( "Jump" ) );
		}


        if(_controller.isGrounded && (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)) && _velocity.x == 0 && _velocity.y == 0 && attackCount == 0){
            _animator.Play(Animator.StringToHash("Idle"));
        }


		// apply horizontal speed smoothing it
		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?

        if (!isDashing)
        {
            _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);

            _velocity.y += gravity * Time.deltaTime;

            _controller.move(_velocity * Time.deltaTime);
        }
        else
        {
            _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed * dashBoost, Time.deltaTime * groundDamping);
            _velocity.y = 0;
            _controller.move(_velocity * Time.deltaTime);
            myBoxCollider.size = new Vector3(10f, 1.75f);
        }

        
	}

    public void attack(int attackDamage)
    {
        PlayerAttack attack = (PlayerAttack)attackBox.GetComponent<PlayerAttack>();
        attack.setAttackDamage(attackDamage);
        PlayerAttack attackClone = (PlayerAttack)Instantiate(attack, new Vector3(transform.position.x + (6f * transform.localScale.x), transform.position.y + 5.5f, transform.position.z), transform.rotation);
    }

    public void shoutOut()
    {

    }

}
