using System.Formats.Asn1;
using System.Runtime.CompilerServices;

public partial class Program
{
    // Klasa Notatka dziedziczy po klasie abstrakcyjnej Wpis
    // Reprezentuje pojedyncz¹ notatkê w systemie.
    public class Notatka : Wpis
    {


        public bool Ulubiona { get; private set; }// Okreœla, czy notatka jest oznaczona jako ulubiona
        public List<Tag> Tagi { get; set; }// Lista tagów przypisanych do notatki


        public Notatka(string tytul, string tresc, List<Tag> tagi)// Konstruktor klasy Notatka
            : base(tytul, tresc)
        {

            if (tagi != null)
                Tagi = tagi;
            else
                Tagi = new List<Tag>();

            Ulubiona = false; // domyœlnie notatka nie jest ulubiona
        }

        // Ustawia lub usuwa status ulubionej notatki
        public void UstawUlubione(bool ustawienie)
        {
            Ulubiona = ustawienie;
        }

        // Nadpisanie metody abstrakcyjnej WypiszInformacje() z klasy Wpis
        // Zwraca wszystkie informacje o notatce w formie stringa
        public override string WypiszInformacje()
        {
            string tagiStr;

            if (Tagi.Count > 0)
                tagiStr = string.Join(", ", Tagi.Select(t => t.Nazwa));
            else
                tagiStr = "Brak tagów";

            return $"ID: {ID}, Tytu³: {Tytul}, Treœæ: {Tresc}, Ulubiona: {Ulubiona}, Tagi: {tagiStr}";
        }

        // Nadpisanie ToString() dla wygodnego wypisywania notatki
        public override string ToString()
        {
            return WypiszInformacje();
        }
    }
}