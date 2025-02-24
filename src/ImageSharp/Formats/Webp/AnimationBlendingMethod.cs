// Copyright (c) Six Labors.
// Licensed under the Six Labors Split License.

namespace SixLabors.ImageSharp.Formats.Webp;

/// <summary>
/// Indicates how transparent pixels of the current frame are to be blended with corresponding pixels of the previous canvas.
/// </summary>
internal enum AnimationBlendingMethod
{
    /// <summary>
    /// Use alpha blending. After disposing of the previous frame, render the current frame on the canvas using alpha-blending.
    /// If the current frame does not have an alpha channel, assume alpha value of 255, effectively replacing the rectangle.
    /// </summary>
    AlphaBlending = 0,

    /// <summary>
    /// Do not blend. After disposing of the previous frame,
    /// render the current frame on the canvas by overwriting the rectangle covered by the current frame.
    /// </summary>
    DoNotBlend = 1
}
