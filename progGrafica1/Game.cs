using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Newtonsoft.Json;


namespace progGrafica1
{
    public class Game : GameWindow
    {
        private Escenario escenario;
        private Objeto computadora;

        private ModoSeleccion modoSeleccion = ModoSeleccion.Escenario;

        private Objeto objetoActivo;
        private Parte parteActiva;
        private Poligono poligonoActivo;

        private List<string> objetosKeys;
        private int objetoIndex = 0;

        private List<string> partesKeys;
        private int parteIndex = 0;
        public Game(int width, int height)
            : base(width, height, GraphicsMode.Default, "ProgGrafica 2-2025")
        {

        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(0.0f, 0.0f, 0.5f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            //GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);

            /*
            computadora = new Objeto(new Dictionary<string, Parte>(), Vector3.Zero);
            Parte monitor = CrearCubo(Color4.Gray, 1.0f, 0.8f, 0.1f, new Vector3(0f, 0.3f, 0f));
            computadora.AddParte("Monitor", monitor);

            // CPU (cubo alto y profundo)
            Parte cpu = CrearCubo(Color4.Gray, 0.3f, 0.6f, 0.4f, new Vector3(1.0f, -0.3f, 0f));
            computadora.AddParte("CPU", cpu);

            // Teclado (cubo plano, bajito)
            Parte teclado = CrearCubo(Color4.DarkGray, 1.2f, 0.1f, 0.4f, new Vector3(0f, -0.6f, 0f));
            computadora.AddParte("Teclado", teclado);

            Parte pantalla = CrearCubo(Color4.Black, 0.9f, 0.6f, 0.01f, new Vector3(0f, 0.3f, 0.051f));
            computadora.AddParte("Pantalla", pantalla);
            // Botón en el CPU
            Parte botonCPU = CrearCubo(Color4.Red, 0.05f, 0.05f, 0.01f, new Vector3(1.15f, -0.45f, 0.21f));
            computadora.AddParte("BotonCPU", botonCPU);

            for (int i = 0; i < 10; i++)
            {
                float offset = -0.55f + i * 0.12f; // separación entre teclas
                Parte tecla = CrearCubo(Color4.White, 0.1f, 0.05f, 0.1f, new Vector3(offset, -0.58f, 0f));
                computadora.AddParte($"Tecla{i}", tecla);
            }*/
            //Serializador.SerializarObjeto(computadora,"computadora.json");
            escenario = new Escenario();

            Objeto computadora1 = Serializador.DeserializarObjeto<Objeto>("computadora.json");
            if (computadora1 != null)
            {
                computadora1.SetCentro(computadora1.CalcularCentroMasa());
                computadora1.Trasladar(-1.0f, 0, 0);
                escenario.AddObjeto("Computadora1", computadora1);
            }

            // --- Computadora 2 (misma, pero trasladada a la derecha) ---
            Objeto computadora2 = Serializador.DeserializarObjeto<Objeto>("computadora.json");
            if (computadora2 != null)
            {
                computadora2.SetCentro(computadora2.CalcularCentroMasa());
                computadora2.Trasladar(1.0f, 0, 0);  // mover 3 unidades en X
                escenario.AddObjeto("Computadora2", computadora2);
            }

            objetosKeys = escenario.GetObjetos().Select(o => escenario.GetNombreObjeto(o)).ToList();

            // Seleccionamos por defecto la primera computadora
            objetoActivo = escenario.GetObjeto(objetosKeys.First());

        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
            float aspectRatio = (float)Width / Height;
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45.0f), aspectRatio, 0.1f, 100.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            Matrix4 modelview = Matrix4.LookAt(
                new Vector3(1.5f, 2f, 3.5f),  // Cámara fija
                new Vector3(0.0f, 0.1f, 0.0f), // Mira al origen
                Vector3.UnitY);
            GL.LoadMatrix(ref modelview);

            escenario.Draw();

            DibujarEjes();
            SwapBuffers();
        }
        private void DibujarEjes()
        {
            GL.Begin(PrimitiveType.Lines);

            GL.Color3(1.0f, 0.0f, 0.0f); // X
            GL.Vertex3(-2.0f, 0.0f, 0.0f);
            GL.Vertex3(2.0f, 0.0f, 0.0f);

            GL.Color3(0.0f, 1.0f, 0.0f); // Y
            GL.Vertex3(0.0f, -2.0f, 0.0f);
            GL.Vertex3(0.0f, 2.0f, 0.0f);

            GL.Color3(0.0f, 0.0f, 1.0f); // Z
            GL.Vertex3(0.0f, 0.0f, -2.0f);
            GL.Vertex3(0.0f, 0.0f, 2.0f);

            GL.End();
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var kb = Keyboard.GetState();
            float dt = (float)e.Time;
            float v = 1.0f;   // velocidad de traslación
            float r = 60.0f;  // velocidad de rotación (grados/seg)
            float s = 1.5f;   // factor de escala incremental

            // --- Alternar modo de selección (Tab) ---
            if (kb.IsKeyDown(Key.Tab))
            {
                modoSeleccion = (ModoSeleccion)(((int)modoSeleccion + 1) % Enum.GetNames(typeof(ModoSeleccion)).Length);
                Console.WriteLine($"[Modo de selección]: {modoSeleccion}");
                System.Threading.Thread.Sleep(200); // debounce
            }

            // --- Cambiar objeto/parte activa (Enter) ---
            if (kb.IsKeyDown(Key.Enter))
            {
                if (modoSeleccion == ModoSeleccion.Objeto && objetosKeys.Count > 0)
                {
                    objetoIndex = (objetoIndex + 1) % objetosKeys.Count;
                    objetoActivo = escenario.GetObjeto(objetosKeys[objetoIndex]);
                    Console.WriteLine($"[Objeto activo]: {objetosKeys[objetoIndex]}");
                }
                else if (modoSeleccion == ModoSeleccion.Parte && objetoActivo != null)
                {
                    partesKeys = objetoActivo.listaDePartes.Keys.ToList();
                    if (partesKeys.Count > 0)
                    {
                        parteIndex = (parteIndex + 1) % partesKeys.Count;
                        parteActiva = objetoActivo.GetParte(partesKeys[parteIndex]);
                        Console.WriteLine($"[Parte activa]: {partesKeys[parteIndex]}");
                    }
                }
                System.Threading.Thread.Sleep(200); // debounce
            }

            // --- Transformaciones según el nivel seleccionado ---
            // Traslación (WASD + Q/E)
            if (kb.IsKeyDown(Key.W)) AplicarTraslacion(0, v * dt, 0);
            if (kb.IsKeyDown(Key.S)) AplicarTraslacion(0, -v * dt, 0);
            if (kb.IsKeyDown(Key.A)) AplicarTraslacion(-v * dt, 0, 0);
            if (kb.IsKeyDown(Key.D)) AplicarTraslacion(v * dt, 0, 0);
            if (kb.IsKeyDown(Key.Q)) AplicarTraslacion(0, 0, v * dt);
            if (kb.IsKeyDown(Key.E)) AplicarTraslacion(0, 0, -v * dt);

            // Rotación (Flechas + PgUp/PgDn)
            if (kb.IsKeyDown(Key.Up)) AplicarRotacion(-r * dt, Vector3.UnitX);
            if (kb.IsKeyDown(Key.Down)) AplicarRotacion(r * dt, Vector3.UnitX);
            if (kb.IsKeyDown(Key.Left)) AplicarRotacion(-r * dt, Vector3.UnitY);
            if (kb.IsKeyDown(Key.Right)) AplicarRotacion(r * dt, Vector3.UnitY);
            if (kb.IsKeyDown(Key.N)) AplicarRotacion(r * dt, Vector3.UnitZ);
            if (kb.IsKeyDown(Key.M)) AplicarRotacion(-r * dt, Vector3.UnitZ);

            // Escalado (Z / X)
            if (kb.IsKeyDown(Key.Z)) AplicarEscala(1 + (s - 1) * dt, 1 + (s - 1) * dt, 1 + (s - 1) * dt);
            if (kb.IsKeyDown(Key.X)) AplicarEscala(1 - (s - 1) * dt, 1 - (s - 1) * dt, 1 - (s - 1) * dt);
        }
        private void AplicarTraslacion(float dx, float dy, float dz)
        {
            switch (modoSeleccion)
            {
                case ModoSeleccion.Escenario:
                    escenario.Trasladar(dx, dy, dz);
                    break;
                case ModoSeleccion.Objeto:
                    objetoActivo?.Trasladar(dx, dy, dz);
                    break;
                case ModoSeleccion.Parte:
                    parteActiva?.Trasladar(dx, dy, dz);
                    break;
            }
        }

        private void AplicarRotacion(float grados, Vector3 eje)
        {
            switch (modoSeleccion)
            {
                case ModoSeleccion.Escenario:
                    escenario.Rotar(grados, eje);
                    break;
                case ModoSeleccion.Objeto:
                    objetoActivo?.Rotar(grados, eje);
                    break;
                case ModoSeleccion.Parte:
                    parteActiva?.Rotar(grados, eje);
                    break;
            }
        }

        private void AplicarEscala(float sx, float sy, float sz)
        {
            switch (modoSeleccion)
            {
                case ModoSeleccion.Escenario:
                    escenario.Escalar(sx, sy, sz);
                    break;
                case ModoSeleccion.Objeto:
                    objetoActivo?.Escalar(sx, sy, sz);
                    break;
                case ModoSeleccion.Parte:
                    parteActiva?.Escalar(sx, sy, sz);
                    break;
            }
        }

        Parte CrearCubo(Color4 color, float ancho, float alto, float profundo, Vector3 centro)
        {
            Parte cubo = new Parte();

            float x = ancho / 2f;
            float y = alto / 2f;
            float z = profundo / 2f;

            // Frente
            Poligono frente = new Poligono(color);
            frente.Add(new Vector3(-x, y, z) + centro);
            frente.Add(new Vector3(x, y, z) + centro);
            frente.Add(new Vector3(x, -y, z) + centro);
            frente.Add(new Vector3(-x, -y, z) + centro);
            cubo.Add("Frente", frente);

            // Atrás
            Poligono atras = new Poligono(color);
            atras.Add(new Vector3(x, y, -z) + centro);
            atras.Add(new Vector3(-x, y, -z) + centro);
            atras.Add(new Vector3(-x, -y, -z) + centro);
            atras.Add(new Vector3(x, -y, -z) + centro);
            cubo.Add("Atras", atras);

            // Arriba
            Poligono arriba = new Poligono(color);
            arriba.Add(new Vector3(-x, y, -z) + centro);
            arriba.Add(new Vector3(x, y, -z) + centro);
            arriba.Add(new Vector3(x, y, z) + centro);
            arriba.Add(new Vector3(-x, y, z) + centro);
            cubo.Add("Arriba", arriba);

            // Abajo
            Poligono abajo = new Poligono(color);
            abajo.Add(new Vector3(-x, -y, z) + centro);
            abajo.Add(new Vector3(x, -y, z) + centro);
            abajo.Add(new Vector3(x, -y, -z) + centro);
            abajo.Add(new Vector3(-x, -y, -z) + centro);
            cubo.Add("Abajo", abajo);

            // Izquierda
            Poligono izquierda = new Poligono(color);
            izquierda.Add(new Vector3(-x, y, -z) + centro);
            izquierda.Add(new Vector3(-x, y, z) + centro);
            izquierda.Add(new Vector3(-x, -y, z) + centro);
            izquierda.Add(new Vector3(-x, -y, -z) + centro);
            cubo.Add("Izquierda", izquierda);

            // Derecha
            Poligono derecha = new Poligono(color);
            derecha.Add(new Vector3(x, y, z) + centro);
            derecha.Add(new Vector3(x, y, -z) + centro);
            derecha.Add(new Vector3(x, -y, -z) + centro);
            derecha.Add(new Vector3(x, -y, z) + centro);
            cubo.Add("Derecha", derecha);

            return cubo;
        }

    }
    public enum ModoSeleccion
    {
        Escenario,
        Objeto,
        Parte,
        Poligono
    }
}
