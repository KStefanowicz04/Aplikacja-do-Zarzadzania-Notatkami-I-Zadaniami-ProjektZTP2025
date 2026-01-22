using static Program;

public partial class Program
{
    // Interfejs Buildera
    public interface IZadanieBuilder
    {
        ZadanieBuilder UstawTytul(string tytul);
        ZadanieBuilder UstawTresc(string tresc);
        ZadanieBuilder UstawStan(IStanZadania stan);
        ZadanieBuilder UstawPriorytet(Priorytet priorytet);
        ZadanieBuilder UstawTermin(DateTime termin);
        ZadanieBuilder UstawTagi(List<string> nazwyTagow);
    }

    // Builder konstruujący Zadania
    public class ZadanieBuilder : IZadanieBuilder
    {
        // Wskaźnik na Zadanie
        private Zadanie _zadanie = new Zadanie();


        // Ustawienie tytułu Zadania
        public ZadanieBuilder UstawTytul(string tytul)
        {
            _zadanie.tytul = tytul;
            return this;
        }

        // Ustawienie treści Zadania
        public ZadanieBuilder UstawTresc(string tresc)
        {
            _zadanie.tresc = tresc;
            return this;
        }

        // Ustawienie stanu Zadania
        public ZadanieBuilder UstawStan(IStanZadania stan)
        {
            _zadanie.Stan = stan;
            return this;
        }

        // Ustawienie priorytetu Zadania
        public ZadanieBuilder UstawPriorytet(Priorytet priorytet)
        {
            _zadanie.PriorytetZadania = priorytet;
            return this;
        }

        // Ustawienie terminu Zadania
        public ZadanieBuilder UstawTermin(DateTime termin)
        {
            _zadanie.Termin = termin;
            return this;
        }

        // Ustawienie tagów Zadania
        // (podobnie jak w Fabryce, podane w Liście stringów Tagi nie koniecznie istnieją)
        public ZadanieBuilder UstawTagi(List<string> nazwyTagow)
        {
            List<Tag> tagi = null;
            if (nazwyTagow != null)
            {
                // MenedżerTagów zajmuje się znalezieniem i zwróceniem odpowiednich Tagów.
                tagi = new List<Tag>();  // Rzeczywista Lista Tagów, przekazywana do Zadania
                foreach (string nazwaTagu in nazwyTagow)
                {
                    // Pytamy MenedżeraTagów o zwrócenie wskaźnika na dany Tag.
                    // Jeśli zwróci 'null', dany Tag nie istnieje, więc go nie dodajemy
                    Tag tag = MenedzerTagow.GetterInstancji().ZwrocTag(nazwaTagu);
                    if (tag != null)
                    {
                        tagi.Add(tag);
                    }
                }
            }

            _zadanie.tagi = tagi;
            return this;
        }

        // Ukończenie tworzenia i zwrócenie utworzonego Zadania
        public Zadanie Build()
        {
            // Dodanie Zadania do Listy Zadań MenedżeraZadań
            Zadanie wynik = _zadanie;
            MenedzerZadan.GetterInstancji().DodajZadanie(_zadanie);

            // Reset; przy następnej konstrukcji utworzone zostanie nowe Zadanie.
            _zadanie = new Zadanie();

            return wynik;
        }
    }
}
