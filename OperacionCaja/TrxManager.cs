using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperacionCaja
{
    /// <summary>
    /// Clase para administrar el funcionamiento de la caja. Controla la ejecución y anulación de transacciones.</summary>
    /// <remarks>
    /// Esta clase permite administrar las monedas, totalizadores y movimientos de una caja.
    /// Conoce las transacciones que existen y controla su ejecución
    /// </remarks>
    /// <seealso cref="CajaOpInfo"/>
    /// <seealso cref="ColeccionMoneda"/>
    /// <seealso cref="ColeccionTotalizador"/>
    /// <seealso cref="ColeccionMovimiento"/>
    /// 
    public class TrxManager
    {
        public const int CompraMonex = 1;
        public const int VentaMonex = 2;
        public const int PasesTesoro = 3;
        public const int VentaProductos = 4;

        public const int RecepcionTesoro = 1;
        public const int EnvioTesoro = 2;

        private CajaOpInfo _infoOperaciones;
        private ColeccionMoneda _monedas;
        private ColeccionTotalizador _totalizadores;
        private ColeccionMovimiento _movimientos;
        ISCMoneda _integracionMoneda;
        ISCProducto _integracionProducto;

        /// <summary>
        /// Método constructor de la clase
        /// </summary>
        /// <param name="archivoXMLConfiguracion">Archivo de configuración de transacciones y totalizadores</param>
        /// <param name="integracionMoneda">Clase que implementa la interfaz ISCMoneda</param>
        /// <param name="integracionProducto">Clase que implementa la interfaz ISCProducto</param>
        /// <seealso cref="CajaOpInfo"/>
        /// <seealso cref="ISCMoneda"/>
        /// <seealso cref="ISCProducto"/>
        /// 
        public TrxManager(string archivoXMLConfiguracion, ISCMoneda integracionMoneda, ISCProducto integracionProducto)
        {
            _infoOperaciones = new CajaOpInfo(archivoXMLConfiguracion);
            _monedas = new ColeccionMoneda();
            _totalizadores = new ColeccionTotalizador();
            _movimientos = new ColeccionMovimiento();
            _integracionMoneda = integracionMoneda;
            _integracionProducto = integracionProducto;
        }

        /// <summary>
        /// Propiedad infoTransaccion</summary>
        /// <value>
        /// Información de todos los totalizadores y transacciones configurados
        /// </value>
        /// <seealso cref="CajaOpInfo"/>

        public CajaOpInfo infoTransaccion
        {
            get { return _infoOperaciones; }
        }

        /// <summary>
        /// Propiedad monedas</summary>
        /// <value>
        /// Colección de monedas asociadas a la operatoria de caja
        /// </value>
        /// <seealso cref="ColeccionMoneda"/>
        public ColeccionMoneda monedas
        {
            get {return _monedas;}
        }

        /// <summary>
        /// Propiedad totalizadores</summary>
        /// <value>
        /// Colección de totalizadores asociados a la operatoria de caja
        /// </value>
        /// <seealso cref="ColeccionTotalizador"/>
        public ColeccionTotalizador totalizadores
        {
            get { return _totalizadores; }
        }

        /// <summary>
        /// Propiedad movimientos</summary>
        /// <value>
        /// Colección de movimientos asociados a la operatoria de caja
        /// </value>
        /// <seealso cref="ColeccionMovimiento"/>
        /// 
        public ColeccionMovimiento movimientos
        {
            get { return _movimientos; }
        }

        /// <summary>
        /// Ejecuta la transacción definida en el movimiento recibido por parámetro
        /// Para todas aquellas transacciones donde alguna de las dos monedas involucradas sean distintas a
        /// pesos argentinos, el método calculará el importe2 de la transacción tomando la cotización
        /// desde los sistemas centrales. Este proceso puede anularse al cargar explícitamente la cotización
        /// utilizada al momento de la transacción
        /// 
        /// Los sistemas centrales también se invocan cuando hay transacciones con artículos involucradas
        /// Estas transacciones deben incluir artículo con precio, en caso contrario, el sistema central será 
        /// invocado para establecer el precio de venta
        /// </summary>
        /// <param name="unMovimiento">Clase Movimiento que moldea la transacción</param>
        /// <returns>Número de operación asociada</returns>
        public int ejecutarTransaccion(Movimiento unMovimiento)
        {
            TrxInfo unaTrx = _infoOperaciones.obtenerTrxInfo(unMovimiento.codigo, unMovimiento.subcodigo);

            if ((unMovimiento.idMoneda1 != unaTrx.listaMonedas[0])||
                (unMovimiento.idMoneda2 != unaTrx.listaMonedas[1]))
                throw new MonedaMovimientoDifereConfiguracion();

            if ((unMovimiento.idMoneda1 != MonedaConst.pesos_argentinos || 
                unMovimiento.idMoneda2 != MonedaConst.pesos_argentinos) && unMovimiento.cotizacion == 0)
            {
                if (unMovimiento.codigo == 1)
                {
                    unMovimiento.cotizacion = _integracionMoneda.obtenerCotizacion(unMovimiento.idMoneda1, TipoCotizacion.Compra);
                    unMovimiento.importe2 = unMovimiento.importe1 * unMovimiento.cotizacion;
                }
                else
                {
                    unMovimiento.cotizacion = _integracionMoneda.obtenerCotizacion(unMovimiento.idMoneda1, TipoCotizacion.Venta);
                    unMovimiento.importe2 = unMovimiento.importe1 / unMovimiento.cotizacion;
                }
            }

            IMoneda unaMoneda1 = _monedas.obtenerMonedaPorCodigo(unMovimiento.idMoneda1);
            IMoneda unaMoneda2 = _monedas.obtenerMonedaPorCodigo(unMovimiento.idMoneda2);

            switch (unaTrx.tipoTrx)
            { 
                case CompraMonex:

                    if (unMovimiento.importe1 != (int)unMovimiento.importe1)
                        throw new ImporteInvalidoException();

                    unaMoneda1.ingreso(unMovimiento.importe1);
                    unaMoneda2.extraccion(unMovimiento.importe2);

                    break;
                case VentaMonex:
                    if (unMovimiento.importe1 != (int)unMovimiento.importe1)
                        throw new ImporteInvalidoException();

                    unaMoneda1.extraccion(unMovimiento.importe1);
                    unaMoneda2.ingreso(unMovimiento.importe2);
                    break;
                case PasesTesoro:
                    if (unMovimiento.subcodigo == RecepcionTesoro)
                        unaMoneda1.ingreso(unMovimiento.importe1);
                    else
                        unaMoneda1.extraccion(unMovimiento.importe1);

                    break;

                case VentaProductos:
                    double importeTotal=0;
                    foreach (Articulo unArticulo in unMovimiento.articulos)
                    {
                        if (unArticulo.precio == 0)
                            unArticulo.precio = _integracionProducto.obtenerPrecio(unArticulo.codigo);
                        importeTotal += unArticulo.precio * unArticulo.cantidad;
                    }

                    if (unMovimiento.subcodigo == 10)
                    { 
                        //La venta es mayorista. Descuento automático del 15% o del 20% si supera los 20000 $
                        if (importeTotal > 20000)
                            importeTotal = importeTotal - (importeTotal * 0.20);
                        else
                            importeTotal = importeTotal - (importeTotal * 0.15);
                    }
                        
                    if (unMovimiento.importe1 != importeTotal)
                        throw new ImporteInvalidoException();

                    unaMoneda1.ingreso(importeTotal);

                    break;
            }

            ITotalizador unTotalizador;
            foreach (TotalizadorOperacion tot in unaTrx.listaTotalizadorOp)
            {
                unTotalizador = _totalizadores.obtenerTotalizadorPorCodigo(tot.codigo);


                if (tot.operacion.Equals("+"))
                {
                    if (tot.moneda == 1)
                        unTotalizador.incrementar(unMovimiento.importe1);
                    else
                        unTotalizador.incrementar(unMovimiento.importe2);
                }
                else
                {
                    if (tot.moneda == 1)
                        unTotalizador.incrementar(-unMovimiento.importe1);
                    else
                        unTotalizador.incrementar(-unMovimiento.importe2);

                }
            }

            unMovimiento.fecha = DateTime.Today;
            unMovimiento.estado = 1;

            _movimientos.agregar(unMovimiento);
            unMovimiento.operacion = _movimientos.todosLosMovimientos.Count;

            return unMovimiento.operacion;
        }

        /// <summary>
        /// Anula la transacción correspondiente al número de operación recibido por parámetro
        /// </summary>
        /// <param name="operacion">número de operación</param>
        /// <returns>Verdadero si pudo anular la transacción</returns>
        public bool anularTransaccion(int operacion)
        {
            IMovimiento unMovimiento = movimientos.obtenerMovimientoPorOperacion(operacion);
            TrxInfo unaTrx = _infoOperaciones.obtenerTrxInfo(unMovimiento.codigo, unMovimiento.subcodigo);

            if ((unMovimiento.idMoneda1 != unaTrx.listaMonedas[0]) ||
                (unMovimiento.idMoneda2 != unaTrx.listaMonedas[1]))
                throw new MonedaMovimientoDifereConfiguracion();

            IMoneda unaMoneda1 = _monedas.obtenerMonedaPorCodigo(unMovimiento.idMoneda1);
            IMoneda unaMoneda2 = _monedas.obtenerMonedaPorCodigo(unMovimiento.idMoneda2);

            switch (unaTrx.tipoTrx)
            {
                case CompraMonex:
                    unaMoneda1.ingreso(-unMovimiento.importe1);
                    unaMoneda2.extraccion(-unMovimiento.importe2);

                    break;
                case VentaMonex:
                    unaMoneda1.extraccion(-unMovimiento.importe1);
                    unaMoneda2.ingreso(-unMovimiento.importe2);
                    break;
                case PasesTesoro:
                    if (unMovimiento.subcodigo == RecepcionTesoro)
                        unaMoneda1.ingreso(-unMovimiento.importe1);
                    else
                        unaMoneda1.extraccion(-unMovimiento.importe1);

                    break;

                case VentaProductos:
                    unaMoneda1.ingreso(-unMovimiento.importe1);

                    break;
            }

            ITotalizador unTotalizador;
            foreach (TotalizadorOperacion tot in unaTrx.listaTotalizadorOp)
            {
                unTotalizador = _totalizadores.obtenerTotalizadorPorCodigo(tot.codigo);


                if (tot.operacion.Equals("+"))
                {
                    if (tot.moneda == 1)
                        unTotalizador.incrementar(-unMovimiento.importe1);
                    else
                        unTotalizador.incrementar(-unMovimiento.importe2);
                }
                else
                {
                    if (tot.moneda == 1)
                        unTotalizador.incrementar(unMovimiento.importe1);
                    else
                        unTotalizador.incrementar(unMovimiento.importe2);

                }
            }
            unMovimiento.anular();

            //Estado cero = anulado
            return (unMovimiento.estado == 0);
        }

    }
}
