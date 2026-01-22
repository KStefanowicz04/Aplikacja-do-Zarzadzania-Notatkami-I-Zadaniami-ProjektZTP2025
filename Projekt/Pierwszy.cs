public partial class Program
{
    public interface IZadanieBuilder
    {
        ZadanieBuilder UstawTytul(string tytul);
        ZadanieBuilder UstawTresc(string tresc);
        ZadanieBuilder UstawStan(IStanZadania stan);
        ZadanieBuilder UstawPriorytet(Priorytet priorytet);
        ZadanieBuilder UstawTermin(DateTime termin);
        ZadanieBuilder UstawTagi(List<Tag> tagi);
        Zadanie Build(); // Metoda kończąca budowanie
    }

    public class ZadanieBuilder : IZadanieBuilder
    {
        private Zadanie _zadanie = new Zadanie();

        public ZadanieBuilder UstawTytul(string tytul)
        {
            _zadanie.tytul = tytul;
            return this;
        }

        public ZadanieBuilder UstawTresc(string tresc)
        {
            _zadanie.tresc = tresc;
            return this;
        }

        public ZadanieBuilder UstawStan(IStanZadania stan)
        {
            _zadanie.Stan = stan;
            return this;
        }

        public ZadanieBuilder UstawPriorytet(Priorytet priorytet)
        {
            _zadanie.PriorytetZadania = priorytet;
            return this;
        }

        public ZadanieBuilder UstawTermin(DateTime termin)
        {
            _zadanie.Termin = termin;
            return this;
        }

        public ZadanieBuilder UstawTagi(List<Tag> tagi)
        {
            _zadanie.tagi = tagi;
            return this;
        }

        public Zadanie Build()
        {
            Zadanie wynik = _zadanie;
            _zadanie = new Zadanie();
            return wynik;
        }
    }
}
