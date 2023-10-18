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
        Apertura = 421,
        Cierre = 422,
        CierreActivosYPasivos = 423,
        AjustesPorInflacion = 469,
        AjustesPorInflacionPorSucursal = 573,
    }

    public enum EnumTiposImputaciones
    {
        Debe = 208,
        Haber = 209,
    }

    public enum EnumCodigosAsientosModelos
    {
        //Modulo Afiliaciones/Ahorros
        AfiCompensaciones,
        AfiCompensacionesMonotributistas,

        //Modulo Ahorros
        AhoDepExt,
        AhoComi,
        AhoComiExterior,
        AhoInteres,
        AhoPFCapital,
        AhoPFInteres,
        AhoPFImporteTotal,
        AhoPFCapitalRenovar,
        AhoPFInteresRenovar,
        AhoRecDepo,

        //Modulo de Bancos
        ValDepo,
        TarjetasCreditos,
        ChequeBancoSol,
        ImpDebito,
        ImpCredito,
        
        //Modulo Ordenes Cobros
        VTAFacVenta,
        VTANotaCreditoVenta,
        VTAResultadoPorVenta,
        VTAAnticiposClientes,
        CobCobrosPorCargos,
        //Modulo Contabilidad

        //Modulo Cuentas a Pagar
        CapSPProv,
        CapSPAnticipo,
        CapSPAnticipoDescontar,
        
        //Modulo de Haberes
        HabPoderes,
        HabPoderesDevolver,
        HabPoderesPagar,

        //Modulo de Prestamos
PreAnticipos,
        PreBonificaciones,
        PreCancelaciones,
        PreCancelacionesComisiones,
        PreCancelacionesInteres,
        PreCancelacionesInteresesACobrar,
        PreCancelacionesInteresesACobrarNoCte,
        PreCancelacionesInteresNoCte,
        PreCancelacionesNoCte,
        PreComisiones,
        PreInteres,
        PreInteresesACobrar,
        PreInteresesACobrarNoCte,
        PreInteresNoCte,
        PrePrestamos,
        PrePrestamosNoCte,
        PrePrestamosCesionados,
        PreServicios,

        //Modulo de Tipos de Cargos
        CarCargosACobrarTerceros,
        CarCargosCobrados,
        CarDescuentosRecuperar,
        CarEnviosCargos,
        CarDescuentosNoRecuperados,
        //CarDescuentosGenerar,
        CarNotasCreditos,

        //Impuestos
        IVACreditoFiscal,
        IVADebitoFiscal,
        
        //Subsidios
        SubPagar,
        SubGastos,

        //Opciones de Valores o Codigos Comodines (No tienen la cuenta contable)
        TablaBanco,
        TablaBancoDestino,
        TablaCmpFliaProducto,
        TablaConceptosContables,
        TablaCaja,
        //TablaChequesTerceros,        
        TablaTiposCargos,
        //TablaValDepo,
        TablaListaValorCuentaContable,

        //Comodin
        Resultado,
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

    
}
