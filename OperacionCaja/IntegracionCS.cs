using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OperacionCaja;
using OperacionCaja.ServiceReferenceCotizacion;
using OperacionCaja.ServiceReferenceSM;
using OperacionCaja.ServiceReferenceProducto;

namespace OperacionCaja
{
    class IntegracionCS: ISCMoneda, ISCProducto
    {
        private SesionManagerClient unSM = new SesionManagerClient();
        private CotizacionClient _unCliCot = new CotizacionClient();
        private ProductoClient _unCliProducto = new ProductoClient();

        private string _idSesionCotizacion;

        public IntegracionCS()
        {
            _idSesionCotizacion = unSM.obtenerIDSesionCotizacion(System.Environment.MachineName);
        }

        ~IntegracionCS()  // destructor
        {
            // cleanup statements...
            unSM.cerrarSesionCotizacion(_idSesionCotizacion);
        }
        public string idSesionCotizacion
        {
            get { return _idSesionCotizacion; }
        }

        public CotizacionClient wsCotizacion
        {
            get { return _unCliCot; }
        }

        #region Miembros de ISCMoneda

        public double obtenerCotizacion(int moneda, TipoCotizacion tipo)
        {
            TWSCotizacion unaCotizacion = _unCliCot.obtenerCotizacion(_idSesionCotizacion, moneda);
            double ret = 0;
            if (tipo == TipoCotizacion.Compra)
                ret = unaCotizacion.Compra;
            else
                ret = unaCotizacion.Venta;
            return ret;
        }

        public string obtenerDescripcion(int moneda)
        {
            TWSCotizacion unaCotizacion = _unCliCot.obtenerCotizacion(_idSesionCotizacion, moneda);
            return unaCotizacion.Descripcion;
        }

        public InfoMoneda obtenerInfoMoneda(int moneda)
        {
            TWSCotizacion unaCotizacion = _unCliCot.obtenerCotizacion(_idSesionCotizacion, moneda);
            InfoMoneda ret = new InfoMoneda(unaCotizacion.Codigo, unaCotizacion.Descripcion, unaCotizacion.Compra, unaCotizacion.Venta);
            return ret;
        }

        #endregion

        public InfoMoneda[] obtenerTodasLasMonedas()
        {
            TWSCotizacion[] cotizaciones = _unCliCot.obtenerCotizaciones(idSesionCotizacion);
            InfoMoneda[] ret = new InfoMoneda[cotizaciones.Length];
            int pos = 0;
            foreach (TWSCotizacion unaCotizacion in cotizaciones)
            {
                ret[pos] = new InfoMoneda(unaCotizacion.Codigo, unaCotizacion.Descripcion, unaCotizacion.Compra, unaCotizacion.Venta);
                pos++;
            }
            return ret;
        }

        #region Miembros de ISCProducto

        public double obtenerPrecio(int codigo)
        {
            TWSProducto unProducto = _unCliProducto.obtenerProducto(_idSesionCotizacion, codigo);
            return unProducto.Precio;
        }

        public InfoProducto obtenerInfoProducto(int codigo)
        {
            TWSProducto unProducto = _unCliProducto.obtenerProducto(_idSesionCotizacion, codigo);
            InfoProducto ret = new InfoProducto(unProducto.Codigo, unProducto.Nombre, unProducto.Precio);
            return ret;
        }
        #endregion
    }
}
