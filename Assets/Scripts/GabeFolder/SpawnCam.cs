using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[ExecuteAlways]
public class SpawnCam : MonoBehaviour
{
    private Camera mainCam;
    [Header("Main Camera Settings")]
    [SerializeField, Tooltip("How far back the camera is for orthographic.")]
    private float orthoSize = 14.75f;
    [SerializeField, Tooltip("Type of Background")]
    private CameraClearFlags bType = CameraClearFlags.Skybox;

    private CinemachineVirtualCamera virtCam;
    private CinemachineTransposer transposer;
    [Header("Virtual Camera Settings")]
    [SerializeField, Tooltip("Rotation of the Camera")]
    private Quaternion rotation = Quaternion.Euler(45, 0, 0);
    [SerializeField, Tooltip("How far back the camera is for orthographic on the Virtual Camera.")]
    private float vOrthoSize = 14.75f;
    [SerializeField, Tooltip("How close the clipping for the camera will occur")]
    private float nearClipPane = -20;
    [SerializeField, Tooltip("How far the clipping for the camera will occur")]
    private float farClipPane = 5000;
    [SerializeField, Tooltip("Offset of the Camera")]
    private Vector3 offset = new(0, 20, -20);
    [SerializeField, Tooltip("Dampening of the Camera")]
    private Vector3 dampening = new(0.3f, 0.3f, 0.3f);

    [Header("Clipping Hitbox Settings")]
    [SerializeField, Tooltip("Size of the collider")]
    private Vector3 size = new Vector3(20, 20, 20);
    private bool isTrigger = true;
    private bool isKinematic = true;


    public void OnEnable() {
        if(Camera.main != null && !Camera.main.TryGetComponent(out CinemachineBrain brain)) {
            DestroyImmediate(Camera.main.gameObject);
        }

        if(Camera.main == null) {
            GameObject go = new();
            go.name = "Main Camera";
            go.tag = "MainCamera";
            go.transform.rotation = rotation;
            go.layer = LayerMask.NameToLayer("Camera");
            go.AddComponent<CinemachineBrain>();
            mainCam = go.AddComponent<Camera>();
            //Projection
            mainCam.orthographic = true;
            mainCam.orthographicSize = orthoSize;
            //Environment
            mainCam.clearFlags = bType;
            var collider = go.AddComponent<BoxCollider>();
            collider.size = size;
            collider.isTrigger = isTrigger;
            var rb = go.AddComponent<Rigidbody>();
            rb.isKinematic = isKinematic;
            go.AddComponent<CamThruWall>();
        }

        if(GameObject.FindGameObjectWithTag("VirtCam") == null) {
            GameObject go = new();
            go.name = "Virtual Camera";
            go.tag = "VirtCam";
            go.transform.rotation = rotation;
            go.layer = LayerMask.NameToLayer("Camera");
            virtCam = go.AddComponent<CinemachineVirtualCamera>();
            virtCam.Follow = GameObject.FindGameObjectWithTag("Player").transform;
            virtCam.m_Lens.Orthographic = true;
            virtCam.m_Lens.OrthographicSize = vOrthoSize;
            virtCam.m_Lens.NearClipPlane = nearClipPane;
            virtCam.m_Lens.FarClipPlane = farClipPane;
            transposer = virtCam.AddCinemachineComponent<CinemachineTransposer>();
            transposer.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;
            transposer.m_FollowOffset.Set(offset.x, offset.y, offset.z);
            transposer.m_XDamping = dampening.x;
            transposer.m_YDamping = dampening.y;
            transposer.m_ZDamping = dampening.z;
        }
    }
}
