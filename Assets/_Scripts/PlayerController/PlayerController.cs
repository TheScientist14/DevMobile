using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
	public enum InputType
	{
		Static,
		StaticDynamic,
		Dynamic,
	}

    [SerializeField] private InputType m_InputType = InputType.Static;
	private Vector2 m_MovementInputValue; // magnitude <= 1

	// DEBUG
	[SerializeField] TextMeshProUGUI m_XDisplay;
	[SerializeField] TextMeshProUGUI m_YDisplay;
	private Vector3 m_InitPosition;

	private bool m_IsPressed = false;

	// static
	[SerializeField] private GameObject m_VirtualGamepad;
	private Vector2 m_GamepadPos = Vector2.up * 200 + Vector2.right * 400;

	// static dynamic
	private bool m_IsInitialized = false;
	private Vector2 m_InitPos = Vector2.zero;

	[SerializeField] private float m_InputRadius = 100;

	private Camera m_Camera;
	private PlayerInput m_PlayerInput;
	private InputAction m_PositionInput;
	private InputAction m_TouchInput;

	private void Awake()
	{
		m_PlayerInput = GetComponent<PlayerInput>();
		m_PositionInput = m_PlayerInput.currentActionMap.FindAction("TouchscreenPosition");
		m_PositionInput.performed += OnTouchscreenPosition;

		m_TouchInput = m_PlayerInput.currentActionMap.FindAction("TouchscreenTouch");
		m_TouchInput.started += OnTouchscreenTouch;
		m_TouchInput.canceled += OnTouchscreenTouch;

		m_Camera = Camera.main;
	}

	// Start is called before the first frame update
	void Start()
	{
		SetInputType(InputType.StaticDynamic);

		m_InitPosition = transform.position;
	}

	void Update()
	{
		// update player with input value
		//transform.position = m_InitPosition + (Vector3)m_MovementInputValue;

		m_XDisplay.text = m_MovementInputValue.x.ToString();
		m_YDisplay.text = m_MovementInputValue.y.ToString();
	}

	public void SetInputType(InputType iInputType)
	{
		m_InputType = iInputType;

		m_VirtualGamepad.SetActive(m_InputType == InputType.Static);
		m_IsPressed = false;
		m_MovementInputValue = Vector2.zero;
	}

	private void OnTouchscreenPosition(InputAction.CallbackContext _)
	{
		if(!m_IsPressed)
			return;

		Debug.Log("position");
		Vector2 curPos = m_PositionInput.ReadValue<Vector2>();
		if(m_InputType == InputType.Static)
			_SetDelta(m_GamepadPos, curPos, m_InputRadius);
		else if(m_InputType == InputType.StaticDynamic)
		{
			if(!m_IsInitialized)
			{
				m_IsInitialized = true;
				m_InitPos = curPos;
			}
			else
				_SetDelta(m_InitPos, curPos, m_InputRadius);
		}
		else
		{
			// WRONG
			// _SetDelta(m_Camera.WorldToScreenPoint(transform.position), curPos, m_InputRadius);
		}
	}

	private void OnTouchscreenTouch(InputAction.CallbackContext _)
	{
		m_IsPressed = m_TouchInput.ReadValue<float>() > 0.5f; // actuation point

		if(!m_IsPressed)
		{
			Debug.Log("released");
			m_MovementInputValue = Vector2.zero;
			m_IsInitialized = false;
			return;
		}

		Debug.Log("touched");
	}

	private void _SetDelta(Vector2 iInitPos, Vector2 iCurPos, float iMaxLength)
	{
		Vector2 delta = iCurPos - iInitPos;
		delta /= iMaxLength;

		if(delta.sqrMagnitude > 1)
			m_MovementInputValue = delta.normalized;
		else
			m_MovementInputValue = delta;
	}

    public Vector2 GetMovementInputValue()
    {
        return m_MovementInputValue;
    }
}
