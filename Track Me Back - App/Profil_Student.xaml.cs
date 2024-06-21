using TrackMeBack_Management.Clase_Utilitare.Management;

namespace Track_Me_Back___App;

public partial class Profil_Student : ContentPage
{
	public Profil_Student()
	{
		InitializeComponent();

        Seteaza_Info();
	}

    private void Seteaza_Info()
    {
        string nume, marca, specializare;
        Management_UtilizatorCurent.Get_Info_Student(out nume, out marca, out specializare);

        Label_Nume_Student.Text += nume;
        Label_Marca_Student.Text += marca;
        Label_Specializare_Student.Text += specializare;
    }

    private void ScannerBtn_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(Scanner_Student));
    }

    private void LogOutBtn_Clicked(object sender, EventArgs e)
    {
        Management_UtilizatorCurent.Resetare_Utilizator();
        Shell.Current.GoToAsync(nameof(Auth));
    }
}