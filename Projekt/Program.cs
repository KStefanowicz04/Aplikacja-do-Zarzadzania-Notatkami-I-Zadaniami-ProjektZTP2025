
// Program główny
// "partial class" oznacza jedynie, że kod całego programu jest porozsiewany po wielu plikach.
// Wszystkie pliki w projekcie powinny zaczynać się od "public partial class Program"
using static Program;

public partial class Program
{

    // Funkcja Main - wykonuje program
    static void Main(string[] args)
    {
        // Singletony
        MenedzerNotatek menedzerNotatek = MenedzerNotatek.GetterInstancji();
        MenedzerZadan menedzerZadan = MenedzerZadan.GetterInstancji();
        MenedzerTagow menedzerTagow = MenedzerTagow.GetterInstancji();

        // Builder i Director Zadań
        ZadanieBuilder zadanieBuilder = new ZadanieBuilder();
        ZadanieDirector zadanieDirector = new ZadanieDirector(zadanieBuilder);

        // Przykładowe tagi
        menedzerTagow.UtworzTag("Ain");
        menedzerTagow.UtworzTag("Betelgeuse");
        menedzerTagow.UtworzTag("Capella");

        // Przykładowe Notatki
        menedzerNotatek.UtworzNotatkePrzezFabryke("Zupełnie pusta notatka", "");
        menedzerNotatek.UtworzNotatkePrzezFabryke("Pusta notatka z tagami", "", new List<String>{ "Ain" });
        menedzerNotatek.UtworzNotatkePrzezFabryke("Lorem ipsum", "dolor sit amet", new List<String>{ "Ain", "Betelgeuse" });

        // Przykładowe Zadania
        menedzerZadan.UtworzZadaniePrzezFabryke("Zadanie z wysokim priorytetem bez Tagów", "",
            Priorytet.Wysoki, new DateTime(2026, 2, 12));
        menedzerZadan.UtworzZadaniePrzezFabryke("Zadanie z niskim pr. z Tagami", "Ma podany jeden nieistniejący domyślnie Tag",
            Priorytet.Niski, new DateTime(2021, 12, 28), new List<string>{ "Betelgeuse", "Capella", "Sirius" });
        // Przykładowe Zadanie utworzone przez Buildera i poprzez Directora
        zadanieBuilder.UstawTytul("Zadanie z Buildera")
            .UstawTresc("zawiera Tagi")
            .UstawPriorytet(Priorytet.Wysoki)
            .UstawTermin(DateTime.Now.AddDays(7))
            .UstawTagi(new List<string> { "Capella", "Sirius" })
            .Build();
        zadanieDirector.KonstruujZadanieDlugoterminowe("Zadanie z Directora", "(długoterminowe)");


        // Menu główne
        while (true)
        {
            Console.WriteLine("\n\nWybierz komendę (wpisz liczbę):\n1: Wypisz notatki\n2: Wypisz zadania\n3: Wypisz tagi");
            Console.WriteLine("4: Dodaj nowy tag\n5. Usuń tag\n6. Dodaj Tag do Notatki\n7. Dodaj Tag do Zadania");
            Console.WriteLine("8. Usuń Tag z Notatki\n9. Usuń Tag z Zadania");
            Console.WriteLine("10. Dodaj nową notatkę\n11. Usuń notatkę");
            Console.WriteLine("12. Dodaj nowe zadanie przez Fabrykę\n13. Usuń zadanie");
            Console.WriteLine("14. Wypisz Notatki wraz z ich tagami\n15. Wypisz Zadania wraz z ich tagami");
            Console.WriteLine("16. Wypisz Zadania wraz z ich obecnym stanem");
            Console.WriteLine("17. Zmień stan wybranego Zadania");
            Console.WriteLine("18. Wyszukaj ");
            string command = Console.ReadLine();  // Odczytuje komendę z klawiatury.

            switch (command)
            {
                // Wypisanie Notatek
                case "1":
                    menedzerNotatek.WypiszNotatki();

                    break;

                // Wypisanie Zadań
                case "2":
                    menedzerZadan.WypiszZadania();

                    break;

                // Wypisanie Tagów
                case "3":
                    menedzerTagow.Wypisztagi();

                    break;

                
                // Dodanie nowego Tagu
                case "4":
                    Console.WriteLine("Podaj nazwę nowego Tagu do utworzenia:");
                    command = Console.ReadLine();

                    Console.WriteLine($"Dodanie tagu o nazwie {command}: {menedzerTagow.UtworzTag(command)}.");

                    break;

                // Usunięcie danego tagu
                case "5":
                    Console.WriteLine("Podaj nazwę Tagu do usunięcia:");
                    command = Console.ReadLine();

                    menedzerTagow.UsunTag(command);

                    break;

                // Dodaj Tag do Notatki
                case "6":
                    Console.WriteLine("Podaj nazwę Tagu do dodania (wpisanie nieistniejącego Tagu utworzy nowy Tag):");
                    command = Console.ReadLine();

                    // Zmienna pomocnicza przechowująca wskaźnik na dany Tag
                    Tag tag = menedzerTagow.ZwrocTag(command);
                    // Utworzenie nowego Tagu jeśli nie istnieje taki o podanej nazwie
                    if (tag == null)
                    {
                        tag = menedzerTagow.UtworzTag(command);
                    }

                    Console.WriteLine("Podaj ID Notatki, do której zostanie dodany ten Tag:");
                    command = Console.ReadLine();

                    // Podane ID musi być liczbą
                    if (int.TryParse(command, out _) == false)
                    {
                        Console.WriteLine("Podano niewłaściwe ID");
                        break;
                    }

                    // Próba znalezienia Notatki
                    Notatka notatka = menedzerNotatek.WyszukajNotatke(int.Parse(command));
                    if (notatka == null)
                    {
                        Console.WriteLine("Notatka o danym ID nie istnieje");
                        break;
                    }

                    // Właściwe dodanie podanego Tagu do Notatki
                    bool wynik = menedzerNotatek.DodajTagDoNotatki(notatka, tag);

                    Console.WriteLine($"Dodanie tagu \"{tag.nazwa}\" do Notatki o ID={notatka.id} zakończyło się: {wynik}");

                    break;

                // Dodaj Tag do Zadania
                case "7":
                    Console.WriteLine("Podaj nazwę Tagu do dodania (wpisanie nieistniejącego Tagu utworzy nowy Tag):");
                    command = Console.ReadLine();

                    // Zmienna pomocnicza przechowująca wskaźnik na dany Tag
                    tag = menedzerTagow.ZwrocTag(command);
                    // Utworzenie nowego Tagu jeśli nie istnieje taki o podanej nazwie
                    if (tag == null)
                    {
                        tag = menedzerTagow.UtworzTag(command);
                    }

                    Console.WriteLine("Podaj ID Zadania, do którego zostanie dodany ten Tag:");
                    command = Console.ReadLine();

                    // Podane ID musi być liczbą
                    if (int.TryParse(command, out _) == false)
                    {
                        Console.WriteLine("Podano niewłaściwe ID");
                        break;
                    }

                    // Próba znalezienia Zadania
                    Zadanie zadanie = menedzerZadan.WyszukajZadanie(int.Parse(command));
                    if (zadanie == null)
                    {
                        Console.WriteLine("Zadanie o danym ID nie istnieje");
                        break;
                    }

                    // Właściwe dodanie podanego Tagu do Notatki
                    wynik = menedzerZadan.DodajTagDoZadania(zadanie, tag);

                    Console.WriteLine($"Dodanie tagu \"{tag.nazwa}\" do Zadania o ID={zadanie.id} zakończyło się: {wynik}");

                    break;

                // Usuń Tag z Notatki
                case "8":
                    Console.WriteLine("Podaj nazwę Tagu do usunięcia:");
                    command = Console.ReadLine();

                    // Zmienna pomocnicza przechowująca wskaźnik na dany Tag
                    tag = menedzerTagow.ZwrocTag(command);
                    // Utworzenie nowego Tagu jeśli nie istnieje taki o podanej nazwie
                    if (tag == null)
                    {
                        Console.WriteLine("Dany Tag nie istnieje!");
                        break;
                    }

                    Console.WriteLine("Podaj ID Notatki, z której zostanie usunięty ten Tag:");
                    command = Console.ReadLine();

                    // Podane ID musi być liczbą
                    if (int.TryParse(command, out _) == false)
                    {
                        Console.WriteLine("Podano niewłaściwe ID");
                        break;
                    }

                    // Próba znalezienia Notatki
                    notatka = menedzerNotatek.WyszukajNotatke(int.Parse(command));
                    if (notatka == null)
                    {
                        Console.WriteLine("Notatka o danym ID nie istnieje");
                        break;
                    }

                    // Właściwe usunięcie podanego Tagu z Notatki
                    wynik = menedzerNotatek.UsunTagZNotatki(notatka, tag);

                    Console.WriteLine($"Usunięcie tagu \"{tag.nazwa}\" z Notatki o ID={notatka.id} zakończyło się: {wynik}");

                    break;

                // Usuń Tag z Zadania
                case "9":
                    Console.WriteLine("Podaj nazwę Tagu do usunięcia:");
                    command = Console.ReadLine();

                    // Zmienna pomocnicza przechowująca wskaźnik na dany Tag
                    tag = menedzerTagow.ZwrocTag(command);
                    // Utworzenie nowego Tagu jeśli nie istnieje taki o podanej nazwie
                    if (tag == null)
                    {
                        Console.WriteLine("Dany Tag nie istnieje!");
                        break;
                    }

                    Console.WriteLine("Podaj ID Zadania, z którego zostanie usunięty ten Tag:");
                    command = Console.ReadLine();

                    // Podane ID musi być liczbą
                    if (int.TryParse(command, out _) == false)
                    {
                        Console.WriteLine("Podano niewłaściwe ID");
                        break;
                    }

                    // Próba znalezienia Zadania
                    zadanie = menedzerZadan.WyszukajZadanie(int.Parse(command));
                    if (zadanie == null)
                    {
                        Console.WriteLine("Zadanie o danym ID nie istnieje");
                        break;
                    }

                    // Właściwe usunięcie podanego Tagu z Notatki
                    wynik = menedzerZadan.UsunTagZZadania(zadanie, tag);

                    Console.WriteLine($"Usunięcie tagu \"{tag.nazwa}\" z Zadania o ID={zadanie.id} zakończyło się: {wynik}");

                    break;

                // Dodanie nowej notatki
                case "10":
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
                case "11":
                    Console.WriteLine("Podaj ID Notatki do usunięcia:");
                    command = Console.ReadLine();

                    // UWAGA: Ten kod tutaj należyrozbudować i użyć TryParse,
                    // bo może być że ktoś poda znak inny niż cyfra do 'command'
                    menedzerNotatek.UsunNotatke(menedzerNotatek.WyszukajNotatke(Int32.Parse(command)));

                    break;

                // Dodanie nowego zadania przez Fabrykę
                case "12":
                    Console.WriteLine("Podaj tytuł nowego Zadania:");
                    tytul = Console.ReadLine();
                    Console.WriteLine("Podaj treść nowego Zadania:");
                    tresc = Console.ReadLine();

                    // Wybranie priorytetu danego Zadania
                    Console.WriteLine("Wybierz priorytet nowego Zadania (wpisz liczbę):\n1. Niski\n2. Średni\n3. Wysoki\n");
                    string prioString = Console.ReadLine();
                    Priorytet prio;

                    if (prioString == "1") prio = Priorytet.Niski;
                    else if (prioString == "2") prio = Priorytet.Sredni;
                    else if (prioString == "3") prio = Priorytet.Wysoki;
                    // Podano niepoprawną wartość priorytetu
                    else
                    {
                        Console.WriteLine("Podano niepoprawną wartość.");
                        break;
                    }

                    // Wybranie terminu Zadania
                    Console.WriteLine("Podaj termin wykonania Zadania (format: \"rok, miesiąc, dzień\"):");
                    string dataTerminString = Console.ReadLine();
                    DateTime dataTermin = DateTime.Parse(dataTerminString);


                    // Pętla dodawania tagów
                    listaTagowNotatki = new List<string>();  // Pomocnicza lista nazw tagów dla danej Notatki
                    dodawanieTagow = true;
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
                    menedzerZadan.UtworzZadaniePrzezFabryke(tytul, tresc, prio, dataTermin, listaTagowNotatki);

                    break;

                // Usunięcie danego Zadania
                case "13":
                    Console.WriteLine("Podaj ID Zadania do usunięcia:");
                    command = Console.ReadLine();

                    // UWAGA: Ten kod tutaj należy rozbudować i użyć TryParse,
                    // bo może być że ktoś poda znak inny niż cyfra do 'command'
                    menedzerZadan.UsunZadanie(menedzerZadan.WyszukajZadanie(Int32.Parse(command)));

                    break;

                // Wypisz Notatki wraz z ich tagami
                case "14":
                    // Pętla przez wszystkie Notatki w MenedzerNotatek
                    foreach (Notatka not in menedzerNotatek.Notatki)
                    {
                        // DekoratorTagowy danej Notatki
                        DekoratorWpisow deko = new DekoratorTagowy(not);
                        // Wywołanie metody Dekoratora, który dodaje informacje o tagach danej Notatki do WypiszInformacje()
                        Console.WriteLine(deko.WypiszInformacje());
                    }

                    break;

                // Wypisz Zadania wraz z jego tagami
                case "15":
                    // Pętla przez wszystkie Zadania w MenedzerZadan
                    foreach (Zadanie zada in menedzerZadan.Zadania)
                    {
                        // DekoratorTagowy danego Zadania
                        DekoratorWpisow deko = new DekoratorTagowy(zada);
                        // Wywołanie metody Dekoratora, który dodaje informacje o tagach danego Zadania do WypiszInformacje()
                        Console.WriteLine(deko.WypiszInformacje());
                    }

                    break;

                // Wypisz Zadania z ich stanami
                case "16":
                    // Pętla przez wszystkie Zadania w MenedzerZadan
                    foreach (Zadanie zada in menedzerZadan.Zadania)
                    {
                        // DekoratorStanowy dla danego Zadania
                        DekoratorStanowy deko = new DekoratorStanowy(zada);
                        // Wywołanie metody Dekoratora, który dodaje informacje o stanie danego Zadania do WypiszInformacje()
                        Console.WriteLine(deko.WypiszInformacje());
                    }

                    break;

                // Zmień stan wybranego Zadania
                case "17":
                    Console.WriteLine("Podaj ID Zadania, którego stan zostanie zmieniony:");
                    command = Console.ReadLine();

                    // Podane ID musi być liczbą
                    if (int.TryParse(command, out _) == false)
                    {
                        Console.WriteLine("Podano niewłaściwe ID");
                        break;
                    }

                    // Próba znalezienia Zadania
                    zadanie = menedzerZadan.WyszukajZadanie(int.Parse(command));
                    if (zadanie == null)
                    {
                        Console.WriteLine("Zadanie o danym ID nie istnieje");
                        break;
                    }


                    // Wypisanie informacji o danym Zadaniu, w tym o jego Stanie poprzez DekoratorStanowy
                    Console.WriteLine("Obecny stan danego Zadania: ");
                    DekoratorStanowy dekor = new DekoratorStanowy(zadanie);
                    Console.WriteLine(dekor.WypiszInformacje());

                    Console.WriteLine("\nWybierz nowy Stan danego Zadania:\n1. Wykonane\n2. Aktywne\n3. Zaległe");
                    command = Console.ReadLine();

                    // Wybranie odpowiedniej metody
                    if (command == "1") zadanie.OznaczJakoWykonane();
                    else if (command == "2") zadanie.OznaczJakoAktywne();
                    else if (command == "3") zadanie.OznaczJakoZalegle();
                    // Podano niepoprawną wartość
                    else
                    {
                        Console.WriteLine("Podano niepoprawną wartość.");
                        break;
                    }


                    break;

                // Wyszukiwanie
                case "18":
                    Console.WriteLine("WYSZUKIWANIE NIE MA IMPLEMENTACJI W MENU GŁÓWNYM");
                    break;

                // W przypadku podania nieodpowiedniej komendy, należy wpisać coś innego.
                default:
                    Console.WriteLine("Podaj poprawną komendę");
                    break;
            }
        }
    }
}