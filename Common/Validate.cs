using System.Collections.Generic;
using System.Reflection;
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
        /// returns a boolean indicating if <cref name="TObject"/> is applicable to <cref name="TRestrictedTo"/> 
        /// </summary>
        /// <typeparam name="TObject">the type in question to be applied to</typeparam>
        /// <typeparam name="TRestrictedTo">the type in question that is required</typeparam>
        public static bool AppliesTo<TObject, TRestrictedTo>()
            where TRestrictedTo : IResource
        {
            var typeAAttributes = new List<CustomAttributeData>(typeof(TObject).CustomAttributes); 
            var typeAAppliesToAttributes = typeAAttributes.FindAll(a => a.AttributeType == typeof(AppliesToResourceAttribute));
            if (typeAAppliesToAttributes.Count == 0)
            {
                return false;
            }
            var typeAAppliesToAttributeType = typeAAppliesToAttributes.Find(a =>
                {
                    foreach (var namedArg in a.NamedArguments)
                    {
                        if (namedArg.MemberName == "Type" && namedArg.TypedValue.Value is TRestrictedTo)
                        {
                            return true;
                        }
                    }
                    return false; 
                });
            return (typeAAppliesToAttributeType != null); 
        }
    }
}
