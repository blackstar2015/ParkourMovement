using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region
    [TabGroup("Player Movement")] 
    public float MovementSpeed = 5f;
    [TabGroup("Player Movement")] 
    public float RotationSpeed = 720f;
    
    [TabGroup("Player Physics"), FoldoutGroup("_DefaultTabGroup/Player Physics/Grounding", expanded: false)]
    public float GroundCheckRadius = .3f;    
    [TabGroup("Player Physics"), FoldoutGroup("_DefaultTabGroup/Player Physics/Grounding", expanded: false)]
    public Vector3 GroundCheckOffset;    
    [TabGroup("Player Physics"), FoldoutGroup("_DefaultTabGroup/Player Physics/Grounding", expanded: false)]
    public LayerMask GroundLayer;    
    [SerializeField,TabGroup("Player Physics"), FoldoutGroup("_DefaultTabGroup/Player Physics/Grounding", expanded: false),HideInEditorMode]
    private bool _isGrounded;
    [SerializeField,TabGroup("Player Physics"), FoldoutGroup("_DefaultTabGroup/Player Physics/Airborne", expanded: false)]
    private float _fallingSpeed;

    [field: SerializeField, TabGroup("Components")] 
    private MainCameraController MCC;
    [field: SerializeField, TabGroup("Components")] 
    private Animator Animator;
    [field: SerializeField, TabGroup("Components")] 
    private CharacterController CharacterController;

    private Quaternion requiredRotation;
    private Vector2 _moveInput2D;
    private Vector3 _moveDir;
    private bool _playerControl = true;
    #endregion

    private void OnValidate()
    {
        Animator = GetComponent<Animator>();
        CharacterController = GetComponent<CharacterController>();
    }

    public void OnMove(InputValue inputValue)
    {
        _moveInput2D = inputValue.Get<Vector2>();
    }
    private void PlayerMovement()
    {
        float horizontal = _moveInput2D.x;
        float vertical = _moveInput2D.y;
        float movementAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

        //Setting MovementValue parameter in the animator
        Animator.SetFloat("MovementValue",movementAmount,.1f,Time.deltaTime);

        Vector3 movementInput = new Vector3(horizontal,0,vertical).normalized;
        var movementDirection = MCC.flatRotation * movementInput;
        _moveDir = movementDirection;
        CharacterController.Move(movementDirection * MovementSpeed * Time.deltaTime);

        if(movementAmount > 0)
        {
            requiredRotation = Quaternion.LookRotation(movementDirection);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation,requiredRotation,RotationSpeed * Time.deltaTime);
    }

    private void GroundCheck()
    {
        _isGrounded = Physics.CheckSphere(transform.TransformPoint(GroundCheckOffset), GroundCheckRadius, GroundLayer);
    }

    private void Update()
    {
        PlayerMovement();
        if (!_playerControl) return;
        if (_isGrounded)
        {
            _fallingSpeed = 0f;
        }
        else
        {
            _fallingSpeed += Physics.gravity.y * Time.deltaTime;
        }

        var velocity = _moveDir * MovementSpeed;
        velocity.y = _fallingSpeed;

        GroundCheck();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.TransformPoint(GroundCheckOffset), GroundCheckRadius);
    }

    public void SetControl(bool hasControl)
    {
        _playerControl = hasControl;
        CharacterController.enabled = hasControl;

        if (!hasControl)
        {
            Animator.SetFloat("MovementValue", 0);
            requiredRotation = transform.rotation;
        }
    }
}
