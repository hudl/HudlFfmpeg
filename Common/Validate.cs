using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Common
{
    /// <summary>
    /// helper class that helps with validation of objects in a ffmpeg project
    /// </summary>
    internal class Validate
    {
        /// <summary>
        /// returns a boolean indicating if <paramref name="TypeA"/> is applicable to <paramref name="TypeB"/> 
        /// </summary>
        /// <typeparam name="TypeA">the type in question to be applied to</typeparam>
        /// <typeparam name="TypeB">the type in question that is required</typeparam>
        public static bool AppliesTo<TypeA, TypeB>()
            where TypeB : IResource
        {
            var typeAAttributes = new List<CustomAttributeData>(typeof(TypeA).CustomAttributes); 
            var typeAAppliesToAttributes = typeAAttributes.FindAll(a => a.AttributeType == typeof(AppliesToResourceAttribute));
            if (typeAAppliesToAttributes == null || typeAAppliesToAttributes.Count == 0)
            {
                return false;
            }
            var typeAAppliesToAttributeType = typeAAppliesToAttributes.Find(a =>
                {
                    foreach (var namedArg in a.NamedArguments)
                    {
                        if (namedArg.MemberName == "Type" && namedArg.TypedValue.Value is TypeB)
                        {
                            return true;
                        }
                    }
                    return false; 
                }); 
            return (typeAAppliesToAttributes != null); 
        }
    }
}
