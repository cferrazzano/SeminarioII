using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace OperacionCaja
{
    /// <summary>
    /// Clase ColeccionArticulo. Contenedora de artículos
    /// </summary>
    /// <remarks>
    /// ColeccionArticulo es un contenedor de artículos para la la ejecución de transacciones de venta
    /// </remarks>
    /// <seealso cref="Articulo">
    /// Información descriptiva de un artículo </seealso>    

    [Serializable]
    [XmlRoot(ElementName = "ColeccionArticulo")]
    public class ColeccionArticulo : IEnumerable<IArticulo>
    {
        private List<IArticulo> _articulos;
        private ISCProducto _integracionProducto;

        /// <summary>
        /// Constructor de la clase.</summary>
        public ColeccionArticulo(ISCProducto integracionProducto)
        {
            _articulos = new List<IArticulo>();
            _integracionProducto = integracionProducto;
        }

        /// <summary>
        /// Devuelve una lista de articulos contenidos en una clase List. </summary>
        /// <returns>
        /// lista de artículos</returns>
        /// <seealso cref="Articulo">
        /// Descripción de un artículo </seealso>    
        /// <seealso cref="List"/>

        public List<IArticulo> todosLosArticulos
        {
            get { return _articulos; }
        }

        /// <summary>
        /// Agrega un nuevo artículo a la colección</summary>
        /// <param name="unArticulo"> Clase articulo a agregar</param>
        /// <seealso cref="Articulo">
        /// Descripción de un artículo</seealso>    

        public void agregar(IArticulo unArticulo)
        {
            if (unArticulo.precio == 0)
                unArticulo.precio = _integracionProducto.obtenerPrecio(unArticulo.codigo);
            _articulos.Add(unArticulo);
        }

        /// <summary>
        /// Graba la serialización en formato xml de la colección</summary>
        /// <param name="nombreArchivo"> Nombre del archivo con el que se grabará la clase en formato xml</param>
        ///<returns>Verdadero si el archivo fue creado satisfactoriamente</returns>
        public bool serializarComoXML(string nombreArchivo)
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(ColeccionArticulo));
            // To write to a file, create a StreamWriter object.
            StreamWriter myWriter = new StreamWriter(nombreArchivo);
            mySerializer.Serialize(myWriter, this);
            myWriter.Close();

            return File.Exists(nombreArchivo);
        }

        IEnumerator<IArticulo> IEnumerable<IArticulo>.GetEnumerator()
        {
            return _articulos.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _articulos.GetEnumerator();
        }
    }

}

