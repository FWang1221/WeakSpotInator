// Automatically Generated

using System.Diagnostics.CodeAnalysis;
using HKLib.hk2018;

namespace HKLib.Reflection.hk2018;

internal class hkbTransformVectorModifierData : HavokData<hkbTransformVectorModifier> 
{
    public hkbTransformVectorModifierData(HavokType type, hkbTransformVectorModifier instance) : base(type, instance) {}

    public override bool TryGetField<TGet>(string fieldName, [MaybeNull] out TGet value)
    {
        value = default;
        switch (fieldName)
        {
            case "m_propertyBag":
            case "propertyBag":
            {
                if (instance.m_propertyBag is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_variableBindingSet":
            case "variableBindingSet":
            {
                if (instance.m_variableBindingSet is null)
                {
                    return true;
                }
                if (instance.m_variableBindingSet is TGet castValue)
                {
                    value = castValue;
                    return true;
                }
                return false;
            }
            case "m_userData":
            case "userData":
            {
                if (instance.m_userData is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_name":
            case "name":
            {
                if (instance.m_name is null)
                {
                    return true;
                }
                if (instance.m_name is TGet castValue)
                {
                    value = castValue;
                    return true;
                }
                return false;
            }
            case "m_enable":
            case "enable":
            {
                if (instance.m_enable is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_rotation":
            case "rotation":
            {
                if (instance.m_rotation is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_translation":
            case "translation":
            {
                if (instance.m_translation is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_vectorIn":
            case "vectorIn":
            {
                if (instance.m_vectorIn is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_vectorOut":
            case "vectorOut":
            {
                if (instance.m_vectorOut is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_rotateOnly":
            case "rotateOnly":
            {
                if (instance.m_rotateOnly is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_inverse":
            case "inverse":
            {
                if (instance.m_inverse is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_computeOnActivate":
            case "computeOnActivate":
            {
                if (instance.m_computeOnActivate is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_computeOnModify":
            case "computeOnModify":
            {
                if (instance.m_computeOnModify is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            default:
            return false;
        }
    }

    public override bool TrySetField<TSet>(string fieldName, TSet value)
    {
        switch (fieldName)
        {
            case "m_propertyBag":
            case "propertyBag":
            {
                if (value is not hkPropertyBag castValue) return false;
                instance.m_propertyBag = castValue;
                return true;
            }
            case "m_variableBindingSet":
            case "variableBindingSet":
            {
                if (value is null)
                {
                    instance.m_variableBindingSet = default;
                    return true;
                }
                if (value is hkbVariableBindingSet castValue)
                {
                    instance.m_variableBindingSet = castValue;
                    return true;
                }
                return false;
            }
            case "m_userData":
            case "userData":
            {
                if (value is not ulong castValue) return false;
                instance.m_userData = castValue;
                return true;
            }
            case "m_name":
            case "name":
            {
                if (value is null)
                {
                    instance.m_name = default;
                    return true;
                }
                if (value is string castValue)
                {
                    instance.m_name = castValue;
                    return true;
                }
                return false;
            }
            case "m_enable":
            case "enable":
            {
                if (value is not bool castValue) return false;
                instance.m_enable = castValue;
                return true;
            }
            case "m_rotation":
            case "rotation":
            {
                if (value is not Quaternion castValue) return false;
                instance.m_rotation = castValue;
                return true;
            }
            case "m_translation":
            case "translation":
            {
                if (value is not Vector4 castValue) return false;
                instance.m_translation = castValue;
                return true;
            }
            case "m_vectorIn":
            case "vectorIn":
            {
                if (value is not Vector4 castValue) return false;
                instance.m_vectorIn = castValue;
                return true;
            }
            case "m_vectorOut":
            case "vectorOut":
            {
                if (value is not Vector4 castValue) return false;
                instance.m_vectorOut = castValue;
                return true;
            }
            case "m_rotateOnly":
            case "rotateOnly":
            {
                if (value is not bool castValue) return false;
                instance.m_rotateOnly = castValue;
                return true;
            }
            case "m_inverse":
            case "inverse":
            {
                if (value is not bool castValue) return false;
                instance.m_inverse = castValue;
                return true;
            }
            case "m_computeOnActivate":
            case "computeOnActivate":
            {
                if (value is not bool castValue) return false;
                instance.m_computeOnActivate = castValue;
                return true;
            }
            case "m_computeOnModify":
            case "computeOnModify":
            {
                if (value is not bool castValue) return false;
                instance.m_computeOnModify = castValue;
                return true;
            }
            default:
            return false;
        }
    }

}
