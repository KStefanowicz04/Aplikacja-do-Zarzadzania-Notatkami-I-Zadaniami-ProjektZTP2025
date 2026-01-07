
// Program główny
// "partial class" oznacza jedynie, że kod całego programu jest porozsiewany po wielu plikach.
// Wszystkie pliki w projekcie powinny zaczynać się od "public partial class Program"
public partial class Program
{

    // Funkcja Main - wykonuje program
    static void Main(string[] args)
    {
        // Singletony
        MenedzerNotatek menedzerNotatek = MenedzerNotatek.GetterInstancji();
        MenedzerZadan menedzerZadan = MenedzerZadan.GetterInstancji();
        MenedzerTagow menedzerTagow = MenedzerTagow.GetterInstancji();

        // Przykładowe Tagi
        menedzerTagow.UtworzTag("Ain");
        menedzerTagow.UtworzTag("Betelgeuse");
        menedzerTagow.UtworzTag("Capella");

        // Przykładowe Notatki
        menedzerNotatek.UtworzNotatkePrzezFabryke("Zupełnie pusta notatka", "");
        menedzerNotatek.UtworzNotatkePrzezFabryke("Pusta notatka z tagami", "", new List<String> { "Ain" });
        menedzerNotatek.UtworzNotatkePrzezFabryke("Lorem ipsum", "dolor sit amet", new List<String> { "Ain", "Betelgeuse" });\

        // Przykładowe Zadania


        // Menu główne
        while (true)
        {
            Console.WriteLine("\n\nWybierz komendę (wpisz liczbę):\n1: Wypisz notatki\n2: Wypisz tagi\n3: Dodaj nowy tag");
            Console.WriteLine("4. Usuń tag\n5. Dodaj nową notatkę\n6. Usuń notatkę");
            string command = Console.ReadLine();  // Odczytuje komendę z klawiatury.

            switch (command)
            {
                // Wypisanie notatek
                case "1":
                    menedzerNotatek.WypiszWszystkieNotatki();

                    break;

                // Wypisanie tagów
                case "2":
                    menedzerTagow.WypiszTagi();

                    break;

                // Dodanie nowego Tagu
                case "3":
                    Console.WriteLine("Podaj nazwę nowego Tagu do utworzenia:");
                    command = Console.ReadLine();

                    Console.WriteLine($"Dodanie tagu o nazwie {command}: {menedzerTagow.UtworzTag(command)}.");

                    break;

                // Usunięcie danego tagu
                case "4":
                    Console.WriteLine("Podaj nazwę Tagu do usunięcia:");
                    command = Console.ReadLine();

                    menedzerTagow.UsunTag(command);

                    break;

                // Dodanie nowej notatki
                case "5":
                    Console.WriteLine("Podaj tytuł nowej Notatki:");
                    string tytul = Console.ReadLine();
                    Console.WriteLine("Podaj treść nowej Notatki:");
                    string tresc = Console.ReadLine();

                    // Pętla dodawania tagów
                    List<string> listaTagowNotatki = new List<string>();  // Pomocnicza lista nazw tagów dla danej Notatki
                    bool dodawanieTagow = true;
                    while (dodawanieTagow == true)
                    {
                        Console.WriteLine("Podaj Tag nowej Notatki (wstaw pusty znak żeby przerwać):");
                        string nazwaTagu = Console.ReadLine();

                        // Pusty znak przerywa dodawanie tagów
                        if (nazwaTagu == "") break;
                        // Jeśli nie podano pustego znaku, dodajemy kolejny Tag
                        else
                        {
                            menedzerTagow.UtworzTag(nazwaTagu);  // Utworzenie danego Tagu jeśli jeszcze nie istnieje
                            listaTagowNotatki.Add(nazwaTagu);  // Dodanie danego Tagu do Listy pomocniczej
                        }
                    }
                    
                    // Utworzenie Notatki na podstawie powyższych danych.
                    menedzerNotatek.UtworzNotatkePrzezFabryke(tytul, tresc, listaTagowNotatki);

                    break;

                // Usunięcie danej Notatki
                case "6":
                    Console.WriteLine("Podaj ID Notatki do usunięcia:");
                    command = Console.ReadLine();

                    // UWAGA: Ten kod tutaj należyrozbudować i użyć TryParse,
                    // bo może być że ktoś poda znak inny niż cyfra do 'command'
                    menedzerNotatek.UsunNotatke(menedzerNotatek.WyszukajNotatki(Int32.Parse(command)));

                    break;

                // W przypadku podania nieodpowiedniej komendy, należy wpisać coś innego.
                default:
                    Console.WriteLine("Podaj poprawną komendę");
                    break;
            }
        }
    }
}