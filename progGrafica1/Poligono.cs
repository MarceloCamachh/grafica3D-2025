using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace progGrafica1
{
    public class Poligono
    {
        public List<Vector3> listaDeVertices;
        public Color4 color;
        private Vector3 centro;

        public Poligono(Color4 color)
        {
            this.listaDeVertices = new List<Vector3>();
            this.color = color;
        }

        public void SetColor(Color4 color)
        {
            this.color = color;
        }

        public void SetCentro(Vector3 centro)
        {
            this.centro = centro;
        }

        public List<Vector3> GetVertices()
        {
            return this.listaDeVertices;
        }

        public void Add(Vector3 v)
        {
            this.listaDeVertices.Add(v);
            this.centro = CalcularCentroMasa();
        }
        public void Trasladar(float dx, float dy, float dz)
        {
            Vector3 delta = new Vector3(dx, dy, dz);
            for (int i = 0; i < listaDeVertices.Count; i++)
                listaDeVertices[i] += delta;

            centro += delta;
        }
        public void Escalar(float sx, float sy, float sz)
        {
            for (int i = 0; i < listaDeVertices.Count; i++)
            {
                Vector3 relativo = listaDeVertices[i] - centro;
                relativo *= new Vector3(sx, sy, sz);
                listaDeVertices[i] = centro + relativo;
            }
        }
        public void Rotar(float grados, Vector3 eje)
        {
            Matrix3 rot = Matrix3.CreateFromAxisAngle(Vector3.Normalize(eje), MathHelper.DegreesToRadians(grados));
            for (int i = 0; i < listaDeVertices.Count; i++)
            {
                Vector3 relativo = listaDeVertices[i] - centro;
                relativo = Vector3.Transform(relativo, rot);
                listaDeVertices[i] = centro + relativo;
            }
        }
        public void Draw()
        {
            
            GL.Begin(PrimitiveType.Quads);

            foreach (Vector3 v in this.listaDeVertices)
            {
                GL.Color4(color);
                GL.Vertex3(v);
            }

            GL.End();
        }

        private Vector3 MinVertice()
        {
            Vector3 min = listaDeVertices[0];
            foreach (var v in listaDeVertices)
            {
                min.X = Math.Min(min.X, v.X);
                min.Y = Math.Min(min.Y, v.Y);
                min.Z = Math.Min(min.Z, v.Z);
            }
            return min;
        }

        private Vector3 MaxVertice()
        {
            Vector3 max = listaDeVertices[0];
            foreach (var v in listaDeVertices)
            {
                max.X = Math.Max(max.X, v.X);
                max.Y = Math.Max(max.Y, v.Y);
                max.Z = Math.Max(max.Z, v.Z);
            }
            return max;
        }

        public Vector3 CalcularCentroMasa()
        {
            Vector3 min = MinVertice();
            Vector3 max = MaxVertice();
            return new Vector3(
                (min.X + max.X) / 2f,
                (min.Y + max.Y) / 2f,
                (min.Z + max.Z) / 2f
            );
        }
    }
}
