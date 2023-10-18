namespace Afiliados.Entidades
{
    public enum EstadosAfiliados
    {
        /*
        Normal=0,
        Renuncia=1,
        Fallecido=2,
        Expulsado=3,
        Suspendido=4,
        Vitalicio=6,
        Baja=7,
        */
        Normal = 2,
        Renuncia = 3,
        Fallecido = 4,
        Expulsado = 5,
        Suspendido = 6,
        //Vitalicio = 7,
        Baja = 0,
        Moroso = 29,
        AvisodeFallecido = 30,
        Potencial = 74,
        Pendiente = 38,
        Vigente = 84,
        PreJudicial = 96,
        Juridico = 97,
        BajaArt171B = 82,
        Incobrable = 102,
        Cupon = 103,
        Cobranzas = 104,
        Recupero = 105,
    }

    public enum EstadosMensajesAlertas
    {
        Baja = 0,
        Activo = 1,
    }
}
