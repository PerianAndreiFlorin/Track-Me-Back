namespace Track_Me_Back___App;

using Track_Me_Back___App.ClaseUtilitare;
using TrackMeBack_Management.Clase_Utilitare.Management;
using TrackMeBack_Mobile.ClaseUtilitare;
public partial class Auth : ContentPage
{
	public Auth()
	{
		InitializeComponent();
	}

    private void OnAutentificareClicked(object sender, EventArgs e)
    {
        string email = Email_Input.Text.ToLower();
        string parola = Parola_Input.Text;

        if (Servicii_MongoDB.Autentificare.Acces(email, parola))
        {
            if (email.Contains("student"))
            {
                Management_UtilizatorCurent.Set_Info_Student(email);
                Shell.Current.GoToAsync(nameof(Profil_Student));
            }
            else
            {
                Management_UtilizatorCurent.Set_Info_Profesor(email);
                Shell.Current.GoToAsync(nameof(Profil_Profesor));
            }
        }
        else
        {
            DisplayAlert("Eroare", "Email sau parolă incorecte!", "OK");
        }
    }
}