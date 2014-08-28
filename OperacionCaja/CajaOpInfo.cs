using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace OperacionCaja
{
    /// <summary>
    /// Clase TotalizadorInfo
    /// <remarks>
    /// Clase descriptiva de un totalizador asociado al sistema
    /// </remarks>

    public class TotalizadorInfo
    {
        private int _codigo;
        private string _descripcion;

        /// <summary>
        /// Constructor de la clase.</summary>
        /// <param name="nCodigo"> Código de totalizador</param>
        /// <param name="sDescripcion"> Descripción del totalizador</param>
        /// 

        public TotalizadorInfo(int nCodigo, string sDescripcion)
        {
            _codigo = nCodigo;
            _descripcion = sDescripcion;
        }

        /// <summary>
        /// Propiedad codigo</summary>
        /// <value>
        /// Número identificador del totalizador. Debe ser único. Sirve para referenciar el totalizador
        /// </value>

        public int codigo
        {
            get {return _codigo;}
            set { _codigo = value; }
        }

        /// <summary>
        /// Propiedad descripcion</summary>
        /// <value>
        /// Descripción del totalizador
        /// </value>
        public string descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }
    }


    /// <summary>
    /// Clase TotalizadorOperacion
    /// <remarks>
    /// Clase descriptiva de un totalizador y su comportamiento una vez asociado a una transacción
    /// </remarks>

    public class TotalizadorOperacion
    {
        private int _codigo;
        private string _operacion;
        private int _moneda;

        /// <summary>
        /// Constructor de la clase.</summary>
        /// <param name="nCodigo"> Código de totalizador</param>
        /// <param name="sOperacion"> Valores posibles: "+" o "-". Indica si el totalizador se incrementará
        /// o decrementará una vez asociado a una transacción</param>
        /// <param name="nMoneda"> Número de moneda utilizada para la actualización del totalizador.
        /// Valores posibles 1 o 2 (primer o segunda moneda asociada a la transacción)
        /// </param>
        /// 

        public TotalizadorOperacion(int nCodigo, string sOperacion, int nMoneda)
        {
            _codigo = nCodigo;
            _operacion = sOperacion;
            _moneda = nMoneda;
        }

        /// <summary>
        /// Propiedad codigo </summary>
        /// <value>
        /// código del totalizador que se deberá actualizar</value>

        public int codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }

        /// <summary>
        /// Propiedad operacion </summary>
        /// <value>
        /// Valores posibles: "+" o "-". Indica si el totalizador se incrementará
        /// o decrementará una vez asociado a una transacción</value>

        public string operacion
        {
            get { return _operacion; }
            set { _operacion = value; }
        }

        /// <summary>
        /// Propiedad operacion </summary>
        /// <value>
        /// Número de moneda utilizada para la actualización del totalizador.
        /// Valores posibles 1 o 2 (primer o segunda moneda asociada a la transacción)</value>

        public int moneda
        {
            get { return _moneda; }
            set { _moneda = value; }
        }
    }

    /// <summary>
    /// Clase TrxInfo
    /// <remarks>
    /// Reune toda la información correspondiente a una transacción. 
    /// Es la base de la ejecución de transacciones
    /// </remarks>
    /// <seealso cref="TotalizadorOperacion">
    /// Ver información de la operación de totalizadores asociados a una transacción. </seealso>
    /// 
    public class TrxInfo
    {
        private int _codigo;
        private int _subcodigo;
        private string _descripcion;
        private int _tipo;
        private List<int> _monedas;
        private List<TotalizadorOperacion> _TotalizadorOp;

        /// <summary>
        /// Constructor de la clase.</summary>
        /// <param name="nCodigo"> Código de la transacción</param>
        /// <param name="nSubCodigo"> SubCódigo de la transacción.
        /// Junto al código de transacción la identifican una transacción de forma inequívoca</param>
        /// <param name="sDescripcion"> Descripción de la transacción</param>
        /// <param name="nTipoTrx"> Tipo de transacción.
        /// Valores posibles:
        /// 1: Compra de moneda extranjera
        /// 2: Venta de moneda extranjera
        /// 3: Transferencias de monedas al y desde el tesoro
        /// 4: Venta de productos
        /// </param>
        /// 
        public TrxInfo(int nCodigo, int nSubCodigo, string sDescripcion, int nTipoTrx)
        {
            _codigo = nCodigo;
            _subcodigo = nSubCodigo;
            _descripcion = sDescripcion;
            _tipo = nTipoTrx;

            _monedas = new List<int>();
            _TotalizadorOp = new List<TotalizadorOperacion>();
        }

        /// <summary>
        /// Propiedad codigo</summary>
        /// <value>
        /// Número identificador del tipo de transacción. 
        /// </value>
        public int codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }

        /// <summary>
        /// Propiedad subcodigo</summary>
        /// <value>
        /// Subcódigo de la transacción.
        /// Junto al código, referencian una transacción de forma inequívoca. También puede indicar comportamiento
        /// En el caso de las transacciones tipo 3 (transferencias al tesoro) puede significar:
        /// Subcódigo 1 Recepción de pases del tesoro
        /// Subcódigo 2 Envío de pases al tesoro
        /// </value>

        public int subcodigo
        {
            get { return _subcodigo; }
            set { _subcodigo = value; }
        }

        /// <summary>
        /// descripcion</summary>
        /// <value>
        /// Descripción de la transacción
        /// </value>

        public string descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }
    
        /// <summary>
        /// tipoTrx</summary>
        /// <value>
        /// Tipo de transacción.
        /// Valores posibles:
        /// 1: Compra de moneda extranjera
        /// 2: Venta de moneda extranjera
        /// 3: Transferencias de monedas al y desde el tesoro
        /// 4: Venta de productos
        /// </value>

        public int tipoTrx
        {
            get { return _tipo; }
            set { _tipo = value; }
        }

        /// <summary>
        /// listaMonedas</summary>
        /// <value>
        /// Lista de dos elementos. Cada elemento significa la moneda asociada a la transacción.
        /// Según el tipo de transacción puede tener distintos significados.
        /// Compra de moneda extranjera
        ///     Moneda 1: Moneda que se adquiere
        ///     Moneda 2: Moneda que se entrega
        /// Venta de moneda extranjera    
        ///     Moneda 1: Moneda que se adquiere
        ///     Moneda 2: Moneda que se entrega
        /// Transferencias de monedas al y desde el tesoro
        ///     Moneda 1: Moneda de la transacción
        ///     Moneda 2: Sin uso
        /// Venta de productos
        ///     Moneda 1: Moneda de la transacción
        ///     Moneda 2: Sin uso
        /// </value>

        public List<int> listaMonedas
        {
            get { return _monedas; }
        }

        /// <summary>
        /// listaTotalizadorOp</summary>
        /// <value>
        /// Lista de las operaciones a realizarse sobre totalizadores
        /// </value>
        /// <seealso cref="TotalizadorOperacion">
        /// Ver información de la operación de totalizadores asociados a una transacción. </seealso>
        public List<TotalizadorOperacion> listaTotalizadorOp
        {
            get { return _TotalizadorOp; }
        }
    }

    [Serializable]
    /// <summary>
    /// Clase CajaOpInfo
    /// <remarks>
    /// Reune toda la información correspondiente a las transacciones que puede realizar una caja
    /// y de los totalizadores que utiliza el sistema
    /// Es la clase desde donde se ejecutan las transacciones
    /// </remarks>
    /// <seealso cref="TotalizadorInfo">
    /// Información de totalizadores. </seealso>
    /// <seealso cref="TrxInfo">
    /// Información de Transacción. </seealso>
    /// 
    public class CajaOpInfo
    {
        private List<TotalizadorInfo> _listaTotalizadores;
        private List<TrxInfo> _listaTransacciones;

        /// <summary>
        /// Constructor de la clase.</summary>
        /// <param name="archivoXMLConfiguracion"> Ubicación del archivo xml de configuración de la operatoria de caja</param>

        public CajaOpInfo(string archivoXMLConfiguracion)
        {
            if (File.Exists(archivoXMLConfiguracion))
            {
                _listaTotalizadores= new List<TotalizadorInfo>() ;
                _listaTransacciones = new List<TrxInfo>() ;

                XmlDocument xDoc = new XmlDocument();

                xDoc.Load(archivoXMLConfiguracion);  
                //Primero el nodo principal.
                XmlNodeList m_Totalizadores = ((XmlElement)xDoc.GetElementsByTagName("TrxManager")[0]).GetElementsByTagName("Totalizadores"); ;

                //Ahora Todos los totalizadores
                XmlNodeList lista = ((XmlElement)m_Totalizadores[0]).GetElementsByTagName("Totalizador"); 
                foreach (XmlElement nodo in lista)
                {
                    _listaTotalizadores.Add(new TotalizadorInfo(Convert.ToInt32(nodo.GetAttribute("Codigo")),
                                                                nodo.GetAttribute("Descripcion")));

                }

                //Primero el nodo principal.
                XmlNodeList m_Aux;
                XmlNodeList m_Transacciones = ((XmlElement)xDoc.GetElementsByTagName("TrxManager")[0]).GetElementsByTagName("Transacciones"); 
                TrxInfo unaTrxInfo;
                //Ahora Todos los totalizadores
                lista = ((XmlElement)m_Transacciones[0]).GetElementsByTagName("Transaccion"); 
                foreach (XmlElement nodo in lista)
                {
                    _listaTransacciones.Add(new TrxInfo(Convert.ToInt32(nodo.GetAttribute("Codigo")),
                                                        Convert.ToInt32(nodo.GetAttribute("Subcodigo")),
                                                        nodo.GetAttribute("Descripcion"),
                                                        Convert.ToInt32(nodo.GetAttribute("TipoTrx"))));
                    unaTrxInfo = _listaTransacciones[_listaTransacciones.Count-1];
                    
                    m_Aux = nodo.GetElementsByTagName("Moneda");
                    
                    unaTrxInfo.listaMonedas.Add(Convert.ToInt32(((XmlElement)m_Aux[0]).GetAttribute("Moneda1")));
                    unaTrxInfo.listaMonedas.Add(Convert.ToInt32(((XmlElement)m_Aux[0]).GetAttribute("Moneda2")));

                    m_Aux = ((XmlElement)nodo.GetElementsByTagName("Totalizadores")[0]).GetElementsByTagName("Totalizador");
                    foreach (XmlElement nodoTot in m_Aux)
                    {
                        unaTrxInfo.listaTotalizadorOp.Add(new TotalizadorOperacion(Convert.ToInt32(nodoTot.GetAttribute("Codigo")),
                                                                                    nodoTot.GetAttribute("Operacion"),
                                                                                    Convert.ToInt32(nodoTot.GetAttribute("Moneda"))));
                    }
                }
            }
            else
                throw new ArchivoConfiguracionInexistenteException();
        }

        /// <summary>
        /// totalizadores</summary>
        /// <value>
        /// Lista de los totalizadores disponibles para la operatoria de caja
        /// </value>
        /// <seealso cref="TotalizadorInfo">
        /// Información descriptiva del totalizador. </seealso>

        public List<TotalizadorInfo> totalizadores
        {
            get { return _listaTotalizadores; }
        }

        /// <summary>
        /// transacciones</summary>
        /// <value>
        /// Lista de los totalizadores disponibles para la operatoria de caja
        /// </value>
        /// <seealso cref="TotalizadorInfo">
        /// Información descriptiva del totalizador. </seealso>

        public List<TrxInfo> transacciones
        {
            get { return _listaTransacciones; }
        }

        /// <summary>
        /// Busca y devuelve la información de la transacción correspondiente a los parámetros recibidos. </summary>
        /// <param name="codigo"> Código de transacción</param>
        /// <param name="subcodigo"> Subcódigo de transacción</param>
        /// <returns>
        /// Clase TrxInfo asociada al código y subcódigo recibido por parámetros.</returns>
        /// <seealso cref="TrxInfo">
        /// Descripción de una transacción </seealso>    
        public TrxInfo obtenerTrxInfo(int codigo, int subcodigo)
        {
            TrxInfo retVal = null;

            foreach (TrxInfo ti in _listaTransacciones)
            {
                if (ti.codigo == codigo && ti.subcodigo == subcodigo)
                {
                    retVal = ti;
                    break;
                }
            }

            if (retVal == null)
            { 
                throw new TransaccionInexistenteException();
            }

            return retVal;
        }
    }
}
