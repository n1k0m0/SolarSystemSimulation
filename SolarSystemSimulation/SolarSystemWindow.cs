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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace SolarSystemSimulation
{
    /// <summary>
    /// A window that displays a solar system simulation
    /// </summary>
    public class SolarSystemWindow : GameWindow
    {
        private readonly List<AstronomicalBody> _astronomicalBodies = new List<AstronomicalBody>();
        private readonly Dictionary<string, int> _textures = new Dictionary<string, int>();
        private int _backgroundTextureId = 0;
        private double _scale = 0.5 * 1e-6;
        private double _simulatedSeconds = 0;
        private float _zoom = -2000000;
        private float _speed = 1;

        private bool _showData = true;
        private bool _showBackground = true;

        /// <summary>
        /// Creates a new solar system window with the specified width, height, mode, and title
        /// Also creates the solar system with the sun and the planets
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="mode"></param>
        /// <param name="title"></param>
        public SolarSystemWindow(int width, int height, GraphicsMode mode, string title) : base(width, height, mode, title)
        {
            CreateSolarSystem();
        }

        /// <summary>
        /// Creates the solar system with the sun and the planets
        /// and initializes the simulation parameters
        /// </summary>
        private void CreateSolarSystem()
        {
            _astronomicalBodies.Clear();
            _scale = 0.5 * 1e-6;
            _simulatedSeconds = 0;
            _speed = 1;
            _zoom = -2000000;
            _showData = true;
            _showBackground = true;

            // Sun
            _astronomicalBodies.Add(new AstronomicalBody(
                "Sun",
                1.989e30,    // Mass in kg
                6.9634e8,    // Radius in m
                0,           // X position (Sun at center)
                0,           // Y position
                0,           // Z position
                0,           // X velocity
                0,           // Y velocity
                0,           // Z velocity
                2192832      // Rotation period in s
            ));

            // Mercury
            _astronomicalBodies.Add(new AstronomicalBody(
                "Mercury",
                3.3011e23,       // Mass in kg
                2.4397e6,        // Radius in m
                5.79e10,         // X position in m
                0,               // Y position in m
                0,               // Z position in m
                0,               // X velocity in m/s
                4.787e4,         // Y velocity in m/s
                0,               // Z velocity in m/s
                5067014.4        // Rotation period in s
            ));

            // Venus
            _astronomicalBodies.Add(new AstronomicalBody(
                "Venus",
                4.8675e24,       // Mass in kg
                6.0518e6,        // Radius in m
                1.082e11,        // X position in m
                0,               // Y position in m
                0,               // Z position in m
                0,               // X velocity in m/s
                3.502e4,         // Y velocity in m/s
                0,               // Z velocity in m/s
                -20997360        // Rotation period in s
            ));

            // Earth
            _astronomicalBodies.Add(new AstronomicalBody(
                "Earth",
                5.9724e24,       // Mass in kg
                6.371e6,         // Radius in m
                1.496e11,        // X position in m
                0,               // Y position in m
                0,               // Z position in m
                0,               // X velocity in m/s
                2.978e4,         // Y velocity in m/s
                0,               // Z velocity in m/s
                86140.8          // Rotation period in s
            ));

            // Moon
            /*_astronomicalBodies.Add(new AstronomicalBody(
                "Moon",
                7.342e22,        // Mass in kg
                1.7374e6,        // Radius in m
                1.496e11 + 3.844e8, // X position: Earth + Moon distance (approx. 384,400 km)
                0,               // Y position in m
                0,               // Z position in m
                0,               // X velocity in m/s
                2.978e4 + 1.022e3, // Y velocity: Earth + Moon (1.022 km/s)
                0,               // Z velocity in m/s
                27.32            // Rotation period in days (synchronous with orbit)
            ));*/

            // Mars
            _astronomicalBodies.Add(new AstronomicalBody(
                "Mars",
                6.4171e23,       // Mass in kg
                3.3895e6,        // Radius in m
                2.279e11,        // X position in m
                0,               // Y position in m
                0,               // Z position in m
                0,               // X velocity in m/s
                2.407e4,         // Y velocity in m/s
                0,               // Z velocity in m/s
                88646.4           // Rotation period in s
            ));

            // Jupiter
            _astronomicalBodies.Add(new AstronomicalBody(
                "Jupiter",
                1.8982e27,       // Mass in kg
                6.9911e7,        // Radius in m
                7.785e11,        // X position in m
                0,               // Y position in m
                0,               // Z position in m
                0,               // X velocity in m/s
                1.307e4,         // Y velocity in m/s
                0,               // Z velocity in m/s
                35683.2          // Rotation period in s
            ));

            // Saturn
            _astronomicalBodies.Add(new AstronomicalBody(
                "Saturn",
                5.6834e26,       // Mass in kg
                5.8232e7,        // Radius in m
                1.433e12,        // X position in m
                0,               // Y position in m
                0,               // Z position in m
                0,               // X velocity in m/s
                9.69e3,          // Y velocity in m/s
                0,               // Z velocity in m/s
                38361.6          // Rotation period in s
            ));

            // Uranus
            _astronomicalBodies.Add(new AstronomicalBody(
                "Uranus",
                8.6810e25,       // Mass in kg
                2.5362e7,        // Radius in m
                2.871e12,        // X position in m
                0,               // Y position in m
                0,               // Z position in m
                0,               // X velocity in m/s
                6.81e3,          // Y velocity in m/s
                0,               // Z velocity in m/s
                -62035.2         // Rotation period in s
            ));

            // Neptune
            _astronomicalBodies.Add(new AstronomicalBody(
                "Neptune",
                1.0241e26,       // Mass in kg
                2.4622e7,        // Radius in m
                4.495e12,        // X position in m
                0,               // Y position in m
                0,               // Z position in m
                0,               // X velocity in m/s
                5.43e3,          // Y velocity in m/s
                0,               // Z velocity in m/s
                57974.4          // Rotation period in s
            ));

            // Pluto
            _astronomicalBodies.Add(new AstronomicalBody(
                "Pluto",
                1.303e22,       // Mass in kg
                1.1883e6,       // Radius in m
                5.906e12,       // X position in m
                0,              // Y position in m
                0,              // Z position in m
                0,              // X velocity in m/s
                4.74e3,         // Y velocity in m/s
                0,              // Z velocity in m/s
                -551836.8       // Rotation period in s
            ));
            LoadTextures();
        }

        /// <summary>
        /// Loads the texture located in the textures subfolder by loading each image and converting it to a bitmap
        /// </summary>
        private void LoadTextures()
        {
            //Load the textures
            foreach (AstronomicalBody astronomicalBody in _astronomicalBodies)
            {
                int textureId = LoadTexture(astronomicalBody.Name);
                //Set the texture id for the body
                astronomicalBody.TextureId = textureId;
            }

            //Load the background texture
            _backgroundTextureId = LoadTexture("background");
        }

        /// <summary>
        /// Loads a texture from the textures subfolder
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private int LoadTexture(string name)
        {
            if (_textures.ContainsKey(name))
            {
                return _textures[name];
            }

            int textureId = GL.GenTexture();

            //check if file exists, if not paint red texture
            if (!System.IO.File.Exists(@"textures\" + name + ".png"))
            {
                GL.BindTexture(TextureTarget.Texture2D, textureId);
                //paint red texture
                Bitmap bmp = new Bitmap(1, 1);
                bmp.SetPixel(0, 0, Color.Red);
                BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                bmp.UnlockBits(data);
                bmp.Dispose();
            }
            else
            {
                //Load the texture
                Bitmap bmp = new Bitmap(@"textures\" + name + ".png");
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                //Generate the texture
                textureId = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, textureId);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                bmp.UnlockBits(data);
                bmp.Dispose();
            }

            //Set the texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear); //Linear filtering when the texture is near the view
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear); //Linear filtering when the texture is far from the view
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat); //Repeat the texture in the S direction
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat); //Repeat the texture in the T direction

            //Add the texture to the list of textures
            _textures.Add(name, textureId);

            return textureId;
        }

        /// <summary>
        /// Handles the key down event to zoom in and out using the up and down arrow keys
        /// Also handles the scale of the solar system using the left and right arrow keys
        /// Also handles the speed of the simulation using page up and page down
        /// Also handles the escape key to exit the application
        /// Also handles F1 key to reset the simulation
        /// Also handles F2 key to show/hide the data
        /// Also handles F3 key to show/hide the background
        /// </summary>
        /// <param name="keyboardEventArgs"></param>
        protected override void OnKeyDown(KeyboardKeyEventArgs keyboardEventArgs)
        {
            if (keyboardEventArgs.Key == Key.Up)
            {
                _zoom += 10000;
            }
            else if (keyboardEventArgs.Key == Key.Down)
            {
                _zoom -= 10000;
            }
            else if (keyboardEventArgs.Key == Key.Left)
            {
                _scale += 0.1 * 1e-7;
            }
            else if (keyboardEventArgs.Key == Key.Right)
            {
                _scale -= 0.1 * 1e-7;
            }
            else if (keyboardEventArgs.Key == Key.PageUp)
            {
                if (_speed < 0.01f)
                {
                    _speed = (float)Math.Round(_speed + 0.001f, 3);
                }
                else if (_speed < 0.1f)
                {
                    _speed = (float)Math.Round(_speed + 0.01f, 2);
                }
                else if (_speed < 1.0f)
                {
                    _speed = (float)Math.Round(_speed + 0.1f, 1);
                }
                else if (_speed < 10.0f)
                {
                    _speed = (float)Math.Round(_speed + 1.0f, 0);
                }
                else
                {
                    _speed = (float)Math.Round(_speed + 10.0f, 0);
                }
            }
            else if (keyboardEventArgs.Key == Key.PageDown)
            {
                if (_speed > 10.0f)
                {
                    _speed = (float)Math.Round(_speed - 10.0f, 0);
                }
                else if (_speed > 1.0f)
                {
                    _speed = (float)Math.Round(_speed - 1.0f, 0);
                }
                else if (_speed > 0.1f)
                {
                    _speed = (float)Math.Round(_speed - 0.1f, 1);
                }
                else if (_speed > 0.01f)
                {
                    _speed = (float)Math.Round(_speed - 0.01f, 2);
                }
                else if (_speed > 0.001f)
                {
                    _speed = (float)Math.Round(_speed - 0.001f, 3);
                }
            }
            else if (keyboardEventArgs.Key == Key.Escape)
            {
                Exit();
            }
            else if (keyboardEventArgs.Key == Key.F1)
            {
                CreateSolarSystem();
            }
            else if (keyboardEventArgs.Key == Key.F2)
            {
                _showData = !_showData;
            }
            else if (keyboardEventArgs.Key == Key.F3)
            {
                _showBackground = !_showBackground;
            }
        }

        /// <summary>
        /// Called every frame to render the solar system
        /// </summary>
        /// <param name="frameEventArgs"></param>
        protected override void OnRenderFrame(FrameEventArgs frameEventArgs)
        {
            double timeInMilliseconds = frameEventArgs.Time * 1000;

            for (int i = 0; i < 1000; i++)
            {

                //Update each planet's velocity
                foreach (AstronomicalBody planet in _astronomicalBodies)
                {
                    planet.UpdateVelocity(_astronomicalBodies, timeInMilliseconds * _speed);
                }

                //Update each planet's position
                foreach (AstronomicalBody planet in _astronomicalBodies)
                {
                    planet.UpdatePosition(timeInMilliseconds * _speed);
                }

                //Update each planet's rotation
                foreach (AstronomicalBody planet in _astronomicalBodies)
                {
                    planet.UpdateRotation(timeInMilliseconds * _speed);
                }
            }

            // Render the solar system
            RenderSolarSystem(timeInMilliseconds);
        }

        /// <summary>
        /// Renders the specified text at the specified position with the specified width and height
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="textColor"></param>
        /// <param name="backgroundColor"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void RenderText(string text, Font font, Color textColor, Color backgroundColor, float x, float y, float width, float height)
        {
            Bitmap bmp = new Bitmap((int)width, (int)height);
            Graphics gfx = Graphics.FromImage(bmp);
            gfx.Clear(backgroundColor);
            gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            gfx.DrawString(text, font, new SolidBrush(textColor), new PointF(5, 5));

            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            int textureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp.Width, bmp.Height,
                0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bmp.UnlockBits(data);
            bmp.Dispose();
            gfx.Dispose();

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);

            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Ortho(0, Width, Height, 0, -1, 1);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadIdentity();

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.Color3(1.0f, 1.0f, 1.0f);

            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0); GL.Vertex2(x, y);
            GL.TexCoord2(1, 0); GL.Vertex2(x + width, y);
            GL.TexCoord2(1, 1); GL.Vertex2(x + width, y + height);
            GL.TexCoord2(0, 1); GL.Vertex2(x, y + height);
            GL.End();

            GL.Disable(EnableCap.Texture2D);

            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Modelview);

            GL.DeleteTexture(textureId);
        }

        /// <summary>
        /// Renders the solar system
        /// </summary>
        /// <param name="time"></param>

        private void RenderSolarSystem(double time)
        {
            //Compute the frames per second
            double fps = Math.Round(1000 / time, 0);

            // increase the speed of the simulation
            time *= _speed;

            // Clear the color and depth buffers
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Set the background color
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            // Draw the background image
            if (_showBackground)
            {
                DrawBackground();
            }

            // Set the projection matrix
            Matrix4 projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)(Math.PI / 4), 1920.0f / 1080.0f, 1.0f, int.MaxValue);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projectionMatrix);

            //view from above
            Matrix4 modelviewMatrix = Matrix4.LookAt(new Vector3(0, 0, 1) * _zoom, Vector3.Zero, Vector3.UnitX);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelviewMatrix);

            // Set the lighting parameters
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Light(LightName.Light0, LightParameter.Position, new float[] { 0.0f, 0.0f, 0.0f, 1.0f });
            GL.Light(LightName.Light0, LightParameter.Ambient, new float[] { 1.5f, 1.5f, 1.5f, 1.0f });
            GL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { 0.9f, 0.9f, 0.9f, 1.0f });
            GL.Light(LightName.Light0, LightParameter.Specular, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });

            _simulatedSeconds += time * 10000;

            if (_showData)
            {
                ShowData(fps);
            }

            // Draw each body as a sphere
            foreach (AstronomicalBody body in _astronomicalBodies)
            {
                body.DrawTexturedBody(_scale);
            }

            SwapBuffers();

        }

        /// <summary>
        /// Shows the data of all astronomical bodies on the upper left side
        /// </summary>
        /// <param name="fps"></param>
        private void ShowData(double fps)
        {
            // Compute the elapsed time in years, days, hours, and minutes
            const int secondsPerMinute = 60;
            const int minutesPerHour = 60;
            const int hoursPerDay = 24;
            const int daysPerYear = 365;
            long secondsPerDay = secondsPerMinute * minutesPerHour * hoursPerDay;
            long secondsPerYear = secondsPerDay * daysPerYear;

            long years = (long)_simulatedSeconds / secondsPerYear;
            long remainingSeconds = (long)_simulatedSeconds % secondsPerYear;
            long days = remainingSeconds / secondsPerDay;
            remainingSeconds %= secondsPerDay;
            long hours = remainingSeconds / (minutesPerHour * secondsPerMinute);
            remainingSeconds %= (minutesPerHour * secondsPerMinute);
            long minutes = remainingSeconds / secondsPerMinute;

            //Compute distance from sun to each planet
            StringBuilder distanceStringBuilder = new StringBuilder();
            foreach (AstronomicalBody body in _astronomicalBodies)
            {
                if (body.Name.Equals("Sun", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                //compute distance earth sun
                double distance = Math.Round(Math.Sqrt(Math.Pow(_astronomicalBodies[0].X - body.X, 2) + Math.Pow(_astronomicalBodies[0].Y - body.Y, 2) + Math.Pow(_astronomicalBodies[0].Z - body.Z, 2)), 0);
                //compute AU
                double AU = Math.Round(distance / 149597870700, 3);
                distanceStringBuilder.AppendLine("Distance Sun -> " + body.Name + ": " + (distance / 1000).ToString("N0") + " km (" + AU.ToString("N3") + " AU)");
            }

            //Compute current speed of each planet
            StringBuilder speedStringBuilder = new StringBuilder();
            foreach (AstronomicalBody body in _astronomicalBodies)
            {
                if (body.Name.Equals("Sun", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                //compute speed
                double speed = Math.Round((Math.Sqrt(Math.Pow(body.Vx, 2) + Math.Pow(body.Vy, 2) + Math.Pow(body.Vz, 2))), 0);
                speedStringBuilder.AppendLine("Speed " + body.Name + ": " + speed.ToString("N0") + " m/s");
            }

            //Compute current position of each planet
            StringBuilder positionStringBuilder = new StringBuilder();
            foreach (AstronomicalBody body in _astronomicalBodies)
            {
                if (body.Name.Equals("Sun", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                //compute position in AU
                double x = Math.Round(body.X / 149597870700, 3);
                double y = Math.Round(body.Y / 149597870700, 3);
                //double z = Math.Round(body.Z / 149597870700, 3);
                positionStringBuilder.AppendLine("Position " + body.Name + ": X=" + x.ToString("N3") + " AU, Y=" + y.ToString("N3") + " AU"/*, Z=" + z.ToString("N3") + " AU"*/);
            }

            //Render text
            Font textFont = new Font("Arial", 10);
            Color textColor = Color.White;
            Color backgroundColor = Color.Black;

            string text = string.Format("FPS: {0}\n\nSimulation time: {1} years, {2} days, {3} hours, {4} minutes\nSimulation speed: {5}\n\n{6}\n{7}\n{8}", fps, years, days, hours, minutes, _speed.ToString("0.###"), distanceStringBuilder.ToString(), speedStringBuilder.ToString(), positionStringBuilder.ToString());
            RenderText(text, textFont, textColor, backgroundColor, 0, 0, 450, 750);
        }

        /// <summary>
        /// Draws the background image, e.g. an image of stars
        /// </summary>
        private void DrawBackground()
        {
            if (_backgroundTextureId == 0)
            {
                Console.WriteLine("Background texture is not loaded!");
                return;
            }

            GL.Disable(EnableCap.DepthTest);

            // Set Orthographic Projection for 2D background rendering
            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Ortho(-500, 500, -250, 250, -1, 1);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadIdentity();

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, _backgroundTextureId);

            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 1); GL.Vertex3(-500, -250, -0.9);  // Bottom-left
            GL.TexCoord2(1, 1); GL.Vertex3(500, -250, -0.9);   // Bottom-right
            GL.TexCoord2(1, 0); GL.Vertex3(500, 250, -0.9);    // Top-right
            GL.TexCoord2(0, 0); GL.Vertex3(-500, 250, -0.9);   // Top-left
            GL.End();

            GL.Disable(EnableCap.Texture2D);

            // Restore Projection Matrix
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Modelview);

            GL.Enable(EnableCap.DepthTest);
        }

    }
}