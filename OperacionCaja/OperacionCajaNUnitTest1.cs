using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

using OperacionCaja.ServiceReferenceCotizacion;
using OperacionCaja.ServiceReferenceProducto;
using OperacionCaja.ServiceReferenceSM;

namespace OperacionCaja
{
    [TestFixture]
    public class OperacionCajaNUnitTest
    {
        private SesionManagerClient wsSM;
        private CotizacionClient wsCot;
        private ProductoClient wsClient;

        private String _idCotizacion;
        private String _idProducto;

        [SetUp]
        protected void Setup()
        {

            wsCot = new CotizacionClient();
            wsClient = new ProductoClient();
            wsSM = new SesionManagerClient();

            _idCotizacion = wsSM.obtenerIDSesionCotizacion(System.Environment.MachineName);
            _idProducto = wsSM.obtenerIDSesionProducto(System.Environment.MachineName);
        }

        [TearDown]
        protected void TearDown()
        {
            wsSM.cerrarSesionCotizacion(_idCotizacion);
            wsSM.cerrarSesionProducto(_idProducto);
        }

        [Test]
        public void probarAccesoSeviciosWebCotizacion()
        {
            TWSCotizacion unaCotizacion = wsCot.obtenerCotizacion(_idCotizacion, 0);

            Console.Error.WriteLine("Se obtuvo informaci'on para la moneda: " + unaCotizacion.Descripcion);

            Assert.AreEqual(unaCotizacion.Compra, 1);
        }

        //TEST1_us0001
        [Test]
        public void probarTotalizadoresEnCero()
        {
            Totalizador t = new Totalizador(1, "totalizadorUno");
            t.incrementar(5);

            ColeccionTotalizador totalizadores = new ColeccionTotalizador();
            totalizadores.agregar(t);

            bool initCorrecto = totalizadores.inicializar();

            Assert.AreEqual(initCorrecto, true);
        }

        //TEST2_us0001
        [Test]
        public void probarMonedasEnCero()
        {
            Moneda m = new Moneda(1, "PesosArgentinos");

            m.saldoInicial = 10;

            ColeccionMoneda monedas = new ColeccionMoneda();
            monedas.agregar(m);

            bool initCorrecto = monedas.inicializar();

            Assert.AreEqual(initCorrecto, true);
        }

        //TEST3_us0003
        [Test]
        public void pruebaPasesAlTesoroTotalizadorIncrementado()
        {
            TrxManager manager = new TrxManager(@"C:\Users\CristianPC\Desktop\SIII\OperacionCaja\TrxManager.xml", null, null);

            manager.monedas.agregar(new Moneda(0, "Pesos Argentinos"));

            //Cargo todos los totalizadores que levanto del XML de transacciones
            foreach (TotalizadorInfo info in manager.infoTransaccion.totalizadores)
                manager.totalizadores.agregar(new Totalizador(info.codigo, info.descripcion));

            /*
                        //cargo una caja inicial
                        Movimiento unMovimiento = new Movimiento();
                        unMovimiento.codigo = 200;
                        unMovimiento.subcodigo = 001;


                        unMovimiento.idMoneda1 = MonedaConst.pesos_argentinos; //Pesos
                        unMovimiento.idMoneda2 = MonedaConst.pesos_argentinos; // Pesos;

                        unMovimiento.importe1 = 10000; //Importe de la transacción

                        unMovimiento.referencia = "Saldo inicial de caja";
                        unMovimiento.descripcion = "Saldo inicial de caja";

  
                      //El resto lo asigna el ejecutor de transacciones
                        manager.ejecutarTransaccion(unMovimiento);
            */

            //Inicializar la caja con 10.000 
            manager.totalizadores.obtenerTotalizadorPorCodigo(1).importeAsociado = 10000;
            manager.monedas.obtenerMonedaPorCodigo(0).saldoInicial = 10000;

            Movimiento mov = new Movimiento();
            mov.codigo = 300;
            mov.subcodigo = 002;
            mov.idMoneda1 = MonedaConst.pesos_argentinos; //Pesos
            mov.importe1 = 1000;
            mov.referencia = "ref";
            mov.descripcion = "descr";


            manager.ejecutarTransaccion(mov);

            Console.Error.WriteLine("Se obtuvo información para el movimiento: " + mov.codigo + "-" + mov.subcodigo);

            Assert.AreEqual(manager.totalizadores.obtenerTotalizadorPorCodigo(1).importeAsociado, 9000);
            Assert.AreEqual(manager.totalizadores.obtenerTotalizadorPorCodigo(50).importeAsociado, 1000);
            Assert.AreEqual(manager.monedas.obtenerMonedaPorCodigo(0).saldo, 9000);

        }

        //TEST4
        //Prueba el ingreso de efectivo a una moneda
        [Test]
        public void caIngresoMoneda()
        {
            Moneda m = new Moneda(0, "Pesos argentinos");

            m.ingreso(100);

            Assert.AreEqual(m.saldo, 100);
            Assert.AreEqual(m.cantEntradas, 1);
        }

        //TEST5
        //Prueba la extraccion de efectivo a una moneda
        [Test]
        public void caExtraccionMoneda()
        {
            Moneda m = new Moneda(30, "Real");
            m.ingreso(500);
            m.extraccion(200);

            Assert.AreEqual(m.saldo, 300);
            Assert.AreEqual(m.cantSalidas, 1);
        }

        //TEST6
        //Prueba el lanzamiento de una excepcion cuando se intenta extraer un monto mayor al saldo actual de una moneda
        [Test]
        [ExpectedException(typeof(SaldoNegativoException))]
        public void caExtraccionMontoMayorASaldo()
        {
            Moneda m = new Moneda(30, "Real");
            m.ingreso(500);
            m.extraccion(600);
        }

        //TEST7
        //Prueba del Articulo

        [Test]
        public void caSeteoArticulos()
        {
            Articulo art = new Articulo(1, "Articulo1", 100);

            String desc = art.descripcion;
            int cod = art.codigo;
            double cant = art.cantidad;

            Assert.AreEqual(art.descripcion, "Articulo1");
            Assert.AreEqual(art.codigo, 1);
            Assert.AreEqual(art.cantidad, 100);
        }

        //TEST8
        //Prueba de anulacion de un Movimiento
        [Test]
        public void caAnularMovimiento()
        {
            Movimiento m = new Movimiento();
            m.codigo = 100;
            m.anular();

            Assert.AreEqual(m.codigo, 0);
        }

        //TEST9
        //Prueba de 
        [Test]
        public void pruebaPasesAlaCajaTotalizadorIncrementado()
        {
            TrxManager manager = new TrxManager(@"C:\Users\CristianPC\Desktop\SIII\OperacionCaja\TrxManager.xml", null, null);

            manager.monedas.agregar(new Moneda(0, "Pesos Argentinos"));

            //Cargo todos los totalizadores que levanto del XML de transacciones
            foreach (TotalizadorInfo info in manager.infoTransaccion.totalizadores)
                manager.totalizadores.agregar(new Totalizador(info.codigo, info.descripcion));

            
                        //cargo una caja inicial
                        Movimiento unMovimiento = new Movimiento();
                        unMovimiento.codigo = 200;
                        unMovimiento.subcodigo = 001;


                        unMovimiento.idMoneda1 = MonedaConst.pesos_argentinos; //Pesos
                        unMovimiento.idMoneda2 = MonedaConst.pesos_argentinos; // Pesos;

                        unMovimiento.importe1 = 10000; //Importe de la transacción

                        unMovimiento.referencia = "Saldo inicial de caja";
                        unMovimiento.descripcion = "Saldo inicial de caja";

  
                      //El resto lo asigna el ejecutor de transacciones
                        manager.ejecutarTransaccion(unMovimiento);
       

            Assert.AreEqual(manager.totalizadores.obtenerTotalizadorPorCodigo(1).importeAsociado, 10000);
            Assert.AreEqual(manager.totalizadores.obtenerTotalizadorPorCodigo(10).importeAsociado, 10000);
            Assert.AreEqual(manager.monedas.obtenerMonedaPorCodigo(0).saldo, 10000);
        }
    }




}