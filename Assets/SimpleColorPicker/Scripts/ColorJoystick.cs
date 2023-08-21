using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.SimpleColorPicker.Scripts
{
	/// <summary>
	/// Color picker 'joystick' representation.
	/// </summary>
	public class ColorJoystick : MonoBehaviour, IDragHandler
    {
        public Canvas Canvas;
		public Image Center;
		public RectTransform RectTransform;
		public ColorPicker ColorPicker;

		/// <summary>
		/// Called when picker moved.
		/// </summary>
		public void OnDrag(PointerEventData eventData)
		{
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransform, eventData.position, Canvas.renderMode == RenderMode.ScreenSpaceCamera ? Camera.main : null, out var position))
			{
				return;
			}

            position.x = Mathf.Max(position.x, RectTransform.rect.min.x);
			position.y = Mathf.Max(position.y, RectTransform.rect.min.y);
			position.x = Mathf.Min(position.x, RectTransform.rect.max.x);
			position.y = Mathf.Min(position.y, RectTransform.rect.max.y);

			transform.localPosition = position;

			var texture = ColorPicker.Texture;
			var x = position.x / RectTransform.rect.width * texture.width;
			var y = position.y / RectTransform.rect.height * texture.height;
			var color = Color.HSVToRGB(ColorPicker.H.Value, x / texture.width,  y / texture.height);

            color.a = ColorPicker.A.Value;

			ColorPicker.SetColor(color, picker: false);
		}
	}
}