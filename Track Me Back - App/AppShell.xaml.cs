namespace Track_Me_Back___App
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            Routing.RegisterRoute(nameof(Scanner_Profesor), typeof(Scanner_Profesor));
            Routing.RegisterRoute(nameof(Scanner_Student), typeof(Scanner_Student));
            Routing.RegisterRoute(nameof(Profil_Profesor), typeof(Profil_Profesor));
            Routing.RegisterRoute(nameof(Profil_Student), typeof(Profil_Student));
            Routing.RegisterRoute(nameof(Auth), typeof(Auth));

        }
    }
}
