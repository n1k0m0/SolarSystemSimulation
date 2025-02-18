/*
   Copyright 2025 Nils Kopal

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace SolarSystemSimulation
{
    /// <summary>
    /// Main entry point of the application
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main entry point of the application
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // Create a window
            using (SolarSystemWindow window = new SolarSystemWindow(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, GraphicsMode.Default, "Solar System Simulation"))
            {
                window.WindowState = WindowState.Fullscreen;
                window.CursorVisible = false;

                // Set the size of the rendering area
                GL.Viewport(0, 0, window.Width, window.Height);

                // Enable depth testing
                GL.Enable(EnableCap.DepthTest);

                // Run the rendering loop
                window.Run(60, 60);
            }
        }
    }    
}