using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace progGrafica1
{
    public class Escenario
    {
        private Dictionary<string, Objeto> listaDeObjetos;

        public Escenario()
        {
            listaDeObjetos = new Dictionary<string, Objeto>();
        }

        // --- Gestión de Objetos ---
        public void AddObjeto(string nombre, Objeto objeto)
        {
            if (!listaDeObjetos.ContainsKey(nombre))
                listaDeObjetos.Add(nombre, objeto);
            else
                Console.WriteLine($"El objeto '{nombre}' ya existe en el escenario.");
        }

        public void RemoveObjeto(string nombre)
        {
            if (listaDeObjetos.ContainsKey(nombre))
                listaDeObjetos.Remove(nombre);
        }

        public Objeto GetObjeto(string nombre)
        {
            if (listaDeObjetos.ContainsKey(nombre))
                return listaDeObjetos[nombre];
            return null;
        }

        public IEnumerable<Objeto> GetObjetos()
        {
            return listaDeObjetos.Values;
        }

        // --- Dibujado de todo el escenario ---
        public void Draw()
        {
            foreach (var objeto in listaDeObjetos.Values)
            {
                objeto.Draw();
            }
        }

        // --- Transformaciones globales ---
        public void Trasladar(float dx, float dy, float dz)
        {
            foreach (var objeto in listaDeObjetos.Values)
                objeto.Trasladar(dx, dy, dz);
        }

        public void Escalar(float sx, float sy, float sz)
        {
            foreach (var objeto in listaDeObjetos.Values)
                objeto.Escalar(sx, sy, sz);
        }

        public void Rotar(float grados, Vector3 eje)
        {
            foreach (var objeto in listaDeObjetos.Values)
                objeto.Rotar(grados, eje);
        }

        // --- Calcular el centro de masa global del escenario ---
        public Vector3 CalcularCentroMasa()
        {
            Vector3 suma = Vector3.Zero;
            int count = 0;

            foreach (var objeto in listaDeObjetos.Values)
            {
                suma += objeto.CalcularCentroMasa();
                count++;
            }

            if (count > 0)
                suma /= count;

            return suma;
        }
    }
}
