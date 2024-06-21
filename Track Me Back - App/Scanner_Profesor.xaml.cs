using MongoDB.Bson;
using Track_Me_Back___App.ClaseUtilitare;
using TrackMeBack_Mobile.ClaseUtilitare;
using ZXing;
using ZXing.Net.Maui.Controls;

namespace Track_Me_Back___App;

public partial class Scanner_Profesor : ContentPage
{
    bool cod_rulat = false;
    public Scanner_Profesor()
	{
		InitializeComponent();
        
        Dispatcher.Dispatch(() =>
        {
            cititor_QR.Options = new ZXing.Net.Maui.BarcodeReaderOptions
            {
               Formats = ZXing.Net.Maui.BarcodeFormat.QrCode,
                AutoRotate = true,
                Multiple = false
            };
        });
    }


    private void cititor_QR_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        var result = e.Results.FirstOrDefault();
        if (result != null)
        {
            Dispatcher.Dispatch(() =>
            {
                string sala_curenta = result.Value;
                string mesaj = string.Empty;

                if (!cod_rulat)
                {
                    string ora = DateTime.Now.ToString("HH:mm");
                    string ziua = DateTime.Today.DayOfWeek.ToString().ToUpper();

                    Sistem_Start_Ora.Adaugare_Disciplina_Activa(sala_curenta,out mesaj);
                    DisplayAlert(" ", mesaj, "Ok");
                    cod_rulat = true;
                    Shell.Current.GoToAsync(nameof(Profil_Profesor));
                }

            });
        }
    }

    private void BackBtn_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(Profil_Profesor));
    }

}