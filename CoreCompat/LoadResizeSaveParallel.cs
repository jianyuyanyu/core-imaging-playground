﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace ImageProcessing
{
    public class LoadResizeSaveParallel
    {
        private const int ThumbnailSize = 150;
        private readonly IEnumerable<string> images;
        private readonly string outputDirectory;

        public LoadResizeSaveParallel()
        {
            // Find the closest images directory
            string imageDirectory = Path.GetFullPath(".");
            while (!Directory.Exists(Path.Combine(imageDirectory, "images")))
            {
                imageDirectory = Path.GetDirectoryName(imageDirectory);
                if (imageDirectory == null)
                {
                    throw new FileNotFoundException("Could not find an image directory.");
                }
            }

            imageDirectory = Path.Combine(imageDirectory, "images");

            // Get at most 20 images from there
            this.images = Directory.EnumerateFiles(imageDirectory).Take(20);

            // Create the output directory next to the images directory
            this.outputDirectory = Path.Combine(Path.GetDirectoryName(imageDirectory), "output");
            if (!Directory.Exists(this.outputDirectory))
            {
                Directory.CreateDirectory(this.outputDirectory);
            }
        }

        [Benchmark(Baseline = true, Description = "System.Drawing Load, Resize, Save - Parallel")]
        public void SystemDrawingResizeBenchmark() => Parallel.ForEach(this.images, image => LoadResizeSave.SystemDrawingResize(image, ThumbnailSize, this.outputDirectory));
    }
}