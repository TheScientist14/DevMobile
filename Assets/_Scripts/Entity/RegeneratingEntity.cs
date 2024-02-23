using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LivingEntity))]
public class RegeneratingEntity : MonoBehaviour
{
    [SerializeField] private float m_Value;
    [SerializeField] private float m_Rate;

    private bool m_IsActive = false;

    private GameStateMachine m_GameStateMachine;
    private LivingEntity m_LivingEntity;

    private void Awake()
    {
        m_GameStateMachine = GameManager.Get().GetStateMachine();
        m_LivingEntity = GetComponent<LivingEntity>();
    }

    private void OnEnable()
    {
        m_GameStateMachine.OnStateChange += OnStateChange;
    }

    private void OnDisable()
    {
        m_GameStateMachine.OnStateChange -= OnStateChange;
    }

    private void OnStateChange(IGameState iPrevious, IGameState iCurrent)
    {
        if (iCurrent is PlayGameState)
            Activate();
        else
            Deactivate();
    }

    public void Activate()
    {
        m_IsActive = true;
        StartCoroutine(HandleRegeneration());
    }

    public void Deactivate()
    {
        m_IsActive = false;
    }

    private IEnumerator HandleRegeneration()
    {
        while (m_IsActive)
        {
            if (!m_LivingEntity.IsMaximal())
                m_LivingEntity.Heal(m_Value);
            yield return new WaitForSeconds(1.0f / m_Rate);
        }
    }
}
