using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperacionCaja
{
    public interface IArticulo
    {
        int codigo {get; set;}
        string descripcion {get; set;}
        double cantidad {get; set;}
        double precio {get; set;}
    }

    /// <summary>
    /// Clase Articulo para la ejecución de transacciones de ventas.</summary>
    /// <remarks>
    /// Esta clase es la unidad de venta. Todas las ventas a minoristas o mayoristas será
    /// realizada mediante el uso de artículos.
    /// La información de cada artículo debe ser tomada (en lo posible) del servicio IProducto
    /// </remarks>
    [Serializable]
    public class Articulo : IArticulo
    {
        private int _codigo;
        private string _descripcion;
        private double _cantidad;
        private double _precio;

        /// <summary>
        /// Constructor de la clase.</summary>
        /// <param name="nCodigo"> Código de artículo</param>
        /// <param name="sDescripcion"> Descripción del artículo</param>
        /// <param name="dCant"> Cantidad vendida del artículo en la venta presente</param>
        /// <param name="dPrecio"> Precio unitario del artículo vendido</param>
        /// 
        public Articulo(int nCodigo, string sDescripcion, double dCant, double dPrecio)
        {
            _codigo = nCodigo;
            _descripcion = sDescripcion;
            _cantidad = dCant;
            _precio = dPrecio;
        }

        public Articulo(int nCodigo, string sDescripcion, double dCant)
        {
            _codigo = nCodigo;
            _descripcion = sDescripcion;
            _cantidad = dCant;
            _precio = 0;
        }

        /// <summary>
        /// Propiedad codigo</summary>
        /// <value>
        /// Número identificador del artículo. Debe ser único. Sirve para referenciar el artículo y 
        /// actualizar stock</value>
        public int codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }

        /// <summary>
        /// Propiedad descripcion </summary>
        /// <value>
        /// Descripción del artículo</value>
        public string descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        /// <summary>
        /// Propiedad cantidad </summary>
        /// <value>
        /// Cantidad vendida del artículo en esta venta</value>

        public double cantidad
        {
            get { return _cantidad; }
            set { _cantidad = value; }
        }
        /// <summary>
        /// Propiedad precio </summary>
        /// <value>
        /// Precio unitario del artículo para la venta presente</value>

        public double precio
        {
            get { return _precio; }
            set { _precio = value; }
        }
    }
}
