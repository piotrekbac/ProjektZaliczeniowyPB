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


### Autor aplikacji ProjektZaliczeniowyPB ### 

- Imię i nazwisko: Piotr Bacior 
- Numer albumu: 15 722
- Uczelnia: WSEI Kraków
- Prowadzący: Krzysztof Molenda
- Przedmiot: Programowanie obietkowe - Lab.





