using System.Collections.Generic;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Linq;
using System.Runtime.CompilerServices;
using static Program;

public partial class Program
{
    // Interfejs stanów Zadania (Wzorzec State)
    public interface IStanZadania
    {
        // Metody wykorzysywane przez ka¿dy Stan
        public void wykonane(Zadanie zadanie);
        public void aktywne(Zadanie zadanie);
        public void zalegle(Zadanie zadanie);
    }


    public enum Priorytet
    {
        Niski,
        Sredni,
        Wysoki
    }

    // Klasa Zadanie, pochodna od Wpis
    public class Zadanie : Wpis
    {
        private IStanZadania stan;  // Obecny stan zadania (wykonane/aktywne/zaleg³e)
        private Priorytet priorytet;  // Obecny priorytet zadania
        private DateTime termin;  // Termin wykonania zadania


        // Konstruktor
        public Zadanie(string tytul, string tresc, IStanZadania stan, Priorytet priorytet, DateTime termin, List<Tag> tagi) : base(tytul, tresc)
        {
            // Manager zadañ decyduje jaki numer ID zostanie przypisany do danego Zadania
            id = MenedzerZadan.GetterInstancji().WybierzIDZadania();

            // Domyœlnie stan jest Aktywny - mo¿e nale¿y to usun¹æ z parametrów Konstruktora?
            if (stan != null) this.stan = stan;
            else this.stan = new StanAktywne();

            this.stan = stan;
            this.priorytet = priorytet;
            this.termin = termin;

            if (tagi != null)
                this.tagi = tagi;
            else
                this.tagi = new List<Tag>();
        }


        // Metody

        public Priorytet Priorytet
        {
            get { return priorytet; }
        }
        // Zwraca termin wykonania zadania.
        // pole "termin" jest prywatne i nie mo¿e byæ bezpoœrednio odczytywane poza klas¹ Zadanie (np. przez Mened¿eraZadañ).
        public DateTime ZwrocTermin()
        {
            return termin;
        }
        // Edytowanie zadania
        public override void Edytuj(string tytul, string tresc, Priorytet priorytet, DateTime termin)
        {
            base.Edytuj(tytul, tresc);  // Wezwanie bazowej metody Edytuj w klasie Wpis
            this.priorytet = priorytet;
            this.termin = termin;
        }

        // Zmienia stan Zadania na wykonane
        public void OznaczJakoWykonane()
        {
            //stan = IStanZadania.wykonane;
            //ZmienStan();
        }


        // Zwrócenie nazwy obecnego stanu Zadania w formie string
        public string ZwrocStan()
        {
            return stan.GetType().Name;
        }

        // Zmiana obecnego stanu zadania
        public void ZmienStan(IStanZadania stan)
        {
            this.stan = stan;
        }

        // Zmienia obecny stan za pomoc¹ IStanZadania na Wykonane
        public void StanWykonane()
        {
            stan.wykonane(this);
        }

        // Zmienia obecny stan za pomoc¹ IStanZadania na Aktywne
        public void StanAktywne()
        {
            stan.aktywne(this);
        }

        // Zmienia obecny stan za pomoc¹ IStanZadania na Zaleg³e
        public void StanZalegle()
        {
            stan.zalegle(this);
        }

        // Zwraca true/false zale¿nie od obecnego stanu zadania
        public bool SprawdzCzyZalegle()
        {
            // Porównanie obecnego typu zmiennej stan z klas¹ StanWykonane;
            // sprawdzamy czy obecny stan jest stanem Wykonane
            if (stan.GetType() == typeof(StanWykonane))
                return false;

            // Porównuje czas obecny z wyznaczonym terminem zadania
            return DateTime.Now > termin;
        }

        // Wypisuje podstawowe informacje o zadaniu
        public override string WypiszInformacje()
        {
            return $"[ZADANIE] ID: {id} | Tytu³: {tytul} | Treœæ: {tresc} | Priorytet: {priorytet} | Termin: {termin:d}";
        }

        // Nadpisanie ToString() dla wygodnego wypisywania notatki
        public override string ToString()
        {
            return WypiszInformacje();
        }
    }

    // Konkretne stany Zadania
    //
    // Zadanie wykonane
    public class StanWykonane : IStanZadania
    {
        // Ponowienie tego stanu
        public void wykonane(Zadanie zadanie)
        {
            Console.WriteLine("Zadanie ju¿ zosta³o wykonane");
        }

        // Zmiana stanu z Wykonane na Aktywne
        public void aktywne(Zadanie zadanie)
        {
            Console.WriteLine("Zmiana stanu zadania z wykonane na aktywne");
        }

        // Zmiana stanu z Wykonane na Zaleg³e
        public void zalegle(Zadanie zadanie)
        {
            Console.WriteLine("Zmiana stanu zadania z wykonane na zaleg³e");
        }
    }

    // Zadanie aktywne
    public class StanAktywne : IStanZadania
    {
        // Zmiana stanu z Aktywne na Wykonane
        public void wykonane(Zadanie zadanie)
        {
            Console.WriteLine("Zmiana stanu zadania z aktywne na wykonane");
        }

        // Ponowienie tego stanu
        public void aktywne(Zadanie zadanie)
        {
            Console.WriteLine("Zadanie pozostaje aktywne");
        }

        // Zmiana stanu z Aktywne na Zaleg³e
        public void zalegle(Zadanie zadanie)
        {
            Console.WriteLine("Zmiana stanu zadania z aktywne na zaleg³e");
        }
    }

    // Zadanie zaleg³e
    public class StanZalegle : IStanZadania
    {
        // Zmiana stanu z Zaleg³e na Wykonane
        public void wykonane(Zadanie zadanie)
        {
            Console.WriteLine("Zmiana stanu zadania z zaleg³e na wykonane");
        }

        // Zmiana stanu z Zaleg³e na Aktywne
        public void aktywne(Zadanie zadanie)
        {
            Console.WriteLine("Zmiana stanu zadania z zaleg³e na aktywne");
        }

        // Ponowienie tego stanu
        public void zalegle(Zadanie zadanie)
        {
            Console.WriteLine("Zadanie pozostaje zaleg³e");
        }
    }




    // Klasa Mened¿er Zadañ (Wzorzec Singleton)
    public class MenedzerZadan
    {
        // Pola
        private static MenedzerZadan instancja;  // Singleton; instancja Fabryki Zadañ
        private FabrykaZadan fabryka;  // WskaŸnik na Fabrykê zadañ
        private List<Zadanie> zadania;  // Lista wszystkich Zadañ w programie
        public List<Zadanie> Zadania  // Publiczny getter
        {
            get { return zadania; }
        }
        private HashSet<int> IDZadan = new();  // HashSet unikalnych ID Zadañ. ID siê nie powtarzaj¹.


        // Prywatny Konstruktor
        private MenedzerZadan()
        {
            fabryka = new FabrykaZadan();
            zadania = new List<Zadanie>();
        }


        // Metody
        public static MenedzerZadan GetterInstancji()
        {
            if (instancja == null)
            {
                instancja = new MenedzerZadan();
            }
            return instancja;
        }


        // Utworzenie nowego Zadania poprzez Fabrykê 
        public void UtworzZadaniePrzezFabryke(string tytul, string tresc, Priorytet priorytet, DateTime termin, List<string> tagi = null)
        {
            // Lista stringów 'tagi' podana do metody zawiera nazwy Tagów, które powinny zostaæ przypisane do
            // nowo utworzonej notatki. Te tagi nie koniecznie istniej¹, wiêc zajmie siê tym Fabryka.

            // Utworzenie Zadania poprzez Fabrykê
            Zadanie zadanie = (Zadanie)fabryka.UtworzWpis(tytul, tresc, priorytet, termin, tagi);
            // Dodajemy nowe Zadanie do listy mened¿era
            zadania.Add(zadanie);
        }

        // Metoda wybieraj¹ca unikalne ID dla Zadania, zwraca to ID.
        public int WybierzIDZadania()
        {
            int id = 0;  // Nowe ID zaczyna odliczanie od 0
            // Pêtla od 0 w górê, przez HashSet ID, a¿ znajdziemy nieu¿yte ID.
            while (IDZadan.Contains(id))
            {
                id++;
            }

            // Dane ID nie jest u¿ywane, dodajemy je do HashSetu.
            IDZadan.Add(id);
            return id;
        }

        // Usuwa Zadanie z listy i wypisuje jego zawartoœæ
        public void UsunZadanie(Zadanie zadanie)
        {
            if (zadania.Remove(zadanie))
            {
                // Usuniêcie danej Notatki z Listy Notatek Tagów przypisanych do danej Notatki
                foreach (Tag tag in zadanie.tagi)
                {
                    tag.UsunWpis(zadanie);
                }

                Console.WriteLine("Usuniêto zadanie:");
                WypiszZadanie(zadanie);
            }
            else
            {
                Console.WriteLine("Nie znaleziono zadania do usuniêcia.");
            }
        }


        // Dodaje podany Tag do danego Zadania. Zwraca true jeœli dodanie zakoñczy³o siê sukcesem.
        public bool DodajTagDoZadania(Zadanie zadanie, Tag tag)
        {
            return zadanie.DodajTag(tag);
        }

        // Usuwa podany Tag z danego Zadania. Zwraca true jeœli usuniêcie zakoñczy³o siê sukcesem.
        public bool UsunTagZZadania(Zadanie zadanie, Tag tag)
        {
            return zadanie.UsunTag(tag);
        }


        // Wypisanie zawartoœci danego zadania
        public void WypiszZadanie(Zadanie zadanie)
        {
            if (zadanie != null)
            {
                Console.WriteLine(zadanie.WypiszInformacje());
            }
        }

        // Wyszukiwanie Zadania po ID
        public Zadanie WyszukajZadanie(int id)
        {
            foreach (Zadanie z in zadania)
            {
                if (z.id == id)
                {
                    return z;
                }
            }
            return null;
        }

        // Wyszukuje zadanie po podanej frazie
        public List<Zadanie> SzukajZadan(string fraza)
        {
            List<Zadanie> wynik = new List<Zadanie>();

            foreach (var z in zadania)
            {
                // zak³adamy, ¿e Wpis ma dostêp do tytulu (getter)
                if (z.WypiszInformacje().Contains(fraza))
                {
                    wynik.Add(z);
                }
            }

            return wynik;
        }
        public List<Zadanie> SzukajPoTerminach(DateTime od, DateTime doDaty)
        {
            List<Zadanie> wynik = new List<Zadanie>();

            foreach (Zadanie z in zadania)
            {
                if (z != null)
                {
                    if (z.SprawdzCzyZalegle() == false &&
                        z.ZwrocTermin() >= od &&
                        z.ZwrocTermin() <= doDaty)
                    {
                        wynik.Add(z);
                    }
                }
            }

            return wynik;
        }
        public void SortujZadaniaPoTerminach(List<Zadanie> lista)
        {
            lista.Sort(delegate (Zadanie a, Zadanie b)
            {
                return a.ZwrocTermin().CompareTo(b.ZwrocTermin());
            });
        }

        // Sortowanie listy zadañ po priorytecie malej¹co (wysoki priorytet pierwszy)
        public void SortujZadaniaPoPriorytecie(List<Zadanie> lista)
        {
            lista.Sort(delegate (Zadanie a, Zadanie b)
            {
                return b.Priorytet.CompareTo(a.Priorytet);
            });
        }



        // Oznacza Zadania z podanej Listy Zadañ jako wykonane
        public void OznaczZadaniaJakowykonane(List<Zadanie> lista)
        {
            foreach (var z in lista)
            {
                z.OznaczJakoWykonane();
            }
        }

        // Zwraca Listê Zadañ zaleg³ych
        public List<Zadanie> WybierzZalegle()
        {
            // Lista pomocnicza
            List<Zadanie> zalegle = new List<Zadanie>();

            // Dodanie zaleg³ych Zadañ do Listy pomocniczej
            foreach (var z in zadania)
            {
                if (z.SprawdzCzyZalegle())
                    zalegle.Add(z);
            }

            return zalegle;
        }

        // Wypisuje wszystkie Zadania
        public void WypiszZadania()
        {
            // Dzia³¹ tylko gdy Lista zadañ nie jest pusta
            if (zadania.Count > 0)
            {
                foreach (Zadanie z in zadania)
                {
                    WypiszZadanie(z);
                }
            }
        }
    }


    // Fabryka Zadañ
    public class FabrykaZadan : FabrykaWpisow
    {
        // Konstruktor
        public FabrykaZadan() : base() { }


        // Nadpisanie metody fabrykuj¹cej wpis
        public override Wpis UtworzWpis(string tytul, string tresc, Priorytet priorytet, DateTime termin, List<string> nazwyTagow)
        {
            List<Tag> tagi = null;
            if (nazwyTagow != null)
            {
                // Mened¿erTagów zajmuje siê znalezieniem i zwróceniem odpowiednich Tagów.
                tagi = new List<Tag>();  // Rzeczywista Lista Tagów, przekazywana do Zadania
                foreach (string nazwaTagu in nazwyTagow)
                {
                    // Pytamy Mened¿eraTagów o zwrócenie wskaŸnika na dany Tag.
                    // Jeœli zwróci 'null', dany Tag nie istnieje, wiêc go nie dodajemy
                    Tag tag = MenedzerTagow.GetterInstancji().ZwrocTag(nazwaTagu);
                    if (tag != null)
                    {
                        tagi.Add(tag);
                    }
                }
            }

            IStanZadania domyslnyStan = new StanAktywne();


            // W³aœciwe utworzenie Zadania na podstawie powy¿szych danych
            Zadanie zadanie = new Zadanie(tytul, tresc, domyslnyStan, priorytet, termin, tagi);
            // Dodanie Zadania do listy Zadañ wszystkich wybranych Tagów
            if (tagi != null)
            {
                foreach (Tag tag in tagi)
                {
                    tag.DodajWpis(zadanie);
                }
            }


            // Utworzenie i zwrócenie nowego Zadania 
            return zadanie;
        }

       
    }
} 