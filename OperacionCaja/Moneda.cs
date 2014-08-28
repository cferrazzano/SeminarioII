using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperacionCaja
{
    public interface IMoneda
    {
        void inicializar();
        int codigo { get; set;}
        string descripcion { get; set;}
        double saldoInicial { get; set;}
        double entradas { get; set;}
        double salidas { get; set;}
        int cantEntradas { get;}
        int cantSalidas { get;}
        double saldo { get;}
        double ingreso(double importe);
        double extraccion(double importe);
    }

    /// <summary>
    /// Clase Moneda para la información existencia de monedas.</summary>
    /// <remarks>
    /// Esta clase brinda la información del movimiento que tiene una moneda en particular
    /// Controla la existencia, así como las entradas y salidas, cantidad de transacciones asociadas, etc.
    /// La información de creación de esta moneda debería obtenerse (de ser posible) a partir del servicio ICotizacion
    /// </remarks>
    [Serializable]
    public class Moneda: IMoneda
    {
        private int     _codigo;
        private string  _descripcion;

        private double _saldoInicial;
        private double _entradas;
        private int _cantEntradas;

        private double _salidas;
        private int _cantSalidas;


        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codigo">Código de moneda</param>
        /// <param name="descripcion">Descripción de la moneda</param>
        public Moneda(int codigo, string descripcion)
        {
            _codigo = codigo;
            _descripcion = descripcion;

            inicializar();
        }
        /// <summary>
        /// Inicializa todas las propiedades de la moneda con cero
        /// </summary>
        public void inicializar()
        {
            _saldoInicial = 0;
            _entradas = 0;
            _cantEntradas = 0;

            _salidas = 0;
            _cantSalidas = 0;
        }

        //Propiedades
        /// <summary>
        /// Propiedad codigo</summary>
        /// <value>
        /// Número identificador de la moneda. Debe ser única. Sirve para referenciar la moneda
        /// </value>
        public int codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }

        //Propiedades
        /// <summary>
        /// Propiedad codigo</summary>
        /// <value>
        /// Número identificador de la moneda. Debe ser única. Sirve para referenciar la moneda
        /// </value>
        public string descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        /// <summary>
        /// Propiedad saldoInicial</summary>
        /// <value>
        /// Saldo con el que comienza la moneda al inicio de operaciones
        /// </value>
        public double saldoInicial
        {
            get { return _saldoInicial; }
            set { _saldoInicial = value; }
        }

        /// <summary>
        /// Propiedad entradas</summary>
        /// <value>
        /// Importe total de entradas para la moneda
        /// </value>
        public double entradas
        {
            get { return _entradas; }
            set { _entradas = value; }
        }

        /// <summary>
        /// Propiedad salidas</summary>
        /// <value>
        /// Importe total de salidas para la moneda
        /// </value>
        public double salidas
        {
            get { return _salidas; }
            set { _salidas = value; }
        }

        /// <summary>
        /// Propiedad cantEntradas</summary>
        /// <value>
        /// Cantidad total de entradas para la moneda
        /// </value>
        public int cantEntradas
        {
            get { return _cantEntradas; }
        }

        /// <summary>
        /// Propiedad cantSalidas</summary>
        /// <value>
        /// Cantidad total de salidas para la moneda
        /// </value>
        public int cantSalidas
        {
            get { return _cantSalidas; }
        }

        /// <summary>
        /// Propiedad saldo</summary>
        /// <value>
        /// Saldo actual de las moneda
        /// </value>

        public double saldo
        {
            get { return _saldoInicial + _entradas - _salidas; }
        }

        /// <summary>
        /// Método para el ingreso de efectivo en la moneda
        /// </summary>
        /// <param name="importe">Importe del ingreso</param>
        /// <returns>Saldo actual de la moneda</returns>
        /// 
        public double ingreso(double importe)
        {
            if (saldo + importe < 0)
                throw new SaldoNegativoException();
            _entradas += importe;

            if (importe > 0)
                _cantEntradas++;
            else
                _cantEntradas--;

            return saldo;
        }

        /// <summary>
        /// Método para la extracción de efectivo en la moneda
        /// </summary>
        /// <param name="importe">Importe de la extracción</param>
        /// <returns>Saldo actual de la moneda</returns>
        /// 
        public double extraccion(double importe)
        {
            if (saldo - importe < 0)
                throw new SaldoNegativoException();
            _salidas += importe;

            if (importe > 0)
                _cantSalidas++;
            else
                _cantSalidas--;

            return saldo;
        }
    }
}
