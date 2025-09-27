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
        
        public void Draw()
        {
            foreach (var parte in this.listaDePartes.Values)
            {
                parte.Draw();
            }
        }

        public Vector3 CalcularCentroMasa()
        {
            if (listaDePartes.Count == 0) return centro;
            Vector3 suma = Vector3.Zero;
            int count = 0;
            foreach (var parte in listaDePartes.Values)
            {
                suma += parte.CalcularCentroMasa();
                count++;
            }
            return count > 0 ? suma / count : centro;
        }

        public void Rotar(float grados, Vector3 eje)
        {
            var pivote = CalcularCentroMasa();
            foreach (var parte in listaDePartes.Values)
            {
                // Rotar cada polígono de la parte respecto al pivote del OBJETO
                foreach (var p in parte.listaDePoligonos.Values)
                    p.RotarSobre(grados, eje, pivote);
            }
        }

        public void Escalar(float sx, float sy, float sz)
        {
            var pivote = CalcularCentroMasa();
            foreach (var parte in listaDePartes.Values)
            {
                foreach (var p in parte.listaDePoligonos.Values)
                    p.EscalarSobre(sx, sy, sz, pivote);
            }
        }

        public void Trasladar(float dx, float dy, float dz)
        {
            foreach (var parte in listaDePartes.Values)
                parte.Trasladar(dx, dy, dz);
        }
    }
}
