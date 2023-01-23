using _Project.Scripts.Gameplay.Data.Scriptable_Object_Templates;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class ShowIntObject : MonoBehaviour
	{
		[SerializeField] private IntObject IntObject;
		[Space]
		[SerializeField] private string precedingText = string.Empty;
		[SerializeField] private bool shouldUpdate;

		TextMeshProUGUI textMeshPro;

		void Awake()
		{
			textMeshPro = GetComponent<TextMeshProUGUI>();
		}

		void Start()
		{
			UpdateText();

			if (!shouldUpdate)
				enabled = false;
		}

		private void UpdateText()
		{
			textMeshPro.SetText(precedingText + IntObject.value);
		}

		void Update()
		{
			UpdateText();
		}
	}
}
