using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK;

namespace progGrafica1
{
    public class Parte
    {
        public Dictionary<string, Poligono> listaDePoligonos;
        private Vector3 centro;
        public Color4 color;

        public Parte()
        {
            this.listaDePoligonos = new Dictionary<string, Poligono>();
            this.color = new Color4(0, 0, 0, 1f);
        }

        public void Add(string nombre, Poligono p)
        {
            if (!listaDePoligonos.ContainsKey(nombre))
                listaDePoligonos.Add(nombre, p);
        }

        public Poligono GetPoligono(string nombre)
        {
            return listaDePoligonos.ContainsKey(nombre) ? listaDePoligonos[nombre] : null;
        }

        public Vector3 GetCentro()
        {
            return this.centro;
        }

        public void SetCentro(Vector3 centro)
        {
            this.centro = centro;
        }

        public void SetColor(Color4 color)
        {
            this.color = color;
            foreach (var poligono in listaDePoligonos.Values)
            {
                poligono.SetColor(this.color);
            }
        }

        public Vector3 CalcularCentroMasa()
        {
            // Promedio de centros de polígonos
            if (listaDePoligonos.Count == 0) return centro;
            Vector3 suma = Vector3.Zero;
            foreach (var p in listaDePoligonos.Values)
                suma += p.CalcularCentroMasa();
            return suma / listaDePoligonos.Count;
        }

        // ✅ Rotar/escala de la PARTE respecto a su propio centro:
        public void Rotar(float grados, Vector3 eje)
        {
            var pivote = CalcularCentroMasa(); // o usa this.centro si lo mantienes actualizado
            foreach (var p in listaDePoligonos.Values)
                p.RotarSobre(grados, eje, pivote);
        }

        public void Escalar(float sx, float sy, float sz)
        {
            var pivote = CalcularCentroMasa();
            foreach (var p in listaDePoligonos.Values)
                p.EscalarSobre(sx, sy, sz, pivote);
        }

        // Traslación no necesita pivote especial
        public void Trasladar(float dx, float dy, float dz)
        {
            foreach (var p in listaDePoligonos.Values)
                p.Trasladar(dx, dy, dz);
        }

        public void Draw()
        {
            foreach (var poligono in this.listaDePoligonos.Values)
            {
                poligono.Draw();
            }
        }
   
    }
}
