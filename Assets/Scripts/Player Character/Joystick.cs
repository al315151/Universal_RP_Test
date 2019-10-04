using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : Character
{
    // Movement variables
    private bool isGrounded = false, isJumping = false, isDashing = false;


    private Vector3 movementDirection;
    private Vector3 movement;
    private float movementSpeed = 2.5f;

    private bool dashAvailable = true;
    private float dashMaxVelocity = 7.5f;
    private float dashTime = 0.25f;
    private float dashCooldown = 1f;
    private float dashVelocity;

    private float jumpSpeed = 6.5f;
    private float gravityMultiplier = 3f;
    private float gravityJumpMultiplier = 1f;
    private float maxFallingSpeed = 15f;

    // Animations
    [Header("Animations")]
    //[HideInInspector]
    public Transform model;
    private Animator animator;
    private const string triggerTrigger = "Trigger";
    private const string floatVelocity = "Velocity";

    private const float tiltSmoothTime = 0.075f;
    private const float rotationMaxDegrees = 720f;
    private float tiltVelocity;

    // Appearance variables
    private const string skinPath = "Characters/Joystick/Skins/";
    private MeshRenderer[] meshRenderers;


    // Components
    //[HideInInspector]
    public PlayerSoundManagement playerSoundReference;
    private CharacterController characterController;

    // ParticleSystems
    //[HideInInspector]
    public ParticleSystem walkParticleSystem, landParticleSystem;
    private bool wasGrounded = true;


    public override void SetUpAppearance(int skin)
    {
        // Change the materials based on the skin number.
        Material[] materials = Resources.LoadAll<Material>(skinPath + skin);
        
        foreach(MeshRenderer meshRenderer in meshRenderers)
        {
            Material[] newMaterials = meshRenderer.sharedMaterials;

            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = materials[ int.Parse(newMaterials[i].name[0].ToString())];
            }

            meshRenderer.sharedMaterials = newMaterials;
        }
    }

    void Awake()
    {      
        animator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    public override void Update()
    {
        // Inputs.
        base.Update();

        // Animations.
        if (inputEnabled)
        {
            animator.SetFloat(floatVelocity, Mathf.SmoothDamp(animator.GetFloat(floatVelocity), movementDirection.magnitude, ref tiltVelocity, tiltSmoothTime));
            
            if (movementDirection.magnitude > 0f)
            {
                model.transform.rotation = Quaternion.LookRotation(movementDirection,Vector3.up);
            } 
        }
    }

    private void FixedUpdate()
    {
        if (inputEnabled)
        {
            // Ground test. (characterController.isGrounded will only return true when the character is close to the ground and has a downwards force going on. That means it won't be true when ascending ramps)
            wasGrounded = isGrounded;
            isGrounded = characterController.isGrounded || Physics.SphereCast(new Ray(transform.position + characterController.center, Vector3.down), characterController.radius, 0.1f, CubeManager.LayerMask, QueryTriggerInteraction.Ignore);


            // Walk Particles
            if (!inputEnabled || walkParticleSystem.isPlaying && !isGrounded)
            {
                walkParticleSystem.Stop();
            }
            else if (!walkParticleSystem.isPlaying && isGrounded)
            {
                walkParticleSystem.Play();
            }

            // Land Particles
            if (!wasGrounded && isGrounded)
            {
                landParticleSystem.Play();
            }

            // Set movement direction.
            movementDirection = Vector3.ClampMagnitude(horizontalAxis * horizontalInput + verticalAxis * verticalInput, 1);

            // Movement = input + vertical movement from the previous frame. Also, add dash in case it is being used
            movement = movementDirection * movementSpeed + Vector3.up * (characterController.velocity.y);

            // Apply and calculate dash velocity
            if (isDashing)
            {
                movement += Vector3.ProjectOnPlane(characterController.velocity, Vector3.up).normalized * dashVelocity;
            }
            else
            {
                if (dashAvailable && leftTriggerInput && movementDirection.magnitude > 0)
                {
                    StartCoroutine(Dash(dashTime, dashCooldown));
                }
            }

            // Apply gravity and constrain vertical velocity.
            movement += Physics.gravity * ((isJumping) ? gravityJumpMultiplier : gravityMultiplier) * Time.fixedDeltaTime;
            if (movement.y < -maxFallingSpeed) { movement.y = -maxFallingSpeed; }

            // Apply jump force.
            if (isJumping)
            {
                if (characterController.velocity.y <= 0 || !rightTriggerInput)
                {
                    isJumping = false;
                }
            }
            if (isGrounded && rightTriggerInput && !isJumping)
            {
                isJumping = true;
                movement.y = jumpSpeed;
                animator.SetTrigger(triggerTrigger);
                playerSoundReference.JumpSoundPlay();
            }

            // Apply Movement
            if (inputEnabled)
            {
                characterController.Move(movement * Time.fixedDeltaTime);
            }
            else
            {
                characterController.Move(Vector3.zero);
            }
        }
        else
        {
            characterController.Move(Vector3.zero);
        }
    }

    IEnumerator Dash(float seconds, float cooldownTime)
    {
        dashAvailable = false;
        isDashing = true;
        dashVelocity = 0f;

        float t = 0f, cooldown = 0f;
        float accelerateTime = seconds / 4f;

        // Dash
        while (t <= seconds && inputEnabled)
        {
            t += Time.fixedDeltaTime;

            // Accelerate
            if (t <= accelerateTime)
            {
                dashVelocity = Mathf.SmoothStep(0, dashMaxVelocity, t / accelerateTime);
            }
            // Brake
            else if (t >= seconds - accelerateTime)
            {
                dashVelocity = Mathf.SmoothStep(dashMaxVelocity, 0f, (t - seconds - accelerateTime) / accelerateTime);
            }
            yield return new WaitForFixedUpdate();
        }
        isDashing = false;

        // Cooldown
        while (cooldown <= cooldownTime && inputEnabled)
        {
            cooldown += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        dashAvailable = true;
        yield return null;
    }

    public override void Spawn()
    {
        throw new System.NotImplementedException();
    }
    public override void Death()
    {
        throw new System.NotImplementedException();
    }
    public override void Respawn()
    {
        throw new System.NotImplementedException();
    }
    public override void EnterGoal()
    {
        throw new System.NotImplementedException();
    }

    

  
}
