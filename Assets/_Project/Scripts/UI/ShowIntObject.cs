using System;
using System.Collections.Generic;
using _Project.Scripts.Gameplay.Data.Scene.Scriptable_Object_Templates;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class ShowIntObject : MonoBehaviour
	{
		[Serializable]
		public class IntShowCondition
		{
			public enum Conditions {
				GreaterThan,
				LessThan,
				EqualTo
			}
			public Conditions condition;
			
			public int value;
		}
		
		[SerializeField] private IntObject IntObjectSO;
		
		[Space]
		[SerializeField] private string precedingText = string.Empty;
		[SerializeField] private bool shouldUpdate;

		TextMeshProUGUI textMeshPro;
		
		
		[Header("Conditions")]
		[SerializeField] private List<IntShowCondition> showingConditions;
 
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
			SetCorrespondingTextVisibility();
			
			textMeshPro.SetText(precedingText + IntObjectSO.runtimeValue);
		}

		private void SetCorrespondingTextVisibility()
		{
			int currentValue = IntObjectSO.runtimeValue;
			
			foreach (IntShowCondition showCondition in showingConditions)
			{
				switch (showCondition.condition)
				{
					case IntShowCondition.Conditions.GreaterThan:
						if (currentValue > showCondition.value)
							ShowTextMesh();
						else
							HideTextMesh();
						break;
					case IntShowCondition.Conditions.LessThan:
						if (currentValue < showCondition.value)
							ShowTextMesh();
						else
							HideTextMesh();
						break;

					case IntShowCondition.Conditions.EqualTo:
					default:
						if (currentValue == showCondition.value)
							ShowTextMesh();
						else
							HideTextMesh();
						break;
				}
			}
		}
		
		private void ShowTextMesh()
		{
			textMeshPro.enabled = true;
		}
		
		private void HideTextMesh()
		{
			textMeshPro.enabled = false;
		}
		
		void Update()
		{
			UpdateText();
		}
	}
}
