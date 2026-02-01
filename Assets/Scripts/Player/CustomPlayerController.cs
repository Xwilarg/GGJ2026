using GGJ2026.Manager;
using GGJ2026.Prop;
using GGJ2026.SO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace GGJ2026.Player
{
    public class CustomPlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInfo _info;
        [SerializeField]
        private Transform _spritesHolder;
        [SerializeField]
        private Transform _raycastTarget;
        [SerializeField]
        private TMP_Text _interactionText;

        [SerializeField]
        private Animator _animPlayer, _animMask;

        private SpriteRenderer _sr;
        private Rigidbody _rb;
        private Vector2 _rawMov;
        private Camera _cam;
        private float _yJumpForce;

        private bool _canJump = true;
        private bool _isMidAirAfterJump = false;

        private SFXPlayerController _sfxController;
        private float _footstepDelay;

        private IInteractible _currentInteraction;

        private List<Wall> _lastSeenWalls = new();

        private float _maskSwitchTimer;

        private void Awake()
        {
            _sr = GetComponentInChildren<SpriteRenderer>();
            _rb = GetComponent<Rigidbody>();
            _cam = Camera.main;
            _animPlayer = GetComponentInChildren<Animator>();
            _sfxController = GetComponent<SFXPlayerController>();

            _interactionText.gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IInteractible>(out var interaction) && interaction.CanInteract(this))
            {
                if (_currentInteraction != null)
                {
                    _currentInteraction.CancelInteraction(this);
                }
                _currentInteraction = interaction;
                _currentInteraction.Prepare(this);
                _interactionText.gameObject.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<IInteractible>(out var interaction) && _currentInteraction != null && interaction.Key == _currentInteraction.Key)
            {
                _currentInteraction.CancelInteraction(this);
                _currentInteraction = null;
                _interactionText.gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            var counter = 1;

            var masks = GameManager.Instance.GetAllMasks();
            foreach (var mask in masks)
            {
                if (mask.Sprite == null) continue;

                AddButton(mask, counter);

                counter++;
            }

            MaskManager.Instance.CurrentMask = masks[0].Type;

            MaskManager.Instance.OnMaskChange.AddListener((mask) =>
            {
                _maskSwitchTimer = 0f;
                _animMask.Play("Switch");
            });
        }

        private void AddButton(MaskInfo mask, int counter)
        {
            var btn = UIManager.Instance.AddButton(mask.Sprite, counter);
            btn.onClick.AddListener(() =>
            {
                MaskManager.Instance.CurrentMask = mask.Type;
            });
            MaskManager.Instance.AddMask(btn, mask.Type);
        }

        private void Update()
        {
            if (_rawMov.x != 0f)
            {
                Vector3 spritesScale = _spritesHolder.transform.localScale;

                spritesScale.x = _rawMov.x < 0 ? Mathf.Abs(spritesScale.x) : -Mathf.Abs(spritesScale.x);

                _spritesHolder.transform.localScale = spritesScale;
            }

            /*if (_yJumpForce > 0f)
            {
                _yJumpForce -= Time.deltaTime * _info.SimulatedGravityForce;
                _rb.AddForce(Vector3.up * _yJumpForce);
            }*/

            if (_maskSwitchTimer < 1f)
            {
                _maskSwitchTimer += Time.deltaTime;

                if (_maskSwitchTimer >= 1f)
                {
                    _animMask.Play(GameManager.Instance.GetMask(MaskManager.Instance.CurrentMask).AnimationName);
                }
            }
        }

        private void FixedUpdate()
        {
            Vector3 mov = UIManager.Instance.IsInEnding ? Vector3.zero : (_cam.transform.forward * _rawMov.y + _cam.transform.right * _rawMov.x);
            mov.y = 0f;

            if (_isMidAirAfterJump && IsOnFloor)
            {
                _isMidAirAfterJump = false;
                _sfxController.PlayRandomJumpLand();
            }

            if (!_isMidAirAfterJump && mov.magnitude > 0f)
            {
                _footstepDelay -= Time.fixedDeltaTime;
                if (_footstepDelay < 0f)
                {
                    _sfxController.PlayRandomFootstep();
                    _footstepDelay = _info.FootstepInterval;
                }
            }

            _animPlayer.SetBool("IsWalking", mov.magnitude > 0f);
            _animPlayer.SetBool("IsMidAir", _isMidAirAfterJump);

            CheckWallForTransparency();

            var dir = mov.normalized * _info.MovementSpeed;
            _rb.linearVelocity = new Vector3(dir.x, _rb.linearVelocity.y, dir.z);
        }

        private bool IsOnFloor => Physics.Raycast(transform.position, Vector3.down, _info.MinDistanceWithFloorForJump, LayerMask.GetMask("World"));
        private bool CanJump => _canJump && IsOnFloor;

        private void OnMaskSelect(InputAction.CallbackContext value, int key)
        {
            if (value.phase == InputActionPhase.Started && !UIManager.Instance.IsInEnding)
            {
                var mask = MaskManager.Instance.TryGetMask(key - 1);
                if (mask == null) return;

                if (mask.MaskType == MaskManager.Instance.CurrentMask) MaskManager.Instance.CurrentMask = MaskType.None;
                else mask.Button.onClick.Invoke();
            }
        }

        public void OnMove(InputAction.CallbackContext value)
        {
            _rawMov = value.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started && CanJump && !UIManager.Instance.IsInEnding)
            {
                _canJump = false;
                _rb.AddForce(Vector3.up * _info.JumpForce, ForceMode.Impulse);
                //_rb.linearVelocity = new(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
                //_yJumpForce = _info.JumpForce;
                StartCoroutine(RefreshJump());

                _isMidAirAfterJump = true;
                _animPlayer.SetTrigger("Jump");
                _sfxController.PlayRandomJump();
            }
        }

        public void OnInteract(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started)
            {
                if (UIManager.Instance.IsInEnding)
                {
                    UIManager.Instance.ShowNextDialogue();
                }
                else if (_currentInteraction != null)
                {
                    _currentInteraction.Interact(this);
                    _interactionText.gameObject.SetActive(false);
                }
            }
        }

        public void OnCrouch(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started) _animPlayer.SetBool("IsCrouching", true);
            else if (value.phase == InputActionPhase.Canceled) _animPlayer.SetBool("IsCrouching", false);
        }

        public void OnRestart(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started)
            {
                SceneManager.LoadScene("Main");
            }
        }

        public void OnMaskSelect1(InputAction.CallbackContext value) => OnMaskSelect(value, 1);
        public void OnMaskSelect2(InputAction.CallbackContext value) => OnMaskSelect(value, 2);
        public void OnMaskSelect3(InputAction.CallbackContext value) => OnMaskSelect(value, 3);
        public void OnMaskSelect4(InputAction.CallbackContext value) => OnMaskSelect(value, 4);
        public void OnMaskSelect5(InputAction.CallbackContext value) => OnMaskSelect(value, 5);
        public void OnMaskSelect6(InputAction.CallbackContext value) => OnMaskSelect(value, 6);
        public void OnMaskSelect7(InputAction.CallbackContext value) => OnMaskSelect(value, 7);
        public void OnMaskSelect8(InputAction.CallbackContext value) => OnMaskSelect(value, 8);
        public void OnMaskSelect9(InputAction.CallbackContext value) => OnMaskSelect(value, 9);

        private IEnumerator RefreshJump()
        {
            yield return new WaitForSeconds(1f);
            _canJump = true;
        }

        private void CheckWallForTransparency()
        {
            RaycastHit[] hits;
            var target = _cam.transform;
            hits = Physics.SphereCastAll(_raycastTarget.position, 4, target.position - _raycastTarget.position, Vector3.Distance(_raycastTarget.position, target.position), LayerMask.GetMask("World"));

            // Create a list to track currently visible walls
            List<Wall> currentlyVisibleWalls = new();

            if (hits.Length > 0)
            {
                foreach (var hit in hits)
                {
                    var wall = hit.transform.GetComponent<Wall>();
                    if (wall != null)
                    {
                        wall.BeingLookedAt = true;
                        wall.HandleTransparency();
                        currentlyVisibleWalls.Add(wall);
                    }
                }
            }

            // Reset walls that are no longer being looked at
            foreach (var previousWall in _lastSeenWalls)
            {
                if (!currentlyVisibleWalls.Contains(previousWall))
                {
                    previousWall.BeingLookedAt = false;
                    previousWall.HandleTransparency();
                }
            }

            // Update the list of currently seen walls
            _lastSeenWalls.Clear();
            _lastSeenWalls.AddRange(currentlyVisibleWalls);
        }
    }
}
