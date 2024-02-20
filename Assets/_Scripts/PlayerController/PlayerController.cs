using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
	public enum InputType
	{
		Static = 0,
		StaticDynamic = 1,
		Dynamic = 2,
	}

	private InputType m_InputType = InputType.Static;
	private Vector2 m_MovementInputValue; // magnitude <= 1

	[SerializeField] private float m_InputRadius = 100;
	private bool m_IsPressed = false;

	// static
	[SerializeField] private Transform m_VirtualGamepad;
	private Vector2 m_GamepadPos;

	// static dynamic
	private bool m_IsInitialized = false;
	private Vector2 m_InitMousePos = Vector2.zero;

	// dynamic
	private Vector2 m_CurMousePos = Vector2.zero;

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

		m_GamepadPos = m_Camera.WorldToScreenPoint(m_VirtualGamepad.position);
	}

	void Update()
	{
		// update input value
		if(m_InputType == InputType.Dynamic)
			_SetDelta(m_Camera.WorldToScreenPoint(transform.position), m_CurMousePos, m_InputRadius * 0.5f);
	}

	public void SetInputType(int iInputType)
	{
		SetInputType((InputType)iInputType);
	}

	public void SetInputType(InputType iInputType)
	{
		m_InputType = iInputType;

		m_VirtualGamepad.gameObject.SetActive(m_InputType == InputType.Static);
		m_IsPressed = false;
		m_MovementInputValue = Vector2.zero;
	}

	private void OnTouchscreenPosition(InputAction.CallbackContext _)
	{
		if(!m_IsPressed)
			return;

		Vector2 curPos = m_PositionInput.ReadValue<Vector2>();

		switch(m_InputType)
		{
			case InputType.Static:
				_SetDelta(m_GamepadPos, curPos, m_InputRadius);
				break;
			case InputType.StaticDynamic:
				if(!m_IsInitialized)
				{
					m_IsInitialized = true;
					m_InitMousePos = curPos;
				}
				else
					// move init pos when maxed
					m_InitMousePos += _SetDelta(m_InitMousePos, curPos, m_InputRadius);
				break;
			case InputType.Dynamic:
				m_CurMousePos = curPos;
				break;
			default:
				Debug.LogError("Input type not implemented");
				break;
		}
	}

	private void OnTouchscreenTouch(InputAction.CallbackContext _)
	{
		m_IsPressed = m_TouchInput.ReadValue<float>() > 0.5f; // actuation point

		if(!m_IsPressed)
		{
			m_MovementInputValue = Vector2.zero;
			m_IsInitialized = false;
			return;
		}

		m_CurMousePos = m_PositionInput.ReadValue<Vector2>();
	}

	private Vector2 _SetDelta(Vector2 iInitPos, Vector2 iCurPos, float iMaxLength)
	{
		Vector2 delta = iCurPos - iInitPos;
		Vector2 normedDelta = delta / iMaxLength;

		float deltaSqrLength = normedDelta.sqrMagnitude;
		if(deltaSqrLength > 1)
		{
			m_MovementInputValue = normedDelta / Mathf.Sqrt(deltaSqrLength);
			return delta - m_MovementInputValue * iMaxLength;
		}

		m_MovementInputValue = normedDelta;
		return Vector2.zero;
	}

    public Vector2 GetMovementInputValue()
    {
        return m_MovementInputValue;
    }
}
