using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public static event System.Action<int> OnEmoteInput; 
    
    [SerializeField] private float moveSpeed;
    private Camera mainCamera;
    private Vector3 mouseInput = Vector3.zero;
    
    private PlayerAmmo playerAmmo;
    
    private void Initialize()
    {   
        mainCamera = Camera.main;
        playerAmmo = GetComponent<PlayerAmmo>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Initialize();
    }

    private void Update()
    {
        if(!IsOwner || !Application.isFocused) return;
        
        // Movement
        mouseInput.x = Input.mousePosition.x;
        mouseInput.y = Input.mousePosition.y;
        mouseInput.z = mainCamera.nearClipPlane;
        
        Vector3 mouseWorldCoordinates = mainCamera.ScreenToWorldPoint(mouseInput);
        mouseWorldCoordinates.z = 0f;
        transform.position = Vector3.MoveTowards(transform.position, mouseWorldCoordinates, Time.deltaTime * moveSpeed);
        
        // Rotate
        if (mouseWorldCoordinates != transform.position)
        {
            Vector3 targetDirection = mouseWorldCoordinates - transform.position;
            targetDirection.z = 0f;
            transform.up = targetDirection;
        }
        
        //Shoot
        if (Input.GetMouseButtonDown(0))
        {
            playerAmmo.Shoot();
        }
        
        //Emote inputs
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnEmoteInput?.Invoke(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnEmoteInput?.Invoke(2);
        }
    }
}
