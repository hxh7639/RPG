using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI;


namespace RPG.Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] float stoppingDistance = 1f;
        [SerializeField] float moveSpeedMultiplier = 0.7f;
        [SerializeField] float movingTurnSpeed = 360;
        [SerializeField] float stationaryTurnSpeed = 180;
        [SerializeField] float moveThreshold = 1f;
        [SerializeField] float animationSpeedMultiplier = 1.5f;


        Vector3 clickPoint;

        NavMeshAgent agent;
        Animator animator;
        Rigidbody rigidBody;
        float turnAmount;
        float forwardAmount;


        void Start()
        {
            CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();


            animator = GetComponent<Animator>();
            animator.applyRootMotion = true;  //TODO  consider if needed

            rigidBody = GetComponent<Rigidbody>();
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;

            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updatePosition = true;
            agent.stoppingDistance = stoppingDistance;

            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;  // add function to do to observe onMouseOverEnemy event (when event happenes, tells observer, and observer does this function)
        }



        void Update()
        {
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                Move(agent.desiredVelocity);
            }
            else
            {
                Move(Vector3.zero);
            }
        }

        public void Move(Vector3 movement)
        {
            SetForwardAndTurn(movement);
            ApplyExtraTurnRotation();
            UpdateAnimator();
        }

        void SetForwardAndTurn(Vector3 movement)
        {
            // convert the world relative moveInput vector into a local-relative
            // turn amount and forward amount required to head in the desired direction
            if (movement.magnitude > moveThreshold)
            {
                movement.Normalize();
            }
            var localMove = transform.InverseTransformDirection(movement);
            turnAmount = Mathf.Atan2(localMove.x, localMove.z);
            forwardAmount = localMove.z;
        }

        void UpdateAnimator()
        {
            animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
            animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
            animator.speed = animationSpeedMultiplier;

        }


        void ApplyExtraTurnRotation()
        {
            // help the character turn faster (this is in addition to root rotation in the animation)
            float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        }

        void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                walkTarget.transform.position = destination;
                agent.SetDestination(destination);
            }
        }

        void OnMouseOverEnemy(Enemy enemy) // pass in enemy
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                // navigate to the enemy
                agent.SetDestination(enemy.transform.position);
            }
        }

        void OnAnimatorMove()
        {
            // we implement this function to override the default root motion.
            // this allows us to modify the positional speed before it's applied.
            if (Time.deltaTime > 0)
            {
                Vector3 velocity = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

                // we preserve the existing y part of the current velocity.
                velocity.y = rigidBody.velocity.y;
                rigidBody.velocity = velocity;
            }
        }





        //	void OnDrawGizmos()
        //	{
        //		// Draw movement Gizmos
        //		Gizmos.color = Color.black;
        //		Gizmos.DrawLine (transform.position, clickPoint);
        //		Gizmos.DrawSphere (currentDestination, 0.1f);
        //        Gizmos.DrawSphere(clickPoint, 0.15f);
        //
        //        // Draw attack sphere
        //        Gizmos.color = new Color(255f, 0f, 0, .3f);
        //        Gizmos.DrawWireSphere(transform.position, stopToAttackRadius);
        //        // try out different gizmos
        //    }



    }
}
