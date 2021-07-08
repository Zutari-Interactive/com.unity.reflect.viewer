using System;
using System.Collections.Generic;
using UnityEngine.Reflect.MeasureTool;

namespace Unity.Reflect.Viewer.UI
{
    [Serializable]
    public struct PointerToolStateData : IEquatable<PointerToolStateData>
    {
        public static readonly PointerToolStateData defaultData = new PointerToolStateData()
        {
            toolState = false,
            selectionType = AnchorType.Point,
            measureMode = MeasureMode.RawDistance,
            measureFormat = MeasureFormat.Meters,
            selectedAnchorsContext = null
        };

        public bool toolState;
        public AnchorType selectionType;
        public MeasureMode measureMode;
        public MeasureFormat measureFormat;
        public List<AnchorSelectionContext> selectedAnchorsContext;

        public bool Equals(PointerToolStateData other)
        {
            return toolState              == other.toolState     &&
                   selectionType          == other.selectionType &&
                   measureMode            == other.measureMode   &&
                   measureFormat          == other.measureFormat &&
                   selectedAnchorsContext == other.selectedAnchorsContext;
        }

        public override bool Equals(object obj)
        {
            return obj is PointerToolStateData other && Equals(other);
        }

        public static bool operator ==(PointerToolStateData a, PointerToolStateData b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(PointerToolStateData a, PointerToolStateData b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = toolState.GetHashCode();
                hashCode = (hashCode * 397) ^ selectionType.GetHashCode();
                hashCode = (hashCode * 397) ^ measureMode.GetHashCode();
                hashCode = (hashCode * 397) ^ measureFormat.GetHashCode();
                hashCode = (hashCode * 397) ^ selectedAnchorsContext.GetHashCode();

                return hashCode;
            }
        }
    }
}
