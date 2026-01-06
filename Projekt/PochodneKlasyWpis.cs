using System.Formats.Asn1;
using System.Runtime.CompilerServices;

public partial class Program
{
    // Interfejs klasy Wpis
    public interface IWpis
    {
        // Funkcja wypisująca informacje o danym Wpisie (Notatce lub Zadaniu).
        // Treść funkcji i wyniku zależy od klas pochodnych.
        protected abstract string WypiszInformacje();
    }

    // Klasa abstrakcyjna Wpis - służy tylko do bycia bazą pod inne klasy
    public abstract class Wpis : IWpis
    {
        // Pola klasy Wpis
        // id danego wpisu (generowane przy konstrukcji przez klasy pochodne Notatka i Zadanie, zapisywane w menedżerach)
        public int id { get; }
        public string tytul { get; set; }  // tytuł danego wpisu
        public string tresc { get; set; }  // treść danego wpisu
        protected DateTime dataUtworzenia;  // data utworzenia wpisu
        protected DateTime dataModyfikacji;  // data ostatniej edycji wpisu
        protected List<Tag> tagi;  // lista tagów przypisanych do danego wpisu


        // Konstruktor
        protected Wpis(string tytul, string tresc) : base()
        {
            this.tytul = tytul;  // Podany tytuł
            this.tresc = tresc;  // Podana treść
            dataUtworzenia = DateTime.Now;  // Czas utworzenia
        }

        // Zmienia dane wpisu na te podane w parametrach metody
        protected void Edytuj(string tytul, string tresc)
        {
            this.tytul = tytul;
            this.tresc = tresc;
        }

        // Ta sama metoda co powyżej, ale z dodatkowymi parametrami dostępnymi tylko dla klasy Zadanie
        public virtual void Edytuj(string tytul, string tresc, Priorytet priorytet, DateTime termin)
        {
            // Wywołanie zwykłej metody Edytuj.
            Edytuj(tytul, tresc);

            // Tu powinien znajdować się kod dla klasy Zadanie;
            // należy wstawić go w pliku z klasą Zadanie, NIE tutaj w Wpis
        }


        // Dodaje podany jako parametr 'tag' do Listy tagów 'tagi'
        protected void DodajTag(Tag tag)
        {
            tagi.Add(tag);
        }

        // Usuwa podany jako parametr 'tag' z Listy tagów 'tagi'
        protected void UsunTag(Tag tag)
        {
            tagi.Remove(tag);
        }

        // Abstrakcyjna metoda pochodząca od interfejsu IWpis;
        // Nic nie zwraca, metoda powinna zostać nadpisana przez klasy pochodne.
        public abstract string WypiszInformacje();
    }



    // Abstrakcyjna klasa fabryka wpisów; pochodzą z niej klasy FabrykaNotatek i FabrykaZadań
    public abstract class FabrykaWpisow
    {
        // Konstruktor (pusty)
        public FabrykaWpisow()
        {

        }


        // Metody
        //
        // Metoda fabrykująca wpis (specyficzna dla Notatki)
        // Tutaj metoda jest pusta, zawartość powinna być nadpisana przez pochodną fabrykę.
        public virtual Wpis UtworzWpis(string tytul, string tresc, List<Tag> tagi)
        {
            return null;
        }

        // Metoda fabrykująca wpis (specyficzna dla Zadań)
        // Tutaj metoda jest pusta, zawartość powinna być nadpisana przez pochodną fabrykę.
        public virtual Wpis UtworzWpis(string tytul, string tresc, Priorytet priorytet, DateTime termin, List<Tag> tagi)
        {
            return null;
        }
    }
}