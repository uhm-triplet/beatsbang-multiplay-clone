using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Cinemachine;

public class PlayerAim : MonoBehaviour
{

    public Cinemachine.AxisState xAxis, yAxis;
    [SerializeField] Transform camFollowPos;
    [HideInInspector] public CinemachineVirtualCamera vCam;

    public float zoomFov = 40;
    [HideInInspector] public float normalFov;
    [HideInInspector] public float currentFov;
    bool zoom = false;
    public float fovSmoothSpeed = 10;

    public Transform aimPos;
    // [HideInInspector] public Vector3 aimPos;
    [SerializeField] float aimSmoothSpeed = 50;
    [SerializeField] LayerMask aimMask;
    [SerializeField] GameObject camera;

    PlayerItem playerItem;

    void Awake()
    {

        camera.SetActive(true);
        vCam = GetComponentInChildren<CinemachineVirtualCamera>();
        normalFov = vCam.m_Lens.FieldOfView;
        playerItem = GetComponent<PlayerItem>();
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;
        currentFov = normalFov;
    }

    void Update()
    {

        if (playerItem.isDead) return;
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);

        vCam.m_Lens.FieldOfView = Mathf.Lerp(vCam.m_Lens.FieldOfView, currentFov, fovSmoothSpeed * Time.deltaTime);

        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimMask))
            aimPos.position = Vector3.Lerp(aimPos.position, hit.point, aimSmoothSpeed * Time.deltaTime);
        // aimPos = hit.point;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (zoom == false)
            {
                currentFov = zoomFov;
                zoom = true;
            }
            else
            {
                currentFov = normalFov;
                zoom = false;
            }
        }
    }

    private void LateUpdate()
    {
        if (!IsOwner) return;
        camFollowPos.localEulerAngles = new Vector3(yAxis.Value, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis.Value, transform.eulerAngles.z);
    }


}
