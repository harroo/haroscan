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

using Auki.Barcode.Rendering;

namespace Auki.Barcode
{
   /// <summary>
   /// A smart class to encode some content to a svg barcode image
   /// </summary>
   public class BarcodeWriterSvg : BarcodeWriter<SvgRenderer.SvgImage>, IBarcodeWriterSvg
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="BarcodeWriterSvg"/> class.
      /// </summary>
      public BarcodeWriterSvg()
      {
         Renderer = new SvgRenderer();
      }
   }
}