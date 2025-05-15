using System;
using UnityEngine;

[ExecuteAlways]
public class SetAmbientColor : MonoBehaviour
{
   [ColorUsage(true, true)]
   public Color AmbientColor = Color.white;
   private void OnEnable()
   {
      RenderSettings.ambientLight = AmbientColor;
   }
}
