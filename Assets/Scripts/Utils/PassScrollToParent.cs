using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class PassScrollToParent : MonoBehaviour, IScrollHandler
{
  private ScrollRect parentScrollRect = null;
  private void Awake()
  {
    parentScrollRect = GetComponentInParent<ScrollRect>();
  }
  public void OnScroll(PointerEventData eventData)
  {
    if (parentScrollRect != null)
    {
      parentScrollRect.OnScroll(eventData);
    }
  }
}
