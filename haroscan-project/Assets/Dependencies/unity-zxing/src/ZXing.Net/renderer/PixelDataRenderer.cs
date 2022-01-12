﻿/*
 * Copyright 2012 Auki.Barcode.Net authors
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */


using System;
using UnityEngine;
using Color = UnityEngine.Color32;

using Auki.Barcode.Common;
using Auki.Barcode.OneD;

namespace Auki.Barcode.Rendering
{
   /// <summary>
   /// Renders a <see cref="BitMatrix" /> to an byte array with pixel data (4 byte per pixel, BGRA)
   /// </summary>
   public sealed class PixelDataRenderer : IBarcodeRenderer<PixelData>
   {
      /// <summary>
      /// Gets or sets the foreground color.
      /// </summary>
      /// <value>
      /// The foreground color.
      /// </value>
      //[System.CLSCompliant(false)]
      public Color Foreground { get; set; }
      /// <summary>
      /// Gets or sets the background color.
      /// </summary>
      /// <value>
      /// The background color.
      /// </value>
      //[System.CLSCompliant(false)]
      public Color Background { get; set; }

      /// <summary>
      /// Initializes a new instance of the <see cref="PixelDataRenderer"/> class.
      /// </summary>
      public PixelDataRenderer()
      {
         Foreground = UnityEngine.Color.black;
         Background = UnityEngine.Color.white;
      }

      /// <summary>
      /// Renders the specified matrix.
      /// </summary>
      /// <param name="matrix">The matrix.</param>
      /// <param name="format">The format.</param>
      /// <param name="content">The content.</param>
      /// <returns></returns>
      public PixelData Render(BitMatrix matrix, BarcodeFormat format, string content)
      {
         return Render(matrix, format, content, null);
      }

      /// <summary>
      /// Renders the specified matrix.
      /// </summary>
      /// <param name="matrix">The matrix.</param>
      /// <param name="format">The format.</param>
      /// <param name="content">The content.</param>
      /// <param name="options">The options.</param>
      /// <returns></returns>
      public PixelData Render(BitMatrix matrix, BarcodeFormat format, string content, EncodingOptions options)
      {
         int width = matrix.Width;
         int heigth = matrix.Height;
         bool outputContent = (options == null || !options.PureBarcode) &&
                              !String.IsNullOrEmpty(content) && (format == BarcodeFormat.CODE_39 ||
                                                                 format == BarcodeFormat.CODE_128 ||
                                                                 format == BarcodeFormat.EAN_13 ||
                                                                 format == BarcodeFormat.EAN_8 ||
                                                                 format == BarcodeFormat.CODABAR ||
                                                                 format == BarcodeFormat.ITF ||
                                                                 format == BarcodeFormat.UPC_A ||
                                                                 format == BarcodeFormat.MSI ||
                                                                 format == BarcodeFormat.PLESSEY);
         int emptyArea = outputContent ? 16 : 0;
         int pixelsize = 1;

         if (options != null)
         {
            if (options.Width > width)
            {
               width = options.Width;
            }
            if (options.Height > heigth)
            {
               heigth = options.Height;
            }
            // calculating the scaling factor
            pixelsize = width / matrix.Width;
            if (pixelsize > heigth / matrix.Height)
            {
               pixelsize = heigth / matrix.Height;
            }
         }

         var pixels = new byte[width * heigth * 4];
         var index = 0;

         for (int y = 0; y < matrix.Height - emptyArea; y++)
         {
            for (var pixelsizeHeight = 0; pixelsizeHeight < pixelsize; pixelsizeHeight++)
            {
               for (var x = 0; x < matrix.Width; x++)
               {
                  var color = matrix[x, y] ? Foreground : Background;
                  for (var pixelsizeWidth = 0; pixelsizeWidth < pixelsize; pixelsizeWidth++)
                  {
                     pixels[index++] = color.b;
                     pixels[index++] = color.g;
                     pixels[index++] = color.r;
                     pixels[index++] = color.a;
                  }
               }
               for (var x = pixelsize * matrix.Width; x < width; x++)
               {
                  pixels[index++] = Background.b;
                  pixels[index++] = Background.g;
                  pixels[index++] = Background.r;
                  pixels[index++] = Background.a;
               }
            }
         }
         for (int y = matrix.Height * pixelsize - emptyArea; y < heigth; y++)
         {
            for (var x = 0; x < width; x++)
            {
               pixels[index++] = Background.b;
               pixels[index++] = Background.g;
               pixels[index++] = Background.r;
               pixels[index++] = Background.a;
            }
         }

         return new PixelData(width, heigth, pixels);
      }
   }
}
