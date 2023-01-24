using System;
using System.Collections.Generic;
using _Project.Scripts.Gameplay.Data.Scriptable_Object_Templates;
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
		
		[SerializeField] private IntObject IntObject;
		[Space]
		[SerializeField] private string precedingText = string.Empty;
		[SerializeField] private bool shouldUpdate;

		TextMeshProUGUI textMeshPro;

		[Space]
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
			
			textMeshPro.SetText(precedingText + IntObject.value);
		}

		private void SetCorrespondingTextVisibility()
		{
			int currentValue = IntObject.value;
			
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
					
					default:
						if (currentValue == showCondition.value)
							ShowTextMesh();
						else
							HideTextMesh();
						break;
				}
			}
		}
		
		private void HideTextMesh()
		{
			textMeshPro.enabled = false;
		}
		
		private void ShowTextMesh()
		{
			textMeshPro.enabled = true;
		}

		void Update()
		{
			UpdateText();
		}
	}
}
