using UnityEngine;

public class UIGaugeViewer<GAUGE> : MonoBehaviour where GAUGE : EntityGauge
{
    [SerializeField] private Transform m_TargetEntity;
    [SerializeField] private RectTransform m_GaugeContainer;
    [SerializeField] private RectTransform m_GaugeView;

    private GAUGE m_Gauge;

    private void Awake()
    {
        m_Gauge = m_TargetEntity.GetComponent<GAUGE>();
    }

    private void OnEnable()
    {
        m_Gauge.OnValueChanged += OnValueChanged;
        m_Gauge.OnMaxReached += UpdateView;
        m_Gauge.OnMinReached += UpdateView;
        UpdateView();
    }

    private void OnDisable()
    {
        m_Gauge.OnValueChanged -= OnValueChanged;
        m_Gauge.OnMaxReached -= UpdateView;
        m_Gauge.OnMinReached -= UpdateView;
    }

    private void OnValueChanged(float _)
    {
        UpdateView();
    }

    private void UpdateView()
    {
        m_GaugeView.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_Gauge.GetCompletion() * m_GaugeContainer.rect.width);
    }
}