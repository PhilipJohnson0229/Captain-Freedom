using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // Start is called before the first frame update
    [field: SerializeField]public Rigidbody RB { get; private set; }
    [field: SerializeField] public Animator Anim { get; private set; }
    [field: SerializeField] public CapsuleCollider Col { get; private set; }

    public Transform[] wallCheckPoints;
    public Transform groundCheck;

    public float groundRange;
    public float wallRange;
    public LayerMask whatIsGround, whatIsObstacle;
    
    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        Anim = GetComponentInChildren<Animator>();
        RB = GetComponent<Rigidbody>();
        Col = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected virtual void FixedUpdate()
    {

    }

    public bool IsGroundDetected() => Physics.Raycast(groundCheck.position, Vector3.down, groundRange, whatIsGround);

    public bool IsWallDetected()
    {
        foreach (Transform wallCheck in wallCheckPoints)
        {
            if (Physics.Raycast(wallCheck.position, wallCheck.forward, wallRange, whatIsGround))
            {
                return true;
            }
        }
        return false;
    }

    protected virtual void OnDrawGizmos()
    {
        Vector3 Pos = groundCheck.position + (-transform.up * groundRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(groundCheck.position, Pos + (-transform.up * groundRange));

        if (wallCheckPoints.Length != 0)
        {
            Gizmos.color = Color.red;

            foreach (Transform wallCheck in wallCheckPoints)
            {
                if (wallCheck != null)
                {
                    Gizmos.DrawLine(wallCheck.position, wallCheck.position + wallCheck.forward * wallRange);
                }
            }
        }
    }
}
