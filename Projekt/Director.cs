public partial class Program
{
    // Director zajmujący się Builderem
    public class ZadanieDirector
    {
        // Wskaźnik na Builder
        private ZadanieBuilder _builder;

        // Konstruktor
        public ZadanieDirector(ZadanieBuilder builder)
        {
            _builder = builder;
        }


        // Metody konstruujące przykładowe Zadania
        // Zadanie o Priorytecie Wysokim do ukończenia za 1 dzień
        public void KonstruujZadaniePilne(string tytul, string tresc)
        {
            _builder
               .UstawTytul(tytul)
               .UstawTresc(tresc)
               .UstawPriorytet(Priorytet.Wysoki)
               .UstawStan(new StanAktywne())
               .UstawTermin(DateTime.Now.AddDays(1))
               .Build();
        }

        // Zadanie o Niskim Priorytecie zaplanowane na za 1 miesiąc
        public void KonstruujZadanieDlugoterminowe(string tytul, string tresc)
        {
            _builder
               .UstawTytul(tytul)
               .UstawTresc(tresc)
               .UstawPriorytet(Priorytet.Niski)
               .UstawStan(new StanAktywne())
               .UstawTermin(DateTime.Now.AddMonths(1))
               .Build();
        }

        // Zadanie o Niskim Priorytecie na za 1 dzień
        public void KonstruujZadanieKrotkoterminowe(string tytul, string tresc)
        {
            _builder
               .UstawTytul(tytul)
               .UstawTresc(tresc)
               .UstawPriorytet(Priorytet.Niski)
               .UstawStan(new StanAktywne())
               .UstawTermin(DateTime.Now.AddDays(1))
               .Build();
        }
    }
}
