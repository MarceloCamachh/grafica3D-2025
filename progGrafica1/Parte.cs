﻿using System;
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
            foreach (Poligono poligono in listaDePoligonos.Values)
            {
                poligono.SetCentro(centro);
            }
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
            Vector3 suma = Vector3.Zero;
            foreach (var poligono in listaDePoligonos.Values)
            {
                suma += poligono.CalcularCentroMasa();
            }

            if (listaDePoligonos.Count > 0)
                suma /= listaDePoligonos.Count;

            return suma;
        }

        public void Draw()
        {
            foreach (var poligono in this.listaDePoligonos.Values)
            {
                poligono.Draw();
            }
        }

        public void Trasladar(float dx, float dy, float dz)
        {
            foreach (var poligono in listaDePoligonos.Values)
                poligono.Trasladar(dx, dy, dz);
        }

        public void Escalar(float sx, float sy, float sz)
        {
            foreach (var poligono in listaDePoligonos.Values)
                poligono.Escalar(sx, sy, sz);
        }

        public void Rotar(float grados, Vector3 eje)
        {
            foreach (var poligono in listaDePoligonos.Values)
                poligono.Rotar(grados, eje);
        }
    }
}
