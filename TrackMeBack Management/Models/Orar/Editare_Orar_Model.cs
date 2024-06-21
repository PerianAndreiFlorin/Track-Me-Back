using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MongoDB.Bson;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;
using TrackMeBack_Management.Clase_Utilitare.Management;

namespace TrackMeBack_Management.Models.Orar
{
    public class Editare_Orar_Model
    {
        [Display(Name = "Acțiune")]
        public string actiune {  get; set; } = string.Empty;

        [Display(Name = "Ziua")]
        public string ziua {  get; set; } = string.Empty;

        [Display(Name = "Ora de început")]
        public string ora_start {  get; set; } = string.Empty;

        [Display(Name = "Materia")]
        public string materia { get; set; } = string.Empty;


        public string specializare {  get; set; } = string.Empty;
        [Display(Name = "Anul")]
        public int anul {  get; set; }

        [Display(Name = "Grupa")]
        public int grupa { get; set; }

        [Display(Name = "Sala")]
        public string sala { get; set; } = string.Empty;
    }
}
