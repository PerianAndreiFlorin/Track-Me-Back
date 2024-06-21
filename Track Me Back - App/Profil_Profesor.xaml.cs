using TrackMeBack_Management.Clase_Utilitare.Management;

namespace Track_Me_Back___App;

public partial class Profil_Profesor : ContentPage
{
	public Profil_Profesor()
	{
		InitializeComponent();
        Seteaza_Info();
	}

    private void ScannerBtn_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(Scanner_Profesor));
    }

    private void Seteaza_Info()
    {
        string nume, marca, facultate;
        Management_UtilizatorCurent.Get_Info_Profesor(out nume, out marca, out facultate);

        Label_Nume_Profesor.Text += nume;
        Label_Marca_Profesor.Text += marca;
        Label_Facultate_Profesor.Text += facultate;

    }

    private void LogOutBtn_Clicked(object sender, EventArgs e)
    {
        Management_UtilizatorCurent.Resetare_Utilizator();
        Shell.Current.GoToAsync(nameof(Auth));

    }
}