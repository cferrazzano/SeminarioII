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
    /// Clase ColeccionMovimiento. Contenedora de movimientos
    /// <remarks>
    /// ColeccionMovimiento es un contenedor de movimientos producto de la ejecución de transacciones
    /// </remarks>
    /// <seealso cref="Movimiento">
    /// Información descriptiva de un movimiento</seealso>    

    [Serializable]
    [XmlRoot(ElementName = "ColeccionMovimiento")]
    public class ColeccionMovimiento : IEnumerable<IMovimiento>
    {
        private const int moneda_pesos = 0;
        private List<IMovimiento> _Movimientos;

        /// <summary>
        /// Constructor de la clase.</summary>
        public ColeccionMovimiento()
        {
            _Movimientos = new List<IMovimiento>();
        }

        /// <summary>
        /// Devuelve una lista de movimientos contenidos en una clase List. </summary>
        /// <returns>
        /// lista de movimientos</returns>
        /// <seealso cref="Movimiento">
        /// Descripción de un movimiento</seealso>    
        /// <seealso cref="List"/>

        public List<IMovimiento> todosLosMovimientos
        {
            get { return _Movimientos; }
        }

        /// <summary>
        /// Busca y devuelve un sub conjunto de movimientos con un mismo código y subcódigo pasados por 
        /// parámetro 
        /// </summary>
        /// <param name="codigo">Código de transacción</param>
        /// <param name="subcodigo">Subcódigo de transacción</param>
        /// <returns>Coleccion de movimientos</returns>

        public ColeccionMovimiento movimientosPorCodigoSubCodigo(int codigo, int subcodigo)
        {
            ColeccionMovimiento retVal = new ColeccionMovimiento();

            foreach (IMovimiento unMov in _Movimientos)
            {
                if (unMov.codigo == codigo && unMov.subcodigo == subcodigo)
                    retVal.agregar(unMov);
            }

            return retVal; 
        }

        /// <summary>
        /// Busca y devuelve un sub conjunto de movimientos con la moneda pasada por parámetro como
        /// moneda principal (moneda1) de la transacción
        /// </summary>
        /// <param name="moneda">Moneda principal de la transacción</param>
        /// <returns>Coleccion de movimientos</returns>
        public ColeccionMovimiento movimientosPorMoneda(int moneda)
        {
            ColeccionMovimiento retVal = new ColeccionMovimiento();

            foreach (IMovimiento unMov in _Movimientos)
            {
                if (unMov.idMoneda1 == moneda)
                    retVal.agregar(unMov);
            }

            return retVal;
        }

        /// <summary>
        /// Busca y devuelve un sub conjunto de movimientos asociados a una misma referencia
        /// </summary>
        /// <param name="referencia">Referencia usada en el movimiento</param>
        /// <returns>Coleccion de movimientos</returns>
        public ColeccionMovimiento movimientosPorReferencia(string referencia)
        {
            ColeccionMovimiento retVal = new ColeccionMovimiento();

            foreach (IMovimiento unMov in _Movimientos)
            {
                if (unMov.referencia == referencia)
                    retVal.agregar(unMov);
            }

            return retVal;
        }

        /// <summary>
        /// Agrega un nuevo movimiento a la colección</summary>
        /// <param name="unMovimiento"> Clase movimiento a agregar</param>
        /// <seealso cref="Movimiento">
        /// Descripción del movimiento</seealso>    
        /// 
        public void agregar(IMovimiento unMovimiento)
        {
            _Movimientos.Add(unMovimiento);
        }

        /// <summary>
        /// Inicializa la colección dejándola vacía
        /// </summary>
        /// <returns>Verdadero si pudo realizar la operación correctamente</returns>
        public bool inicializar()
        {
            _Movimientos.Clear();
            return (_Movimientos.Count == 0);
        }

        /// <summary>
        /// Busca y devuelve una movimiento asociado al número de operación pasado por parámetro.
        /// En caso de no encontrarse la operación en la colección se lanzará la
        /// excepción OperacionInexistenteException</summary>
        /// <param name="numero"> Número de operación a buscar</param>
        /// <returns>Clase Movimiento asociado al código recibido</returns>
        /// <seealso cref="Movimiento">
        /// Descripción del movimiento</seealso> 
        /// <seealso cref="OperacionInexistenteException">
        /// Excepción lanzada cuando el movimiento a buscar no existe en la colección</seealso> 
        /// 
        public IMovimiento obtenerMovimientoPorOperacion(int numero)
        {
            IMovimiento retVal = null;
            foreach (IMovimiento unMov in _Movimientos)
            {
                if (unMov.operacion == numero)
                {
                    retVal = unMov;
                    break;
                }
            }

            if (retVal == null)
                throw new OperacionInexistenteException();

            return retVal;
        }

        /// <summary>
        /// Graba la serialización en formato xml de la colección</summary>
        /// <param name="nombreArchivo"> Nombre del archivo con el que se grabará la clase en formato xml</param>
        ///<returns>Verdadero si el archivo fue creado satisfactoriamente</returns>

        public bool SerializarComoXML(string nombreArchivo)
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(ColeccionMovimiento));
            // To write to a file, create a StreamWriter object.
            StreamWriter myWriter = new StreamWriter(nombreArchivo);
            mySerializer.Serialize(myWriter, this);
            myWriter.Close();

            return File.Exists(nombreArchivo);
        }

        IEnumerator<IMovimiento> IEnumerable<IMovimiento>.GetEnumerator()
        {
            return _Movimientos.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Movimientos.GetEnumerator();
        }
    }
}
