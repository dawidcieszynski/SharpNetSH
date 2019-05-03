﻿using System;

namespace SharpNetSH
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ParameterNameAttribute : Attribute
    {
        public ParameterNameAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }

        public ParameterNameAttribute(string parameterName, BooleanType booleanType)
        {
            ParameterName = parameterName;
            BooleanType = booleanType;
        }

		public String ParameterName { get; private set; }
		public BooleanType BooleanType { get; private set; }
    }
}