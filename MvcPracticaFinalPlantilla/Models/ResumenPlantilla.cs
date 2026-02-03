namespace MvcPracticaFinalPlantilla.Models
{
    public class ResumenPlantilla
    {
        public int Personas { get; set; }
        public int MaximoSalario { get; set; }
        public double MediaSalarial { get; set; }
        public List<Plantilla> Plantilla { get; set; }
    }
}
