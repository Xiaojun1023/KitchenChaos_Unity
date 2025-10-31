using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public event EventHandler OnPickedSomething;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float move_speed = 5f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool isWalking;

    private Vector3 LastInteractDir;

    private BaseCounter selectedCounter;

    private KitchenObject kitchenObject;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying() == false)
        {
            return;
        }

        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying() == false)
        {
            return;
        }

        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteraction();
    }

    public bool IsWalking()
    {
        return isWalking;

    }

    private void HandleInteraction()
    {
        Vector2 InputVector = gameInput.GetMovementVectorNormalized();
        Vector3 MoveDir = new Vector3(InputVector.x, 0f, InputVector.y);

        if (MoveDir != Vector3.zero)
        {
            LastInteractDir = MoveDir;
        }

        float interact_distance = 2f;

        if (Physics.Raycast(transform.position, LastInteractDir, out RaycastHit raycastHit, interact_distance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter basecounter))
            {
                if (basecounter != selectedCounter)
                {
                    SetSelectedCounter(basecounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void HandleMovement()
    {
        Vector2 InputVector = gameInput.GetMovementVectorNormalized();
        Vector3 MoveDir = new Vector3(InputVector.x, 0f, InputVector.y);

        float move_distance = move_speed * Time.deltaTime;
        float player_radius = .7f;
        float player_height = 2f;
        bool can_move = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * player_height, player_radius, MoveDir, move_distance);

        if (!can_move)
        {
            // cannot move towards the direction, try to move sideways

            // Attempt only X movement
            Vector3 MoveDirX = new Vector3(MoveDir.x, 0f, 0f).normalized;
            can_move = (MoveDir.x < -0.5f || MoveDir.x > +0.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * player_height, player_radius, MoveDirX, move_distance);

            if (can_move)
            {
                // can move only on the X
                MoveDir = MoveDirX;
            }
            else
            {
                // cannot move towards the direction, try to move sideways

                // Attempt only Z movement
                Vector3 MoveDirZ = new Vector3(0f, 0f, MoveDir.z).normalized;
                can_move = (MoveDir.z < -0.5f || MoveDir.z > +0.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * player_height, player_radius, MoveDirZ, move_distance);

                if (can_move)
                {
                    // can move only on the Z
                    MoveDir = MoveDirZ;
                }
                else
                {
                    // cannot move in any direction, stay still
                }
            }
        }

        if (can_move)
        {
            transform.position += MoveDir * move_distance;
        }

        isWalking = MoveDir != Vector3.zero;

        float rotate_speed = 10f;

        if (MoveDir != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, MoveDir, Time.deltaTime * rotate_speed);
        }
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFolllowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
