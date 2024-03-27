﻿using System;
using UnityEngine;

namespace Beamable.Editor.UI.Buss
{
	[AttributeUsage(AttributeTargets.Field)]
	public class SerializableValueImplementsAttribute : PropertyAttribute
	{
		public readonly Type baseType;

		public SerializableValueImplementsAttribute(Type baseType)
		{
			this.baseType = baseType;
		}
	}
}
