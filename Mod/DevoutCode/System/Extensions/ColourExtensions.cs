using Godot;

namespace Devout.DevoutCode.System.Extensions;

public static class ColourExtensions {
    public static Color Alpha(this Color opaque, double alpha) {
        return new Color(opaque.R, opaque.G, opaque.B, (float) alpha);
    }
}