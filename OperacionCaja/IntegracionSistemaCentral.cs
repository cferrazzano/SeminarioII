using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperacionCaja
{
    /// <summary>
    /// Clase InfoProducto. Clase interna para el manejo de integración con sistemas centrales
    /// </summary>
    /// <remarks>
    /// Clase interna para el manejo de integración con sistemas centrales.
    /// Provee la información de un producto en particular
    /// </remarks>
    /// 
    public class InfoProducto
    {
        private int _codigo;
        private string _descripcion;
        private double _precio;

        /// <summary>
        /// Método contructor de InfoProducto
        /// </summary>
        /// <param name="Codigo">Código identificador del producto</param>
        /// <param name="Descripcion">Descripción del producto</param>
        /// <param name="Precio">Precio del producto</param>
        /// 
        public InfoProducto(int Codigo, string Descripcion, double Precio)
        {
            this._codigo = Codigo;
            this._descripcion = Descripcion;
            this._precio = Precio;
        }

        public int codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }

        public string descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        public double compra
        {
            get { return _precio; }
            set { _precio = value; }
        }
    }

    /// <summary>
    /// Clase InfoMoneda. Clase interna para el manejo de integración con sistemas centrales
    /// </summary>
    /// <remarks>
    /// Clase interna para el manejo de integración con sistemas centrales.
    /// Provee la información de una moneda en particular
    /// </remarks>
    /// 
    public class InfoMoneda
    {
        private int _codigo;
        private string _descripcion;
        private double _compra;
        private double _venta;

        /// <summary>
        /// Método contructor de InfoMoneda
        /// </summary>
        /// <param name="Codigo">Código identificador de la moneda</param>
        /// <param name="Descripcion">Descripción de la moneda</param>
        /// <param name="Compra">Cotización de Compra de la moneda</param>
        /// <param name="Venta">Cotización de Venta de la moneda</param>
        /// 

        public InfoMoneda(int Codigo, string Descripcion, double Compra, double Venta)
        {
            this._codigo = Codigo;
            this._descripcion = Descripcion;
            this._compra = Compra;
            this._venta = Venta;
        }

        public int codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }

        public string descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        public double compra
        {
            get { return _compra; }
            set { _compra = value; }
        }

        public double venta
        {
            get { return _venta; }
            set { _venta = value; }
        }
    }

    public enum TipoCotizacion { Compra, Venta };

    /// <summary>
    /// Interfaz para la integración con sistemas centrales
    /// </summary>
    public interface ISCMoneda
    {
        /// <summary>
        /// Obtiene la cotización de una moneda de acuerdo a un tipo en particular
        /// </summary>
        /// <param name="moneda">Código identificador de la moneda</param>
        /// <param name="tipo">Tipo de cotización (Compra o Venta)</param>
        double obtenerCotizacion(int moneda, TipoCotizacion tipo);
        /// <summary>
        /// Obtiene la descripción de una moneda  
        /// </summary>
        /// <param name="moneda">Código identificador de la moneda</param>
        string obtenerDescripcion(int moneda);
        /// <summary>
        /// Obtiene toda la información concerniente a una moneda en particular
        /// </summary>
        /// <param name="moneda">Código identificador de la moneda</param>
        /// <seealso cref="InfoMoneda"/>
        InfoMoneda obtenerInfoMoneda(int moneda);
    }

    /// <summary>
    /// Interfaz para la integración con sistemas centrales
    /// </summary>

    public interface ISCProducto
    {
        /// <summary>
        /// Obtiene el precio de un artículo en particular
        /// </summary>
        /// <param name="codigo">Código identificador del producto</param>
        double obtenerPrecio(int codigo);
        /// <summary>
        /// Obtiene la descripción de un artículo en particular
        /// </summary>
        /// <param name="codigo">Código identificador del producto</param>
        string obtenerDescripcion(int codigo);
        /// <summary>
        /// Obtiene toda la información concerniente a un producto en particular
        /// </summary>
        /// <param name="codigo">Código identificador del producto</param>
        /// <seealso cref="InfoProducto"/>
        InfoProducto obtenerInfoProducto(int codigo);
    }
}
