using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OperacionCaja;
using OperacionCaja.ServiceReferenceProducto;
using OperacionCaja.ServiceReferenceCotizacion;
using OperacionCaja.ServiceReferenceSM;

namespace OperacionCaja
{
    class Program
    {

        public void function()
        //static void Main(string[] args)
        {
            IntegracionCS integracion = new IntegracionCS();
            //Ejemplo de como obtener la información de todas las monedas desde el servidor
            InfoMoneda[] cotizaciones = integracion.obtenerTodasLasMonedas();
            foreach (InfoMoneda unaMoneda in cotizaciones)
            {
                Console.WriteLine(unaMoneda.descripcion);
            }
            //TWSCotizacion unaCotizacion2 =  unCliCot.obtenerCotizacion(idSesionCotizacion, 711);


            //Cargo la información de las transacciones
            TrxManager unTrxManager = new TrxManager(@"C:\Users\CristianPC\Desktop\SIII\OperacionCaja\TrxManager.xml", integracion, integracion);

            //Cargo las monedas con las que quiero usar "el sistema"
            //Tener en cuenta que la información de todas las monedas debería tomarse del servicio de cotizaciones
            unTrxManager.monedas.agregar(new Moneda(0, "Pesos Argentinos"));
            unTrxManager.monedas.agregar(new Moneda(2, "Dólares"));
            unTrxManager.monedas.agregar(new Moneda(11, "Pesos uruguayos"));
            unTrxManager.monedas.agregar(new Moneda(30, "Real"));
            unTrxManager.monedas.agregar(new Moneda(50, "Euros"));

            //Cargo los totalizadores que quiero usar con "el sistema".
            //Voy a agregar TODOS los totalizadores aunque se podrían agregar menos (según la conveniencia de la prueba)
            foreach (TotalizadorInfo info in unTrxManager.infoTransaccion.totalizadores)
                unTrxManager.totalizadores.agregar(new Totalizador(info.codigo, info.descripcion));

            //Ejecuto una transacción de pase del tesoro para ponerla plata a la caja
            
            //Primero creo el movimiento
            Movimiento unMovimiento = new Movimiento();
            unMovimiento.codigo = 200;
            unMovimiento.subcodigo = 001;
            
            
            unMovimiento.idMoneda1 = MonedaConst.pesos_argentinos; //Pesos
            unMovimiento.idMoneda2 = MonedaConst.pesos_argentinos; // Pesos;

            unMovimiento.importe1 = 10000; //Importe de la transacción

            unMovimiento.referencia = "Saldo inicial de caja";
            unMovimiento.descripcion = "Saldo inicial de caja";

            //El resto lo asigna el ejecutor de transacciones
            unTrxManager.ejecutarTransaccion(unMovimiento);

            //Si ejecutó bien la transacción, al menos debería tener 10000 pesos!
            Console.WriteLine("{0}", unTrxManager.monedas.obtenerMonedaPorCodigo(0).saldo);
            

            //Movimiento de Compra y Venta
            unMovimiento = new Movimiento();
            unMovimiento.codigo = 102;
            unMovimiento.subcodigo = 001;

            unMovimiento.idMoneda1 = MonedaConst.dolar_americano;
            unMovimiento.idMoneda2 = MonedaConst.pesos_argentinos;
            unMovimiento.importe1 = 500;

            unMovimiento.referencia = "11101231";
            unMovimiento.descripcion = "Compra de 500 dólares";

            //El resto lo asigna el ejecutor de transacciones
            unTrxManager.ejecutarTransaccion(unMovimiento);

            //Si ejecutó bien la transacción, al menos debería tener 10000 pesos!
            Console.WriteLine("{0}", unTrxManager.monedas.obtenerMonedaPorCodigo(0).saldo);
            Console.ReadKey();

        }



        static void Main(string[] args)
        {

            IntegracionCS integracion = new IntegracionCS();
            //Ejemplo de como obtener la información de todas las monedas desde el servidor
            InfoMoneda[] cotizaciones = integracion.obtenerTodasLasMonedas();
            

            TrxManager manager = new TrxManager(@"C:\Users\CristianPC\Desktop\SIII\OperacionCaja\TrxManager.xml", integracion, integracion);

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



            

            //manager.totalizadores.obtenerTotalizadorPorCodigo(1).importeAsociado = 3000;

            Console.WriteLine("{0}", manager.totalizadores.obtenerTotalizadorPorCodigo(1).importeAsociado);
            Console.WriteLine("{0}", manager.totalizadores.obtenerTotalizadorPorCodigo(50).importeAsociado);
            Console.WriteLine("{0}", manager.monedas.obtenerMonedaPorCodigo(0).saldo);
            

            Movimiento mov = new Movimiento();
            mov.codigo = 300;
            mov.subcodigo = 002;
            mov.idMoneda1 = MonedaConst.pesos_argentinos; //Pesos
            mov.importe1 = 1000;
            mov.referencia = "ref";
            mov.descripcion = "descr";


            manager.ejecutarTransaccion(mov);
             Console.WriteLine("{0}", manager.totalizadores.obtenerTotalizadorPorCodigo(1).importeAsociado);
             Console.WriteLine("{0}", manager.totalizadores.obtenerTotalizadorPorCodigo(50).importeAsociado);
             Console.WriteLine("{0}",manager.monedas.obtenerMonedaPorCodigo(0).saldo);
             Console.ReadKey();

            
             Movimiento mov1 = new Movimiento();
             mov1.codigo = 300;
             mov1.subcodigo = 002;
             mov1.idMoneda1 = MonedaConst.pesos_argentinos; //Pesos
             mov1.importe1 = 1000;
             mov1.referencia = "ref";
             mov1.descripcion = "descr";


             manager.ejecutarTransaccion(mov);
             Console.WriteLine("{0}", manager.totalizadores.obtenerTotalizadorPorCodigo(1).importeAsociado);
             Console.WriteLine("{0}", manager.totalizadores.obtenerTotalizadorPorCodigo(50).importeAsociado);
             Console.WriteLine("{0}", manager.monedas.obtenerMonedaPorCodigo(0).saldo); 
             Console.ReadKey();
            

           // Assert.AreEqual(manager.monedas.obtenerMonedaPorCodigo(0).saldo, 11000);
        }


    }
}
