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
    /// Clase ColeccionMoneda. Contenedora de monedas
    /// <remarks>
    /// ColeccionMoneda es un contenedor de monedas para la la ejecución de transacciones 
    /// </remarks>
    /// <seealso cref="Moneda">
    /// Información descriptiva de una moneda</seealso>    

    [Serializable]
    [XmlRoot(ElementName = "ColeccionMoneda")]
    public class ColeccionMoneda : IEnumerable<IMoneda>
    {
        private List<IMoneda> _Monedas;

        /// <summary>
        /// Constructor de la clase.</summary>
        public ColeccionMoneda()
        {
            _Monedas = new List<IMoneda>();
        }

        /// <summary>
        /// Devuelve una lista de monedas contenidos en una clase List. </summary>
        /// <returns>
        /// lista de monedas</returns>
        /// <seealso cref="Moneda">
        /// Descripción de una moneda</seealso>    
        /// <seealso cref="List"/>

        public List<IMoneda> todasLasMonedas
        {
            get { return _Monedas; }
        }

        /// <summary>
        /// Agrega una nueva moneda a la colección</summary>
        /// <param name="unaMoneda"> Clase moneda a agregar</param>
        /// <seealso cref="Moneda">
        /// Descripción de una moneda</seealso>    

        public void agregar(IMoneda unaMoneda)
        {
            _Monedas.Add(unaMoneda);
        }

        /// <summary>
        /// Inicializa (asignación a cero) de toda la existecia de moneda en la colección</summary>
        /// <returns>Verdaro si pudo realizar la operación correctamente</returns>
        /// <seealso cref="Moneda">
        /// Descripción de una moneda</seealso>    

        public bool inicializar()
        {
            double total = 0;
            foreach (IMoneda unaMoneda in _Monedas)
            {
                unaMoneda.inicializar();
                total += unaMoneda.saldo;
            }

            return (total == 0);
        }

        /// <summary>
        /// Busca y devuelve una moneda asociada al código pasado por parámetro.
        /// En caso de no encontrarse el código de moneda en la colección se lanzará la
        /// excepción MonedaInexistenteException</summary>
        /// <param name="codigo"> Código de la moneda a buscar</param>
        /// <returns>Clase Moneda asociada al código recibido</returns>
        /// <seealso cref="Moneda">
        /// Descripción de una moneda</seealso> 
        /// <seealso cref="MonedaInexistenteException">
        /// Excepción lanzada cuando la moneda a buscar no existe en la colección</seealso> 
        /// 
        public IMoneda obtenerMonedaPorCodigo(int codigo)
        {
            IMoneda retVal = null;
            foreach (IMoneda unaMoneda in _Monedas)
            {
                if (unaMoneda.codigo == codigo)
                {
                    retVal = unaMoneda;
                    break;
                }
            }

            if (retVal == null)
                throw new MonedaInexistenteException();

            return retVal;
        }

        /// <summary>
        /// Graba la serialización en formato xml de la colección</summary>
        /// <param name="nombreArchivo"> Nombre del archivo con el que se grabará la clase en formato xml</param>
        ///<returns>Verdadero si el archivo fue creado satisfactoriamente</returns>
        public bool serializarComoXML(string nombreArchivo)
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(ColeccionMoneda));
            // To write to a file, create a StreamWriter object.
            StreamWriter myWriter = new StreamWriter(nombreArchivo);
            mySerializer.Serialize(myWriter, this);
            myWriter.Close();

            return File.Exists(nombreArchivo);
        }

        IEnumerator<IMoneda> IEnumerable<IMoneda>.GetEnumerator()
        {
            return _Monedas.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Monedas.GetEnumerator();
        }
    }
}
