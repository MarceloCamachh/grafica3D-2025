using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace progGrafica1
{
    public class Serializador
    {
        // Serializar objeto con manejo de ciclos de referencia
        public static void SerializarObjeto<T>(T objeto, string rutaArchivo)
        {
            try
            {
                // Configurar JsonSerializerSettings para evitar ciclos de referencia
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore, // Evitar ciclos de referencia
                    Formatting = Formatting.Indented, // Formato legible para el JSON (opcional)
                    Converters = new List<JsonConverter> { new Vector2Converter(), new Vector3Converter() } // Usar los convertidores personalizados
                };

                // Serializar el objeto usando la configuración personalizada
                string json = JsonConvert.SerializeObject(objeto, settings);
                File.WriteAllText(rutaArchivo, json);
                Console.WriteLine($"Objeto serializado y guardado en el archivo: {rutaArchivo}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al serializar el objeto: {ex.Message}");
            }
        }

        // Deserializar objeto
        public static T DeserializarObjeto<T>(string rutaArchivo)
        {
            try
            {
                // Leer el JSON del archivo
                string json = File.ReadAllText(rutaArchivo);
                T objeto = JsonConvert.DeserializeObject<T>(json);
                Console.WriteLine($"Objeto deserializado desde el archivo: {rutaArchivo}");
                return objeto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al deserializar el objeto: {ex.Message}");
                return default(T);
            }
        }
    }

}
