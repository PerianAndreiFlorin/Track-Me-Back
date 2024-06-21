using Track_Me_Back___App.ClaseUtilitare;

namespace Track_Me_Back___App;

public partial class Scanner_Student : ContentPage
{
    bool cod_rulat = false;
    public Scanner_Student()
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
                string ziua = DateTime.Today.DayOfWeek.ToString().ToUpper();

                if (!cod_rulat)
                {
                    cod_rulat = true;
                    switch (SistemPrezenta.Verificare(sala_curenta, ziua, out mesaj))
                    {
                        default:
                            DisplayAlert("Felicitări!", mesaj, "Ok");
                            Shell.Current.GoToAsync(nameof(Profil_Student));
                            break;
                        case 0:
                            DisplayAlert("Atenție!", mesaj, "Ok");
                            Shell.Current.GoToAsync(nameof(Profil_Student));
                            break;
                        case -1:
                            DisplayAlert("Oops!", mesaj, "Ok");
                            Shell.Current.GoToAsync(nameof(Profil_Student));
                            break;
                        case 2:
                            DisplayAlert("Felicitări!", mesaj, "Ok");
                            Shell.Current.GoToAsync(nameof(Profil_Student));
                            break;
                    }   
                }
            });
        }
    }

    private void BackBtn_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(Profil_Student));
    }
}