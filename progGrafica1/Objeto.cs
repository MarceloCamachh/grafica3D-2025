using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK;
using Newtonsoft.Json;
using OpenTK.Graphics.OpenGL;

namespace progGrafica1
{
    public class Objeto
    {
        public Dictionary<string, Parte> listaDePartes;
        private Vector3 centro;
        public Color4 color;

        public Vector3 Traslacion = Vector3.Zero;
        public Vector3 RotacionEuler = Vector3.Zero; // en grados (X,Y,Z)
        public Vector3 Escala = Vector3.One;

        public Objeto(Dictionary<string, Parte> list, Vector3 centro)
        {
            this.listaDePartes = list;
            this.centro = centro;
        }

        public void AddParte(string nombre, Parte nuevaParte)
        {
            listaDePartes.Add(nombre, nuevaParte);
        }

        public Vector3 GetCentro()
        {
            return this.centro;
        }

        public void SetCentro(Vector3 centro)
        {
            this.centro = centro;
            foreach (Parte parteActual in listaDePartes.Values)
            {
                parteActual.SetCentro(centro);
            }
        }

        public void SetColor(string parte, string poligono, Color4 color)
        {
            this.color = color;
            listaDePartes[parte].SetColor(this.color);
        }

        public Parte GetParte(string nombre)
        {
            return this.listaDePartes[nombre];
        }
        public void Trasladar(float dx, float dy, float dz) => Traslacion += new Vector3(dx, dy, dz);
        public void Rotar(float gx, float gy, float gz) => RotacionEuler += new Vector3(gx, gy, gz);
        public void EscalarAcum(float sx, float sy, float sz) => Escala *= new Vector3(sx, sy, sz); // multiplicativa

        public void Draw()
        {
            GL.PushMatrix();    
            GL.Translate(Traslacion);
            GL.Translate(centro);

            if (RotacionEuler.X != 0) GL.Rotate(RotacionEuler.X, 1, 0, 0);
            if (RotacionEuler.Y != 0) GL.Rotate(RotacionEuler.Y, 0, 1, 0);
            if (RotacionEuler.Z != 0) GL.Rotate(RotacionEuler.Z, 0, 0, 1);

            GL.Scale(Escala);
            GL.Translate(-centro);
            foreach (var parte in this.listaDePartes.Values)
            {
                parte.Draw();
            }
            GL.PopMatrix();
        }

        public Vector3 CalcularCentroMasa()
        {
            Vector3 suma = Vector3.Zero;
            foreach (var parte in listaDePartes.Values)
            {
                suma += parte.CalcularCentroMasa();
            }

            if (listaDePartes.Count > 0)
                suma /= listaDePartes.Count;

            return suma;
        }
    }
}
