using System.ComponentModel.DataAnnotations;

namespace Agri_Connect.Enums
{
    public enum ProductType
    {
        [Display(Name = "Solar Pumps")]
        SolarPumps,

        [Display(Name = "Irrigation Systems")]
        Irrigation,

        [Display(Name = "Biogas Systems")]
        BiogasSystem,

        [Display(Name = "Grow Lights")]
        GrowLights,

        [Display(Name = "Fertalizers")]
        Fertilizer,

        [Display(Name = "Compost Machines")]
        CompostMachine,

        Other
    }
}
