using UnityEngine;
using UnityEngine.Events;

namespace SolerSoft.ControllerColo
{
    /// <summary>
    /// Localizes content in a multi-user experience.
    /// </summary>
    public class ColocationManager : MonoBehaviour
    {
        #region Constants

        /// <summary>
        /// The default buttons that should be held down to trigger colocation by controller.
        /// </summary>
        private static readonly OVRInput.Button[] s_ColocationButtons = { OVRInput.Button.One, OVRInput.Button.Two, OVRInput.Button.PrimaryIndexTrigger };

        #endregion Constants

        #region Private Fields

        private bool m_isColocatingToController;
        private bool m_isFirstColocationCompleted;

        #endregion Private Fields

        #region Unity Inspector Variables

        [Header("Camera Settings")]
        [SerializeField]
        [Tooltip("Whether to affect camera pitch during colocation. This is usually unchecked.")]
        private bool m_affectPitch;

        [SerializeField]
        [Tooltip("Whether to affect camera roll during colocation. This is usually unchecked.")]
        private bool m_affectRoll;

        [SerializeField]
        [Tooltip("The camera rig that should be moved when colocated. This must be assigned.")]
        private Transform m_cameraRig;

        [Header("Controller Settings")]
        [SerializeField]
        [Tooltip("The buttons that should be held down to trigger colocation by controller.")]
        private OVRInput.Button[] m_colocateButtons = s_ColocationButtons;

        [SerializeField]
        [Tooltip("The controller that should be used for controller-based colocation. This must be assigned.")]
        private OVRControllerHelper m_controller;

        [Header("Offset Settings")]
        [SerializeField]
        [Tooltip("The positional offset of from the tracked device to consider the 'floor'. This offset might represent the height of a mount for the tracked device.")]
        private Vector3 m_positionOffset;

        [SerializeField]
        [Tooltip("The rotational offset of from the tracked device to consider 'level'. This angle might represent a mount or holder for the tracked device.")]
        private Vector3 m_rotationOffset;

        #endregion Unity Inspector Variables

        #region Unity Event Variables

        [Header("Events")]
        [Tooltip("Raised any time colocation is completed.")]
        [SerializeField] private UnityEvent m_onColocationCompleted;

        [Tooltip("Raised only the first time colocation is completed.")]
        [SerializeField] private UnityEvent m_onFirstColocationCompleted;

        #endregion Unity Event Variables

        #region Private Methods

        /// <summary>
        /// Attempts to colocate to the controller.
        /// </summary>
        private bool TryColocateToController()
        {
            // If no controller defined, can't attempt to colocate to controller
            if (m_controller == null)
            {
                Debug.LogError("Cannot colocate to controller because the controller has not been specified.");
                return false;
            }
            if ((m_controller.m_controller != OVRInput.Controller.LTouch) && (m_controller.m_controller != OVRInput.Controller.RTouch))
            {
                Debug.LogError("Cannot colocate to controller because only LTouch or RTouch are supported.");
                return false;
            }

            // Check to see if all the right buttons are held down on the target controller
            if (OVRInputHelper.GetAll(m_controller.m_controller, m_colocateButtons))
            {
                // Avoid re-entrance
                if (!m_isColocatingToController)
                {
                    // We are now co-locating
                    m_isColocatingToController = true;

                    // Colocate to world reference
                    ColocateTo(m_controller.gameObject.transform);

                    // Success
                    return true;
                }
            }
            else
            {
                // OK to check again on next frame
                m_isColocatingToController = false;
            }

            // Not colocated
            return false;
        }

        #endregion Private Methods

        #region Unity Message Handlers

        /// <inheritdoc />
        protected virtual void Start()
        {
            // Verify all dependencies are met
            if (m_cameraRig == null)
            {
                Debug.LogError($"The player rig must be specified. {nameof(ColocationManager)} will be disabled.");
                enabled = false;
                return;
            }

            if (m_controller == null)
            {
                Debug.LogError($"Controller helper must be specified. {nameof(ColocationManager)} will be disabled.");
                enabled = false;
                return;
            }
        }

        /// <inheritdoc />
        protected virtual void Update()
        {
            TryColocateToController();
        }

        #endregion Unity Message Handlers

        #region Public Methods

        /// <summary>
        /// Colocates to the specified transform.
        /// </summary>
        /// <param name="transform">
        /// The <see cref="Transform" /> to colocate to.
        /// </param>
        public void ColocateTo(Transform transform)
        {
            // Calculate the offset
            Quaternion rotationOffset = Quaternion.Euler(m_rotationOffset);

            // The new rotation is the inverse of the target rotation multiplied by the current rotation
            m_cameraRig.rotation = Quaternion.Inverse(transform.rotation * rotationOffset) * m_cameraRig.rotation;

            // Now limit rotations to affected axis
            m_cameraRig.rotation = Quaternion.Euler(m_affectRoll ? m_cameraRig.rotation.eulerAngles.x : 0, m_cameraRig.rotation.eulerAngles.y, m_affectPitch ? m_cameraRig.rotation.eulerAngles.z : 0);

            // The new position is the old position offset by the target transforms NEGATIVE amount
            m_cameraRig.transform.position = m_cameraRig.transform.position + -(transform.position + m_positionOffset);

            // Raise first time event?
            if (!m_isFirstColocationCompleted)
            {
                m_isFirstColocationCompleted = true;
                OnFirstColocationCompleted.Invoke();
            }

            // Raise event
            OnColocationCompleted.Invoke();
        }

        #endregion Public Methods

        #region Public Properties

        /// <summary>
        /// Gets or sets whether to affect camera pitch during colocation. This is usually false.
        /// </summary>
        public bool AffectPitch { get => m_affectPitch; set => m_affectPitch = value; }

        /// <summary>
        /// Gets or sets whether to affect camera roll during colocation. This is usually false.
        /// </summary>
        public bool AffectRoll { get => m_affectRoll; set => m_affectRoll = value; }

        /// <summary>
        /// Gets or sets the camera rig that should be moved when colocated. This must be supplied.
        /// </summary>
        public Transform CameraRig { get => m_cameraRig; set => m_cameraRig = value; }

        /// <summary>
        /// Gets or sets the buttons that should be held down to trigger colocation by controller.
        /// </summary>
        public OVRInput.Button[] ColocateButtons { get => m_colocateButtons; set => m_colocateButtons = value; }

        /// <summary>
        /// The controller helper that represents the controller to be used for colocation. This
        /// must be supplied.
        /// </summary>
        public OVRControllerHelper ControllerHelper { get => m_controller; set => m_controller = value; }

        /// <summary>
        /// Raised any time colocation has completed.
        /// </summary>
        public UnityEvent OnColocationCompleted { get => m_onColocationCompleted; set => m_onColocationCompleted = value; }

        /// <summary>
        /// Raised only the first time colocation has completed.
        /// </summary>
        public UnityEvent OnFirstColocationCompleted { get => m_onFirstColocationCompleted; set => m_onFirstColocationCompleted = value; }

        #endregion Public Properties
    }
}