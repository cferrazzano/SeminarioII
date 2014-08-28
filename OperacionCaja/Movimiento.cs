using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperacionCaja
{
    public interface IMovimiento
    {
        /// <summary>
        /// Propiedad operacion</summary>
        /// <value>
        /// Número de operación del movimiento. Los números de operación son
        /// consecutivos durante todo un día de trabajo
        /// </value>

        int operacion {get; set;}

        /// <summary>
        /// Propiedad codigo</summary>
        /// <value>
        /// Código de transacción de la operación realizada
        /// </value>
        int codigo {get; set;}

        /// <summary>
        /// Propiedad subcodigo</summary>
        /// <value>
        /// Subcódigo de transacción de la operación realizada
        /// </value>
        int subcodigo {get; set;}

        /// <summary>
        /// Propiedad idMoneda1</summary>
        /// <value>
        /// Moneda principal de la transacción
        /// </value>
        int idMoneda1 { get; set; }

        /// <summary>
        /// Propiedad importe1</summary>
        /// <value>
        /// importe asociado a la moneda principal de la transacción
        /// </value>
        double importe1 {get; set;}

        /// <summary>
        /// Propiedad idMoneda2</summary>
        /// <value>
        /// Moneda secundaria de la transacción
        /// </value>
        int idMoneda2 {get; set;}

        /// <summary>
        /// Propiedad importe2</summary>
        /// <value>
        /// importe asociado a la moneda secundaria de la transacción
        /// </value>
        double importe2 {get; set;}

        /// <summary>
        /// Propiedad cotizacion</summary>
        /// <value>
        /// Cotización utilizada para convertir importe1 en importe2. Sólo cuando aplique.
        /// </value>

        double cotizacion {get; set;}

        /// <summary>
        /// Propiedad fecha</summary>
        /// <value>
        /// Fecha contable actual del momento de la realización de la operación
        /// </value>

        DateTime fecha {get; set;}

        /// <summary>
        /// Propiedad referencia</summary>
        /// <value>
        /// Referencia (número o letras) asociada a la transacción. Es otro modo de identificarla. 
        /// Puede no ser único.
        /// </value>

        string referencia {get; set;}

        /// <summary>
        /// Propiedad estado</summary>
        /// <value>
        /// Estado de la operación.
        /// Valores posibles:
        /// 1: Operación correcta
        /// 0: Operación anulada
        /// </value>
        int estado {get; set;}

        /// <summary>
        /// Propiedad descripcion</summary>
        /// <value>
        /// Descripción asociada a la transacción
        /// </value>

        string descripcion {get; set;}

        /// <summary>
        /// Propiedad articulos</summary>
        /// <value>
        /// Colección de artículos asociados a la transacción de venta
        /// </value>

        ColeccionArticulo articulos {get; set;}

        // Métodos
        /// <summary>
        /// Permite anular una transacción
        /// </summary>
        bool anular();
    }
    /// <summary>
    /// Clase Movimiento para la información ejecución de transacciones.</summary>
    /// <remarks>
    /// Esta clase brinda la información de un movimiento en particular
    /// </remarks>

    [Serializable]
    public class Movimiento : IMovimiento
    {
        //número de operacion
        private int _operacion;

        //Código de transacción
        private int _codigo;

        //Subcódigo de transacción
        private int _subcodigo;

        //Moneda principal de la operación
        private int _idMoneda1;

        //Importe en moneda principal
        private double _importe1;

        //Moneda secundaria de la operación
        private int _idMoneda2;

        //Importe1 expresado en moneda 2
        private double _importe2;

        //Cotización utilizada en la conversión
        private double _cotizacion;

        //Fecha de la operación
        private DateTime _fecha;

        //Referencia externa de la operación
        private string _referencia;

        //Estado 1-OK, 0-Anulado
        private int _estado;

        //Descripción externa de la operación
        private string _descripcion;

        //Artículos asociados a la venta
        private ColeccionArticulo _articulos;

        /// <summary>
        /// Método constructor de la clase
        /// </summary>
        public Movimiento()
        {
            _articulos = null;
        }

        /*
         * Propiedades
         */

        /// <summary>
        /// Propiedad operacion</summary>
        /// <value>
        /// Número de operación del movimiento. Los números de operación son
        /// consecutivos durante todo un día de trabajo
        /// </value>

        public int operacion
        {
            get { return _operacion; }
            set { _operacion = value; }
        }

        /// <summary>
        /// Propiedad codigo</summary>
        /// <value>
        /// Código de transacción de la operación realizada
        /// </value>
        public int codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }

        /// <summary>
        /// Propiedad subcodigo</summary>
        /// <value>
        /// Subcódigo de transacción de la operación realizada
        /// </value>
        public int subcodigo
        {
            get { return _subcodigo; }
            set { _subcodigo = value; }
        }

        /// <summary>
        /// Propiedad idMoneda1</summary>
        /// <value>
        /// Moneda principal de la transacción
        /// </value>
        public int idMoneda1
        {
            get { return _idMoneda1; }
            set { _idMoneda1 = value; }
        }

        /// <summary>
        /// Propiedad importe1</summary>
        /// <value>
        /// importe asociado a la moneda principal de la transacción
        /// </value>
        public double importe1
        {
            get { return _importe1; }
            set { _importe1 = value; }
        }

        /// <summary>
        /// Propiedad idMoneda2</summary>
        /// <value>
        /// Moneda secundaria de la transacción
        /// </value>
        public int idMoneda2
        {
            get { return _idMoneda2; }
            set { _idMoneda2 = value; }
        }

        /// <summary>
        /// Propiedad importe2</summary>
        /// <value>
        /// importe asociado a la moneda secundaria de la transacción
        /// </value>
        public double importe2
        {
            get { return _importe2; }
            set { _importe2 = value; }
        }

        /// <summary>
        /// Propiedad cotizacion</summary>
        /// <value>
        /// Cotización utilizada para convertir importe1 en importe2. Sólo cuando aplique.
        /// </value>

        public double cotizacion
        {
            get { return _cotizacion; }
            set { _cotizacion = value; }
        }

        /// <summary>
        /// Propiedad fecha</summary>
        /// <value>
        /// Fecha contable actual del momento de la realización de la operación
        /// </value>

        public DateTime fecha
        {
            get { return _fecha; }
            set { _fecha = value; }
        }

        /// <summary>
        /// Propiedad referencia</summary>
        /// <value>
        /// Referencia (número o letras) asociada a la transacción. Es otro modo de identificarla. 
        /// Puede no ser único.
        /// </value>

        public string referencia
        {
            get { return _referencia; }
            set { _referencia = value; }
        }

        /// <summary>
        /// Propiedad estado</summary>
        /// <value>
        /// Estado de la operación.
        /// Valores posibles:
        /// 1: Operación correcta
        /// 0: Operación anulada
        /// </value>
        public int estado
        {
            get { return _estado; }
            set { _estado = value; }
        }

        /// <summary>
        /// Propiedad descripcion</summary>
        /// <value>
        /// Descripción asociada a la transacción
        /// </value>

        public string descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        /// <summary>
        /// Propiedad articulos</summary>
        /// <value>
        /// Colección de artículos asociados a la transacción de venta
        /// </value>

        public ColeccionArticulo articulos
        {
            get { return _articulos; }
            set { _articulos = value; }
        }

        // Métodos
        /// <summary>
        /// Permite anular una transacción
        /// </summary>
        public bool anular()
        {
            if (_estado == 1)
                _estado = 0;
            else
                throw new TransaccionYaAnuladaException();

            return true;
        }
    }
}
