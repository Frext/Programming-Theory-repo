using UnityEngine;

namespace _Project.Scripts.Gameplay
{
	public static class HelperMethodsUtil
	{
		public static bool IsLayerInLayerMask(int layer, LayerMask layerMask)
		{
			// Returns true if the layer that is converted into a layer mask and the attack layer mask have a common bit which is 1.
			return (layerMask & (1 << layer)) != 0;
		}
	}
}
