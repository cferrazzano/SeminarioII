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
    /// Clase ColeccionTotalizador. Contenedora de Totalizadores
    /// <remarks>
    /// ColeccionTotalizador es un contenedor de totalizadores producto de la ejecución de transacciones
    /// </remarks>
    /// <seealso cref="Totalizador">
    /// Información de ejecución de un totalizador</seealso>    

    [Serializable]
    [XmlRoot(ElementName = "ColeccionTotalizador")]
    public class ColeccionTotalizador : IEnumerable<ITotalizador>
    {
        private List<ITotalizador> _Totalizadores;

        /// <summary>
        /// Método constructor
        /// </summary>
        public ColeccionTotalizador()
        {
            _Totalizadores = new List<ITotalizador>();
        }

        /// <summary>
        /// Devuelve una lista de totalizadores contenidos en una clase List. </summary>
        /// <returns>
        /// lista de totalizadores</returns>
        /// <seealso cref="Totalizador">
        /// Información de ejecución de un totalizador</seealso>    
        /// <seealso cref="List"/>
        public List<ITotalizador> todosLosTotalizadores
        {
            get { return _Totalizadores; }
        }

        /// <summary>
        /// Agrega un nuevo totalizador a la colección</summary>
        /// <param name="unTotalizador"> Clase totalizador a agregar</param>
        /// <seealso cref="Totalizador">
        /// Clase totalizador a administrar</seealso>    
        /// 

        public void agregar(ITotalizador unTotalizador)
        {
            _Totalizadores.Add(unTotalizador);
        }


        /// <summary>
        /// Inicializa todos los totalizadores (asignación de totales a cero)
        /// </summary>
        /// <returns>Verdadero si logra realizar la operación correctamente</returns>
        public bool inicializar()
        {
            double total = 0;
            foreach (ITotalizador unTot in _Totalizadores)
            {
                unTot.inicializar();
                total += unTot.importeAsociado;
            }

            return (total == 0);
        }

        /// <summary>
        /// Busca y devuelve el totalizador asociado al código recibido por parámetro.
        /// Si el código de totalizador no se encuentra en la colección se lanza la excepción
        /// TotalizadorInexistenteException
        /// </summary>
        /// <param name="codigo">Código del totalizador buscado</param>
        /// <returns>clase Totalizador asociada al código</returns>
        /// <seealso cref="Totalizador"/>

        public ITotalizador obtenerTotalizadorPorCodigo(int codigo)
        {
            ITotalizador retVal = null;
            foreach (ITotalizador unTot in _Totalizadores)
            {
                if (unTot.codigo == codigo)
                {
                    retVal = unTot;
                    break;
                }
            }

            if (retVal == null)
                throw new TotalizadorInexistenteException();

            return retVal;
        }

        /// <summary>
        /// Graba la serialización en formato xml de la colección</summary>
        /// <param name="nombreArchivo"> Nombre del archivo con el que se grabará la clase en formato xml</param>
        ///<returns>Verdadero si el archivo fue creado satisfactoriamente</returns>

        public bool serializarComoXML(string nombreArchivo)
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(ColeccionTotalizador));
            // To write to a file, create a StreamWriter object.
            StreamWriter myWriter = new StreamWriter(nombreArchivo);
            mySerializer.Serialize(myWriter, this);
            myWriter.Close();

            return File.Exists(nombreArchivo);
        }

        IEnumerator<ITotalizador> IEnumerable<ITotalizador>.GetEnumerator()
        {
            return _Totalizadores.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Totalizadores.GetEnumerator();
        }
    }
}
