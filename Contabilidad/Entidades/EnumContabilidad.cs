using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Contabilidad.Entidades
{
    public enum EnumTiposAsientos
    {
        Automaticos = 180,
        Manuales = 182,
    }

    public enum EnumTiposImputaciones
    {
        Debe = 208,
        Haber = 209,
    }

    public enum EnumCodigosAsientosModelos
    {
        //Modulo Ahorros
        AhoDepExt,
        AhoComi,
        AhoInteres,
        AhoPFCapital,
        AhoPFInteres,
        AhoPFImporteTotal,
        AhoPFCapitalRenovar,
        AhoPFInteresRenovar,
        AhoRecDepo,

        //Modulo de Bancos
        ValDepo,
        
        //Modulo Contabilidad

        //Modulo Cuentas a Pagar
        CapSPProv,
        
        //Modulo de Haberes
        HabPoderes,
        HabPoderesDevolver,
        HabPoderesPagar,

        //Modulo de Prestamos
        PreAnticipos,
        PrePrestamos,
        PreInteres,
        PreComisiones,
        PreCancelaciones,
        PreCancelacionesComisiones,
        PreCancelacionesInteres,

        //Modulo de Tipos de Cargos
        CarDescuentosRecuperar,
        CarEnviosCargos,
        CarDescuentosNoRecuperados,

        //Opciones de Valores o Codigos Comodines (No tienen la cuenta contable)
        TablaBanco,
        TablaCmpFliaProducto,
        TablaConceptosContables,
        TablaCaja,
        //TablaChequesTerceros,        
        TablaTiposCargos,
        //TablaValDepo,
    }

    //public enum EnumCodigosEntidadesContables
    //{
    //    Efectivo,
    //    ChequePropio,
    //    ChequeTercero,
    //    Transferencia,
    //    TarjetaCredito,
    //    FliaProducto,
    //}

    public enum EnumEstadosBienesUsos
    {
        Pendiente = 38,
        Activado = 67,
        Desactivado = 68,
    }

    public enum EnumClasificador
    {
        Herramienta = 236,
        Auto = 237,
        Inmueble = 238,
        Otro = 239,
    }
}
