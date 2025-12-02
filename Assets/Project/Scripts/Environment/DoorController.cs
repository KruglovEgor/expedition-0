using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Expedition0.Environment
{
    public class DoorController : MonoBehaviour
    {
        [SerializeField] private bool useTrigger = true;
        [SerializeField] private bool locked = true;
        [SerializeField] private Animator animator;
        [SerializeField] private string openParameter = "IsOpen";
        [SerializeField] private bool startOpen = false;
        
        // Optional: State events
        public UnityEvent onDoorOpened;
        public UnityEvent onDoorClosed;
        public UnityEvent onDoorLocked;
        public UnityEvent onDoorUnlocked;
        
        private bool _isOpen = false;
        
        private void Awake()
        {
            if (!animator) animator = GetComponent<Animator>();
            
            // Set initial state
            _isOpen = startOpen;
            if (animator)
            {
                animator.SetBool(openParameter, startOpen);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!useTrigger) return;
            if (other.CompareTag("Player"))
            {
                TryOpenDoor();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!useTrigger) return;
            if (other.CompareTag("Player"))
            {
                TryCloseDoor();
            }
        }

        // Public methods for interaction
        public void TryToggleDoor()
        {
            if (locked)
            {
                onDoorLocked?.Invoke();
                return;
            }
            
            if (_isOpen)
            {
                CloseDoor();
            }
            else
            {
                OpenDoor();
            }
        }
        
        public void TryOpenDoor()
        {
            if (locked || _isOpen) return;
            OpenDoor();
        }
        
        public void TryCloseDoor()
        {
            if (locked || !_isOpen) return;
            CloseDoor();
        }
        
        // Manual lock control
        public void UnlockDoor()
        {
            locked = false;
            onDoorUnlocked?.Invoke();
        }
        
        public void LockDoor()
        {
            locked = true;
            onDoorLocked?.Invoke();
        }
        
        public void SetLockedState(bool shouldLock)
        {
            locked = shouldLock;
            if (locked)
                onDoorLocked?.Invoke();
            else
                onDoorUnlocked?.Invoke();
        }
        
        private void OpenDoor()
        {
            if (!animator) return;
            
            animator.SetBool(openParameter, true);
            onDoorOpened?.Invoke();
        }
        
        private void CloseDoor()
        {
            if (!animator) return;
            
            animator.SetBool(openParameter, false);
            onDoorClosed?.Invoke();
        }
        
        
        public bool IsOpen => _isOpen;
        public bool IsLocked => locked;
        
        private void OnDrawGizmos()
        {
            if (locked)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(transform.position, Vector3.one * 0.5f);
            }
            else
            {
                Gizmos.color = _isOpen ? Color.green : Color.yellow;
                Gizmos.DrawWireCube(transform.position, Vector3.one * 0.5f);
            }
        }
    }
}