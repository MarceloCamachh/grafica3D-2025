using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK;
using Newtonsoft.Json;

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
            if (!listaDePartes.ContainsKey(nombre))
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
            return listaDePartes.ContainsKey(nombre) ? listaDePartes[nombre] : null;
        }
        public void Trasladar(float dx, float dy, float dz)
        {
            foreach (var parte in listaDePartes.Values)
                parte.Trasladar(dx, dy, dz);
        }

        public void Escalar(float sx, float sy, float sz)
        {
            foreach (var parte in listaDePartes.Values)
                parte.Escalar(sx, sy, sz);
        }

        public void Rotar(float grados, Vector3 eje)
        {
            foreach (var parte in listaDePartes.Values)
                parte.Rotar(grados, eje);
        }
        public void Draw()
        {
            foreach (var parte in this.listaDePartes.Values)
            {
                parte.Draw();
            }
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
