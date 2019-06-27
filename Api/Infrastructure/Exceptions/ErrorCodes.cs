namespace FirmaBudowlana.Infrastructure.Exceptions
{
    public static class ErrorCodes
    {
        public static string BladUsuwania => "nie_mozna_usunzc";

        public static string Nieznaleziono => "nie_znaleziono";

        public static string BladEdycji => "nie_mozna_edytowac";

        public static string PustyRequest => "odebrano_pusty_request";

        public static string NiepoprawnyFormat => "odebrano_niepoprawe_dane";

    }
}