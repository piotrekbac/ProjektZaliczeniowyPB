# ProjektZaliczeniowyPB - prosta aplikacja desktopowa umożliwiająca dostęp oraz możliwość zarządzania bazą danych. 
# Autor: Piotr Bacior - 15 722 WSEI Kraków - Informatyka stosowana - semestr  2


### Opis projektu ### 

Aplikacja desktopowa WPF umożliwiająca kompleksowe zarządzanie bazą danych sprzedaży samochodów. Projekt został zrealizowany indywidualnie z wykorzystaniem Entity Framework, MS SQL Server LocalDB 
oraz technologii WPF/XAML.


### Funkcjonalności projektu ### 

- Obsługa pracowników, zakupów, polis oraz pojazdów
- Możliwość wyszukiwania, edytowania, dodawania, jak i również usuwania danych (pełny CRUD)
- Estetyczny oraz responyswny interfejs graficzny
- Zarzadzanie danymi klientów
- Instalator aplikacji oraz testy jednostkowe
- Dokumentacja XML do publicznych składników naszego kodu


### Technologia użyta w projekcie ### 

- .NET Framework Wersja 4.7.2
- C# wersja 8.0
- WPF/XAML Visual Studio 2022 
- MS SQL Server LocalDB
- MSTest


### Struktura bazy danych ### 

- Tabela Klienci: Imię, nazwisko, kontakt, adres 
- Tabela Pracownicy: Dane personalne, przypisany wydział 
- Tabela Samochody: Model, wersja, numer seryjny 
- Tabela Zakupy: Połaczenie klienta, samochodu oraz pracownika 
- Tabela Polisy: Dane polisy, okres ważności 
- Tabela Wydziały: Nazwy działu pracowników 


### Interfejs użytkownika ###

Aplikacja zawiera następujące okna:
Każde z tych okien obsługuje pełną funkcjonalność CRUD i są ujednolicone stylistycznie.  

- KlienciWindow.xaml
- PracownicyWindow.xaml
- SamochodyWindow.xaml
- ZakupyWindow.xaml
- PolisyWindow.xaml


### Proces uruchamiania aplikacji ###

1. Z kodu źródłowego
- Otwórz aplikację w Visual Studio 2022
- Jeżeli to potrzebne, przywróć paczki NuGet
- Zbuduj projekt i go uruchom (możesz to zrobić za pośrednictwem klawisza F5)

2. Z instalatora:
- Przejdź do folderu: InstalatorAplikacjiPB\InstalatorAplikacjiPB\Debug\InstalatorAplikacjiPB.msi
- Uruchom plik: InstalatorAplikacjiPB.msi
- Zainstaluj aplikację i uruchom skrót z pulpitu


### Testy jednostkowe ### 

W projekcie znajduje się osobny projekt testowy, który zawiera: 
- Testy metody oraz logiki aplikacji (dodawanie, usuwanie oraz wszelkiego rodzaju modyfikacje)
- Testy pisałem w MSTest


### Dokumentacja techniczna ### 

- Komentarze XML znajdują sie w kodzie publicznych metod oraz klas

### FAQ – Najczęściej zadawane pytania ### 

Jak uruchomić aplikację bez Visual Studio?
- Odpowiedź: Skorzystaj z instalatora InstalatorAplikacjiPB.msi, znajdującego się w katalogu InstalatorAplikacjiPB\InstalatorAplikacjiPB\Debug.
  Po zainstalowaniu aplikacja będzie dostępna w menu Start lub na pulpicie (jeśli dodano skrót).

Czy aplikacja wymaga zewnętrznej instalacji SQL Server?
- Odpowiedź: Nie, używana jest wersja LocalDB, która instaluje się automatycznie z Visual Studio. Użytkownicy końcowi muszą mieć zainstalowany
  .NET Framework 4.7.2+ i LocalDB (jeśli nie publikowano aplikacji jako self-contained).

Czy aplikacja działa bez połączenia z internetem?
- Odpowiedź: Tak. Aplikacja działa lokalnie, bez dostępu do internetu.
  Wszelkie operacje wykonywane są na lokalnej bazie danych.

Gdzie znajdują się dane po instalacji?
- Odpowiedź: Dane są przechowywane w bazie danych LocalDB (plik .mdf), której lokalizacja jest skonfigurowana w pliku App.config.

Jakie dane muszą być wypełnione, aby dodać nowy rekord?
- Odpowiedź: Wszystkie wymagane pola muszą być wypełnione. Puste pola powodują zatrzymanie operacji i wyświetlenie komunikatu.

Nie działa przycisk „Edytuj” – dlaczego?
- Odpowiedź: Najczęściej przyczyną jest brak zaznaczonego rekordu w tabeli. Przed edycją zaznacz wiersz, który chcesz edytować.

Czy mogę usunąć rekord powiązany z innymi danymi?
-  Odpowiedź: Niektóre rekordy są chronione relacjami (np. nie można usunąć pracownika przypisanego do zakupu). Należy najpierw usunąć zależne dane.

Czy można dodawać wydziały z poziomu aplikacji?
- Odpowiedź: Nie, wydziały powinny być dodawane bezpośrednio do bazy danych lub rozszerzyć aplikację o dedykowane okno.

Czy aplikacja obsługuje więcej niż jednego użytkownika?
- Odpowiedź: Nie, aplikacja nie posiada systemu logowania ani wielosesyjnego trybu pracy. Przeznaczona jest do pracy lokalnej, jednego użytkownika.

Czy mogę rozbudować projekt o nowe tabele?
- Odpowiedź: Tak. Należy zmodyfikować model danych (EDMX lub Code First), dodać widoki w WPF oraz zaktualizować warstwę logiki.

Czy można uruchomić aplikację na innym systemie niż Windows?
- Odpowiedź: Nie. Aplikacja WPF działa wyłącznie na systemie Windows. Alternatywą byłoby przeniesienie logiki do .NET MAUI lub Blazor.

Czy mogę przygotować backup danych?
- Odpowiedź: Tak. Można skopiować plik .mdf bazy danych lub napisać prostą funkcję eksportującą dane do .csv / .xml.


### Autor aplikacji ProjektZaliczeniowyPB ### 

- Imię i nazwisko: Piotr Bacior 
- Numer albumu: 15 722
- Uczelnia: WSEI Kraków
- Prowadzący: Krzysztof Molenda
- Przedmiot: Programowanie obietkowe - Lab.


### Diagram bazy danych ### 

![image](https://github.com/user-attachments/assets/47a3484c-e854-4083-a120-3e3173f16944)




