using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System;


namespace OperacionCaja
{
    /// <summary>
    /// Excepción lanzada en el caso que el archivo de configuración de transacciones y totalizadores 
    /// no exista en la ubicación especificada
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    public class ArchivoConfiguracionInexistenteException : SystemException
    {
    }

    /// <summary>
    /// Excepción lanzada en el caso en que se busque anular una transacción ya anulada.
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    public class TransaccionYaAnuladaException : SystemException
    {
    }

    /// <summary>
    /// Excepción lanzada en el caso en que el importe de la transacción no coincida con lo que se espera de la misma
    /// Algunos motivos por el que se podría lanzar esta transacción:
    /// 1.El importe de una transacción no puede ser con decimales
    /// 2.El importe de una transacción de venta no coincide con la información de los artículos asociados
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    public class ImporteInvalidoException : SystemException
    {
    }

    /// <summary>
    /// Excepción lanzada en el caso en que se intente invocar a alguna transacción no parametrizada
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    public class TransaccionInexistenteException : SystemException
    {
    }

    /// <summary>
    /// Excepción lanzada en el caso en que la moneda del movimiento no coincida con la moneda de la 
    /// parametrización de la transacción
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    public class MonedaMovimientoDifereConfiguracion : SystemException
    {
    }

    /// <summary>
    /// Excepción lanzada en el caso que alguna operación intente dejar algún totalizador o moneda con saldo negativo
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    public class SaldoNegativoException : SystemException
    {
    }

    /// <summary>
    /// Excepción lanzada en el caso que se intente operar con alguna moneda no existente en la colección de monedas
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    public class MonedaInexistenteException : SystemException
    {
    }

    /// <summary>
    /// Excepción lanzada en el caso que se intente operar con algún totalizador no existente en la colección de totalizadores
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    public class TotalizadorInexistenteException : SystemException
    {
    }

    /// <summary>
    /// Excepción lanzada en el caso que se intente rescatar una operación aún no generada
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    public class OperacionInexistenteException : SystemException
    {
    }
}
