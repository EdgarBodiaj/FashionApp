using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.SimpleColorPicker.Scripts
{
	/// <summary>
	/// Main gradient representation (brightness/saturation).
	/// </summary>
	public class PaletteGradient : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        public Canvas Canvas;
		public ColorJoystick ColorJoystick;

		/// <summary>
		/// Called when user clicks area.
		/// </summary>
		public void OnPointerDown(PointerEventData eventData)
		{
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), eventData.position, Canvas.renderMode == RenderMode.ScreenSpaceCamera ? Camera.main : null, out var position))
			{
				return;
			}

			ColorJoystick.OnDrag(eventData);
		}

		/// <summary>
		/// Called when user drags color picker.
		/// </summary>
		public void OnDrag(PointerEventData eventData)
		{
			ColorJoystick.OnDrag(eventData);
		}
	}
}