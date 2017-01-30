﻿using BenchmarkDotNet.Attributes;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public class LoadResizeSaveParallel
    {
        const int ThumbnailSize = 150;

        readonly IEnumerable<string> _images;
        readonly string _outputDirectory;

        public LoadResizeSaveParallel()
        {
            // Find the closest images directory
            var imageDirectory = Path.GetFullPath(".");
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
            _images = Directory.EnumerateFiles(imageDirectory).Take(20);
            // Create the output directory next to the images directory
            _outputDirectory = Path.Combine(Path.GetDirectoryName(imageDirectory), "output");
            if (!Directory.Exists(_outputDirectory))
            {
                Directory.CreateDirectory(_outputDirectory);
            }
        }

        [Benchmark(Baseline = true, Description = "System.Drawing Load, Resize, Save - Parallel")]
        public void SystemDrawingResizeBenchmark()
        {
            Parallel.ForEach(_images, image => {
                LoadResizeSave.SystemDrawingResize(image, ThumbnailSize, _outputDirectory);
            });
        }
    }
}