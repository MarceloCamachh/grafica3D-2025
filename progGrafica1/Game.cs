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
        private Objeto computadora;
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
            computadora = Serializador.DeserializarObjeto<Objeto>("computadora.json");
            if (computadora != null)
                computadora.SetCentro(computadora.CalcularCentroMasa());

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

            computadora.Draw();

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
            float v = 1.0f;      // velocidad de traslación
            float r = 60.0f;     // vel. de rotación (°/s)
            float s = 1.5f;      // factor de escala incremental

            // Traslación (WASD + Q/E para Z)
            if (kb.IsKeyDown(Key.W)) computadora.Trasladar(0, v * dt, 0);
            if (kb.IsKeyDown(Key.S)) computadora.Trasladar(0, -v * dt, 0);
            if (kb.IsKeyDown(Key.A)) computadora.Trasladar(-v * dt, 0, 0);
            if (kb.IsKeyDown(Key.D)) computadora.Trasladar(v * dt, 0, 0);
            if (kb.IsKeyDown(Key.Q)) computadora.Trasladar(0, 0, v * dt);
            if (kb.IsKeyDown(Key.E)) computadora.Trasladar(0, 0, -v * dt);

            // Rotación con flechas + PgUp/PgDn
            if (kb.IsKeyDown(Key.Up)) computadora.Rotar(-r * dt, 0, 0);
            if (kb.IsKeyDown(Key.Down)) computadora.Rotar(r * dt, 0, 0);
            if (kb.IsKeyDown(Key.Left)) computadora.Rotar(0, -r * dt, 0);
            if (kb.IsKeyDown(Key.Right)) computadora.Rotar(0, r * dt, 0);
            if (kb.IsKeyDown(Key.PageUp)) computadora.Rotar(0, 0, r * dt);
            if (kb.IsKeyDown(Key.PageDown)) computadora.Rotar(0, 0, -r * dt);

            // Escala (Z para crecer, X para reducir)
            if (kb.IsKeyDown(Key.Z)) computadora.EscalarAcum(1 + (s - 1) * dt, 1 + (s - 1) * dt, 1 + (s - 1) * dt);
            if (kb.IsKeyDown(Key.X)) computadora.EscalarAcum(1 - (s - 1) * dt, 1 - (s - 1) * dt, 1 - (s - 1) * dt);
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
}
