namespace refShop_DEV.Models.Restaurant
{

    public class TurnoDTO
{
    public int IDTurno { get; set; }
    public int IDEmpleado { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public DateTime FechaDeCreacion { get; set; }
    public int CreadoPor { get; set; }
    public string Dias_Laborales { get; set; }

    // ... otras propiedades y relaciones ...
}



}
