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
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SolarSystemSimulation
{
    /// <summary>
    /// An astronomical body such as a planet or a star
    /// </summary>
    public class AstronomicalBody
    {
        private const double G = 6.67430e-11;

        // Properties of the AstronomicalBody
        public string Name { get; set; }
        public double Mass { get; set; } // In kilograms
        public double Radius { get; set; } // In meters
        public int TextureId { get; set; }

        // State variables of the AstronomicalBody
        public double X { get; set; } // X position in meters
        public double Y { get; set; } // Y position in meters
        public double Z { get; set; } // Z position in meters
        public double Vx { get; set; } // X velocity in meters/second
        public double Vy { get; set; } // Y velocity in meters/second
        public double Vz { get; set; } // Z velocity in meters/second

        // Rotation of the AstronomicalBody
        public double Rotation { get; set; } // In radians
        public double RotationPeriod { get; set; } // In Earth days

        // The vertices of the sphere
        private static readonly List<Vector3> _vertices = new List<Vector3>();
        private static int _stacks;
        private static int _slices;

        /// <summary>
        /// Creates a new body with the specified properties
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mass"></param>
        /// <param name="radius"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="vx"></param>
        /// <param name="vy"></param>
        /// <param name="vz"></param>
        /// <param name="rotationPeriod"></param>
        public AstronomicalBody(string name, double mass, double radius, double x, double y, double z, double vx, double vy, double vz, double rotationPeriod)
        {
            Name = name;
            Mass = mass;
            Radius = radius;
            X = x;
            Y = y;
            Z = z;
            Vx = vx;
            Vy = vy;
            Vz = vz;
            RotationPeriod = rotationPeriod;
            Rotation = 0;
            BuildSphere(1.0f, 64, 64);
        }

        /// <summary>
        /// Updates the velocity of the body based on the total force acting on it
        /// </summary>
        /// <param name="bodies"></param>
        /// <param name="elapsedTime"></param>
        public void UpdateVelocity(List<AstronomicalBody> bodies, double elapsedTime)
        {
            if (Name.Equals("Sun", StringComparison.OrdinalIgnoreCase))
            {
                //we don't move the sun
                return;
            }

            const double MIN_DISTANCE = 1e7;

            double ax = 0, ay = 0, az = 0;

            foreach (var other in bodies)
            {
                if (other == this) continue;

                double dx = other.X - X;
                double dy = other.Y - Y;
                double dz = other.Z - Z;
                double r = Math.Sqrt(dx * dx + dy * dy + dz * dz);

                if (r < MIN_DISTANCE) continue;

                double force = G * (Mass * other.Mass) / (r * r);

                ax += (force * dx / r) / Mass;
                ay += (force * dy / r) / Mass;
                az += (force * dz / r) / Mass;
            }

            Vx += ax * elapsedTime * 10;
            Vy += ay * elapsedTime * 10;
            Vz += az * elapsedTime * 10;
        }


        /// <summary>
        /// Updates the position of the body based on its velocity
        /// </summary>
        /// <param name="elapsedTime"></param>
        public void UpdatePosition(double elapsedTime)
        {
            if (Name.Equals("Sun", StringComparison.OrdinalIgnoreCase))
            {
                //we don't move the sun
                return;
            }

            // Update the position of the body based on its velocity
            X += Vx * elapsedTime * 10;
            Y += Vy * elapsedTime * 10;
            Z += Vz * elapsedTime * 10;
        }

        /// <summary>
        /// Updates the rotation of the body based on its rotation period in seconds
        /// </summary>
        /// <param name="elapsedTime">Elapsed simulation time in seconds</param>
        public void UpdateRotation(double elapsedTime)
        {
            if (RotationPeriod == 0) return;

            // Berechnung der Rotationsänderung: 360° für eine vollständige Umdrehung
            double rotationDelta = (elapsedTime * 10 / (RotationPeriod)) * 360.0;

            // Rotation anwenden
            Rotation += rotationDelta;

            // Begrenzung der Rotation auf 360°, um Überlauf zu vermeiden
            Rotation %= 360.0;
        }

        /// <summary>
        /// Creates a sphere with the specified radius, number of slices and stacks
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="slices"></param>
        /// <param name="stacks"></param>
        private void BuildSphere(float radius, int slices, int stacks)
        {
            if (_slices == slices && _stacks == stacks)
            {
                return;
            }

            _stacks = stacks;
            _slices = slices;
            _vertices.Clear();

            // Calculate the vertices of the sphere            
            for (int i = 0; i <= stacks; i++)
            {
                float phi = (float)(Math.PI / 2 - i * Math.PI / stacks);

                float y = (float)(radius * Math.Sin(phi));
                float r = (float)(radius * Math.Cos(phi));

                for (int j = 0; j <= slices; j++)
                {
                    float theta = (float)(2 * j * Math.PI / slices);
                    float x = (float)(r * Math.Cos(theta));
                    float z = (float)(r * Math.Sin(theta));
                    _vertices.Add(new Vector3(x, y, z));
                }
            }
        }

        /// <summary>
        /// Draws the body as a textured sphere
        /// </summary>
        /// <param name="scale"></param>
        public void DrawTexturedBody(double scale)
        {
            GL.PushMatrix();
            GL.Translate((float)X * scale, (float)Y * scale, (float)Z * scale);
            GL.Scale(10000, 10000, 10000);


            //rotate the AstronomicalBody
            GL.Rotate(90, 1, 0, 0);
            GL.Rotate(Rotation, 0, 1, 0);

            GL.Material(MaterialFace.Front, MaterialParameter.Ambient, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
            GL.Material(MaterialFace.Front, MaterialParameter.Shininess, 128.0f);

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, TextureId);

            // Render the sphere as a series of triangles and its normals
            GL.Begin(PrimitiveType.TriangleStrip);
            for (int i = 0; i < _stacks; i++)
            {
                for (int j = 0; j <= _slices; j++)
                {
                    Vector3 v = _vertices[i * (_slices + 1) + j];
                    Vector3 n = Vector3.Normalize(v);
                    GL.Normal3(n);
                    GL.TexCoord2((float)j / _slices, (float)i / _stacks);
                    GL.Vertex3(v);
                    v = _vertices[(i + 1) * (_slices + 1) + j];
                    n = Vector3.Normalize(v);
                    GL.Normal3(n);
                    GL.TexCoord2((float)j / _slices, (float)(i + 1) / _stacks);
                    GL.Vertex3(v);
                }
            }
            GL.End();

            GL.PopMatrix();
        }
    }
}