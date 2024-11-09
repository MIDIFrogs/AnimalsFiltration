// Copyright 2024 (c) MIDIFrogs (contact https://github.com/MIDIFrogs)
// Distributed under AGPL v3.0 license. See LICENSE.md file in the project root for more information
namespace AnimaFiltering.Services
{
    /// <summary>
    /// Represents configurable stats for the camera.
    /// </summary>
    public class CameraStats
    {
        /// <summary>
        /// Name of the camera.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Number of images processed for all the time.
        /// </summary>
        public int ProcessedImages { get; set; }

        /// <summary>
        /// Number of good images from the selected ones.
        /// </summary>
        public int GoodImages { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}