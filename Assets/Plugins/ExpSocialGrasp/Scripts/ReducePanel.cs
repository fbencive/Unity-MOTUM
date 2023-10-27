using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReducePanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Reduce()
    {
        RectTransform m_RectTransform = GetComponent<RectTransform>();
        m_RectTransform.anchoredPosition = new Vector2(m_RectTransform.anchoredPosition.x, 600);
    }
}
