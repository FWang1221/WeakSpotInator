// Automatically Generated

using System.Diagnostics.CodeAnalysis;
using HKLib.hk2018;

namespace HKLib.Reflection.hk2018;

internal class hknpCompoundShapeSimdTreeData : HavokData<hknpCompoundShapeSimdTree> 
{
    public hknpCompoundShapeSimdTreeData(HavokType type, hknpCompoundShapeSimdTree instance) : base(type, instance) {}

    public override bool TryGetField<TGet>(string fieldName, [MaybeNull] out TGet value)
    {
        value = default;
        switch (fieldName)
        {
            case "m_nodes":
            case "nodes":
            {
                if (instance.m_nodes is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_isCompact":
            case "isCompact":
            {
                if (instance.m_isCompact is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_points":
            case "points":
            {
                if (instance.m_points is not TGet castValue) return false;
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
            case "m_nodes":
            case "nodes":
            {
                if (value is not List<hkcdSimdTree.Node> castValue) return false;
                instance.m_nodes = castValue;
                return true;
            }
            case "m_isCompact":
            case "isCompact":
            {
                if (value is not bool castValue) return false;
                instance.m_isCompact = castValue;
                return true;
            }
            case "m_points":
            case "points":
            {
                if (value is not List<Vector4> castValue) return false;
                instance.m_points = castValue;
                return true;
            }
            default:
            return false;
        }
    }

}
