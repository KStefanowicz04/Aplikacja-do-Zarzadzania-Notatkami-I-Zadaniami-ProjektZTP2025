using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Linq;
using System.Runtime.CompilerServices;
using static Program;

public partial class Program
{
    public enum Priorytet
    {
        Niski,
        Sredni,
        Wysoki
    }

    // Klasa Zadanie, pochodna od Wpis
    public class Zadanie : Wpis
    {
        // Pola
        private IStanZadania stan;  // Obecny stan zadania (wykonane/aktywne/zaległe)
        private Priorytet priorytet;  // Obecny priorytet zadania
        private DateTime termin;  // Termin wykonania zadania

        // Gettery, settery
        public IStanZadania Stan
        {
            get { return stan; }
            set { stan = value; }
        }
        public Priorytet PriorytetZadania
        {
            get { return priorytet; }
            set { priorytet = value; }
        }
        public DateTime Termin
        {
            get { return termin; }
            set { termin = value; }
        }



        // Konstruktor
        public Zadanie(string tytul, string tresc, IStanZadania stan, Priorytet priorytet, DateTime termin, List<Tag> tagi) : base(tytul, tresc)
        {
            // Manager zadań decyduje jaki numer ID zostanie przypisany do danego Zadania
            id = MenedzerZadan.GetterInstancji().WybierzIDZadania();

            // Domyślnie stan jest Aktywny - może należy to usunąć z parametrów Konstruktora?
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
        // Pusty Konstruktor używany przez BuilderaZadań
        public Zadanie() : base("", "")
        {
            // Manager zadań decyduje jaki numer ID zostanie przypisany do danego Zadania
            id = MenedzerZadan.GetterInstancji().WybierzIDZadania();
        }


        // Metody

        // Getter priorytetu
        public Priorytet Priorytet
        {
            get { return priorytet; }
        }
        // Getter temrminu Zadania
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


        // Zwraca nazwę obecnego stanu Zadania w formie string
        public string ZwrocStan()
        {
            return stan.GetType().Name;
        }

        // Zmienia obecny stan zadania na ten podany jako parametr
        public void ZmienStan(IStanZadania stan)
        {
            this.stan = stan;
        }

        // Prosi obecny stan o zmianę stanu zadania na Wykonane
        public void OznaczJakoWykonane()
        {
            stan.wykonane(this);
        }

        // Prosi obecny stan o zmianę stanu zadania na Aktywne
        public void OznaczJakoAktywne()
        {
            stan.aktywne(this);
        }

        // Prosi obecny stan o zmianę stanu zadania na Zaległe
        public void OznaczJakoZalegle()
        {
            stan.zalegle(this);
        }


        // Zwraca true/false zależnie od obecnego stanu zadania;
        // jeśli Zadanie zostało wykonane - false; jeśli nie, porównuje obecny czas z terminem Zadania
        public bool SprawdzCzyZalegle()
        {
            // Sprawdzamy czy obecny stan jest stanem Wykonane
            if (stan.GetType() == typeof(StanWykonane))
                return false;

            // Porównuje czas obecny z wyznaczonym terminem zadania
            return DateTime.Now > termin;
        }


        // Wypisuje podstawowe informacje o zadaniu
        public override string WypiszInformacje()
        {
            return $"[ZADANIE] ID: {id} | Tytuł: {tytul} | Treść: {tresc} | Priorytet: {priorytet} | Termin: {termin:d}";
        }

        // Nadpisanie ToString() dla wygodnego wypisywania Zadania
        public override string ToString()
        {
            return WypiszInformacje();
        }
    }



    // Stany Zadań
    //
    // Interfejs stanów Zadania (Wzorzec State)
    public interface IStanZadania
    {
        // Metody wykorzysywane przez każdy Stan
        public void wykonane(Zadanie zadanie);
        public void aktywne(Zadanie zadanie);
        public void zalegle(Zadanie zadanie);
    }

    // Konkretne stany Zadania
    //
    // Zadanie wykonane
    public class StanWykonane : IStanZadania
    {
        // Ponowienie tego stanu
        public void wykonane(Zadanie zadanie)
        {
            Console.WriteLine("Zadanie już jest oznaczone jako Wykonane!");
        }

        // Zmiana stanu z Wykonane na Aktywne
        public void aktywne(Zadanie zadanie)
        {
            Console.WriteLine("Zadanie zostało Wykonane, nie można zmienić go spowrotem na Aktywne.");
        }

        // Zmiana stanu z Wykonane na Zaległe
        public void zalegle(Zadanie zadanie)
        {
            Console.WriteLine("Zadanie zostało Wykonane, nie można zmienić go na Zaległe.");
        }
    }

    // Zadanie aktywne
    public class StanAktywne : IStanZadania
    {
        // Zmiana stanu z Aktywne na Wykonane
        public void wykonane(Zadanie zadanie)
        {
            Console.WriteLine("Zmieniono stan Zadania z Aktywnego na Wykonane.");
            zadanie.ZmienStan(new StanWykonane());
        }

        // Ponowienie tego stanu
        public void aktywne(Zadanie zadanie)
        {
            Console.WriteLine("Zadanie już jest oznaczone jako Aktywne!");
        }

        // Zmiana stanu z Aktywne na Zaległe
        public void zalegle(Zadanie zadanie)
        {
            Console.WriteLine("Zmieniono stan Zadania z Aktywnego na Zaległe.");
            zadanie.ZmienStan(new StanZalegle());
        }
    }

    // Zadanie zaległe
    public class StanZalegle : IStanZadania
    {
        // Zmiana stanu z Zaległe na Wykonane
        public void wykonane(Zadanie zadanie)
        {
            Console.WriteLine("Zmieniono stan Zadania z Zaległego na Wykonane.");
            zadanie.ZmienStan(new StanWykonane());
        }

        // Zmiana stanu z Zaległe na Aktywne
        public void aktywne(Zadanie zadanie)
        {
            // Zadanie Zaległe można zmienić na Aktywne tylko jeśli jego termin nie upłynął
            // (np. w przypadku gdy Zadanie zostanie oznaczone jako Zaległe, ale jego termin zostanie zmieniony i ma
            // ono zostać oznaczone jako Aktywne)
            if (zadanie.SprawdzCzyZalegle())
            {
                Console.WriteLine("Zmieniono stan Zadania z Zaległego na Aktywne.");
                zadanie.ZmienStan(new StanAktywne());
            }
            else
            {
                Console.WriteLine("Zadanie wciąż jest Zaległe, nie można zmienić go spowrotem na Aktywne, chyba że zmieni się termin wykonania Zadania.");
            }
        }

        // Ponowienie tego stanu
        public void zalegle(Zadanie zadanie)
        {
            Console.WriteLine("Zadanie już jest oznaczone jako Zaległe!");
        }
    }




    // Klasa Menedżer Zadań (Wzorzec Singleton)
    public class MenedzerZadan
    {
        // Pola
        private static MenedzerZadan instancja;  // Singleton; instancja Fabryki Zadań
        private FabrykaZadan fabryka;  // Wskaźnik na Fabrykę zadań
        private List<Zadanie> zadania;  // Lista wszystkich Zadań w programie
        public List<Zadanie> Zadania  // Publiczny getter
        {
            get { return zadania; }
        }
        private HashSet<int> IDZadan = new();  // HashSet unikalnych ID Zadań. ID się nie powtarzają.
        
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


        // Utworzenie nowego Zadania poprzez Fabrykę 
        public void UtworzZadaniePrzezFabryke(string tytul, string tresc, Priorytet priorytet, DateTime termin, List<string> tagi = null)
        {
            // Lista stringów 'tagi' podana do metody zawiera nazwy Tagów, które powinny zostać przypisane do
            // nowo utworzonej notatki. Te tagi nie koniecznie istnieją, więc zajmie się tym Fabryka.

            // Utworzenie Zadania poprzez Fabrykę
            Zadanie zadanie = (Zadanie)fabryka.UtworzWpis(tytul, tresc, priorytet, termin, tagi);
            // Dodajemy nowe Zadanie do listy menedżera
            zadania.Add(zadanie);
        }

        // Metoda wybierająca unikalne ID dla Zadania, zwraca to ID.
        public int WybierzIDZadania()
        {
            int id = 0;  // Nowe ID zaczyna odliczanie od 0
            // Pętla od 0 w górę, przez HashSet ID, aż znajdziemy nieużyte ID.
            while (IDZadan.Contains(id))
            {
                id++;
            }

            // Dane ID nie jest używane, dodajemy je do HashSetu.
            IDZadan.Add(id);
            return id;
        }

        // Zadania utworzone przez Buildera muszą zostać dodane do Listy Zadań poprzez tę Metodę
        public void DodajZadanie(Zadanie zadanie)
        {
            // Jeśli dane Zadanie już występuje w Liście (co nie powinno nigdy się wydarzyć), nie zostanie dodane.
            if (zadania.Contains(zadanie))
            {
                Console.WriteLine("Dane Zadanie już występuje w Liście Zadań MenedżeraZadań");
            }
            else
            {
                zadania.Add(zadanie);
            }
        }

        // Usuwa Zadanie z listy i wypisuje jego zawartość
        public void UsunZadanie(Zadanie zadanie)
        {
            if (zadania.Remove(zadanie))
            {
                // Usunięcie danej Notatki z Listy Notatek Tagów przypisanych do danej Notatki
                foreach (Tag tag in zadanie.tagi)
                {
                    tag.UsunWpis(zadanie);
                }

                Console.WriteLine("Usunięto zadanie:");
                WypiszZadanie(zadanie);
            }
            else
            {
                Console.WriteLine("Nie znaleziono zadania do usunięcia.");
            }
        }


        // Dodaje podany Tag do danego Zadania. Zwraca true jeśli dodanie zakończyło się sukcesem.
        public bool DodajTagDoZadania(Zadanie zadanie, Tag tag)
        {
            return zadanie.DodajTag(tag);
        }

        // Usuwa podany Tag z danego Zadania. Zwraca true jeśli usunięcie zakończyło się sukcesem.
        public bool UsunTagZZadania(Zadanie zadanie, Tag tag)
        {
            return zadanie.UsunTag(tag);
        }


        // Wypisanie zawartości danego zadania
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
                // zakładamy, że Wpis ma dostęp do tytulu (getter)
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

        // Sortowanie listy zadań po priorytecie malejąco (wysoki priorytet pierwszy)
        public void SortujZadaniaPoPriorytecie(List<Zadanie> lista)
        {
            lista.Sort(delegate (Zadanie a, Zadanie b)
            {
                return b.Priorytet.CompareTo(a.Priorytet);
            });
        }



        // Oznacza Zadania z podanej Listy Zadań jako wykonane
        public void OznaczZadaniaJakowykonane(List<Zadanie> lista)
        {
            foreach (var z in lista)
            {
                z.OznaczJakoWykonane();
            }
        }

        // Zwraca Listę Zadań zaległych
        public List<Zadanie> WybierzZalegle()
        {
            // Lista pomocnicza
            List<Zadanie> zalegle = new List<Zadanie>();

            // Dodanie zaległych Zadań do Listy pomocniczej
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
            // Działą tylko gdy Lista zadań nie jest pusta
            if (zadania.Count > 0)
            {
                foreach (Zadanie z in zadania)
                {
                    WypiszZadanie(z);
                }
            }
        }
    }


    // Fabryka Zadań
    public class FabrykaZadan : FabrykaWpisow
    {
        // Konstruktor
        public FabrykaZadan() : base() { }


        // Nadpisanie metody fabrykującej wpis
        public override Wpis UtworzWpis(string tytul, string tresc, Priorytet priorytet, DateTime termin, List<string> nazwyTagow)
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

            IStanZadania domyslnyStan = new StanAktywne();


            // Właściwe utworzenie Zadania na podstawie powyższych danych
            Zadanie zadanie = new Zadanie(tytul, tresc, domyslnyStan, priorytet, termin, tagi);
            // Dodanie Zadania do listy Zadań wszystkich wybranych Tagów
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
