using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperacionCaja
{
    public interface ITotalizador
    {
        void inicializar();
        int codigo { get; set; }
        string descripcion { get; set; }
        int cantEntradas { get; set; }
        int cantSalidas { get; set; }
        double importeAsociado { get; set; }
        double incrementar(double importe);
    }

    [Serializable]
    public class Totalizador : ITotalizador
    {
        private int _codigo;
        private string _descripcion;
        private int _cantEntradas;
        private int _cantSalidas;
        private double _importeAsociado;

        //Constructor

        /// <summary>
        /// Metodo constructor
        /// </summary>
        /// <param name="codigo">Código identificador del totalizador</param>
        /// <param name="descripcion">Descripción del totalizador</param>
        public Totalizador(int codigo, string descripcion)
        { 
            _codigo = codigo;
            _descripcion = descripcion;
            inicializar();
        }

        /// <summary>
        /// Inicializa la cantidad de entradas, cantidad de salidas e importe asociado en cero
        /// </summary>
        public void inicializar()
        {
            _cantEntradas = 0;
            _cantSalidas = 0;
            _importeAsociado = 0;
        }

        //Propiedades

        /// <summary>
        /// Propiedad codigo</summary>
        /// <value>
        /// Código identificador del totalizador
        /// </value>

        public int codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }

        /// <summary>
        /// Propiedad descripcion</summary>
        /// <value>
        /// Código identificador del totalizador
        /// </value>

        public string descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        /// <summary>
        /// Propiedad cantEntradas</summary>
        /// <value>
        /// Cantidad de entradas (incremento del importe asociado) que tuvo el totalizador
        /// </value>
        
        public int cantEntradas
        {
            get { return _cantEntradas; }
            set { _cantEntradas = value; }
        }

        /// <summary>
        /// Propiedad cantSalidas</summary>
        /// <value>
        /// Cantidad de salidas (disminución del importe asociado) que tuvo el totalizador
        /// </value>

        public int cantSalidas
        {
            get { return _cantSalidas; }
            set { _cantSalidas = value; }
        }

        /// <summary>
        /// Propiedad importeAsociado</summary>
        /// <value>
        /// importe asociado al totalizador
        /// </value>

        public double importeAsociado
        {
            get { return _importeAsociado; }
            set { _importeAsociado = value; }
        }


        /// <summary>
        /// método que permite incrementar o disminuir el totalizador.
        /// Se encarga de incrementar la cantidad de entradas y salidas
        /// </summary>
        /// <param name="importe">Importe asociado al movimiento del totalizador</param>
        /// <returns></returns>
        public double incrementar(double importe)
        {
            if (_importeAsociado + importe < 0)
                throw new SaldoNegativoException();
            
            if (importe > 0)
                _cantEntradas++;
            else
                _cantSalidas++;

            _importeAsociado += importe;
            return _importeAsociado;
        }

    }
}
