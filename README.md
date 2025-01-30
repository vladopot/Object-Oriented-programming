# KOLEGIUM INFORMATYKI STOSOWANEJ

**Kierunek:** INFORMATYKA  
**Przedmiot:** Programowanie obiektowe  

**Uladzislau Sopat**  
**Nr albumu studenta:** w71048  

## Aplikacje do zarządzania procesem treningowym

**Prowadzący:** inż. Damian Kontek  

**Rzeszów 2025**  

---

## Spis treści
1. [Cel projektu](#1-cel-projektu)
2. [Wymagania funkcjonalne i niefunkcjonalne](#2-wymagania-funkcjonalne-i-niefunkcjonalne)
3. [Harmonogram prac](#3-harmonogram-prac)
4. [Opis rozwiązania](#4-opis-rozwiązania)
5. [Podsumowanie](#5-podsumowanie)


## 1. Cel Projektu

Celem projektu jest stworzenie aplikacji do zarządzania procesem treningowym, umożliwiającej użytkownikowi planowanie, rejestrowanie i analizowanie swoich treningów. Aplikacja pozwala na łatwe śledzenie postępów, zarządzanie danymi fizycznymi oraz obliczanie niezbędnego BŻU, co wspiera efektywność treningów i osiąganie celów zdrowotnych.

## 2. Wymagania funkcjonalne i niefunkcjonalne

### Wymagania funkcjonalne:
- Logowanie użytkownika oraz możliwość rejestracji w systemie
- Zarządzanie kontem użytkownika (edycja danych)
- Tworzenie zaplanowanych treningów z możliwością edycji daty i sprawdzeniem jej poprawności
- Dodawanie i usuwanie treningów z historii
- Dodawanie danych fizycznych użytkownika (wysokość, waga, cele)
- Obliczanie potrzebnego BŻU na podstawie danych użytkownika
- Wyświetlanie wykresu szacowanej zmiany masy ciała w ciągu roku

### Wymagania niefunkcjonalne:
- Intuicyjny i przyjazny interfejs użytkownika
- Wydajność aplikacji
- Bezpieczeństwo danych użytkownika

## 3. Harmonogram prac

| Zadanie | Termin rozpoczęcia | Termin zakończenia |
|---------|--------------------|--------------------|
| Analiza wymagań | 01/01/2025 | 02/01/2025 |
| Projektowanie interfejsu | 03/01/2025 | 06/01/2025 |
| Implementacja logowania i rejestracji | 07/01/2025 | 10/01/2025 |
| Dodawanie i edytowanie treningów | 11/01/2025 | 13/01/2025 |
| Funkcjonalność historii treningów | 14/01/2025 | 16/01/2025 |
| Implementacja profilu użytkownika | 17/01/2025 | 19/01/2025 |
| Obliczanie BŻU i wykres wagi | 20/01/2025 | 24/01/2025 |
| Testowanie i poprawki | 23/01/2025 | 26/01/2025 |
| Dokumentacja i zakończenie | 27/01/2025 | 30/01/2025 |

## 4. Opis rozwiązania

Aplikacja została stworzona w języku **C#** z wykorzystaniem frameworka **.NET**. Interfejs użytkownika oparty jest na technologii **WPF (Windows Presentation Foundation)**, co pozwala na łatwe tworzenie nowoczesnych aplikacji desktopowych z bogatym interfejsem graficznym. Dodatkowo aplikacja używa **SQLite** jako bazy danych lokalnej do przechowywania danych użytkowników i informacji o treningach. Dzięki **SQLite** aplikacja jest lekka i szybka w działaniu, idealna do małych projektów desktopowych, gdzie nie ma potrzeby stosowania dużych baz danych.

### Funkcjonalności:
- **Logowanie i rejestracja użytkownika** – przechowywanie danych użytkowników w **SQLite**.
- **Zarządzanie profilem użytkownika** – możliwość edytowania danych: imię, nazwisko, wzrost, waga, wiek i cel.
- **Zarządzanie treningami** – użytkownicy mogą tworzyć zaplanowane treningi oraz dodawać je do historii.
- **Obliczanie BŻU** – na podstawie danych użytkownika aplikacja oblicza zalecane wartości **białka, węglowodanów i tłuszczy**.
- **Analiza postępów** – wykres zmiany wagi w ciągu roku.

### Technologie użyte w projekcie:
- **C#** – język programowania
- **WPF** – technologia do tworzenia aplikacji desktopowych
- **SQLite** – relacyjna baza danych
- **XAML** – język do definiowania interfejsu użytkownika
- **MVVM (Model-View-ViewModel)** – wzorzec projektowy oddzielający logikę od interfejsu użytkownika

## 5. Podsumowanie

Projekt umożliwia użytkownikom efektywne zarządzanie swoimi treningami i śledzenie postępów. W ramach projektu udało się zrealizować wszystkie kluczowe funkcje, takie jak **rejestracja i logowanie użytkownika, dodawanie treningów, zarządzanie historią oraz obliczanie BŻU**.

---

**Link do repozytorium:** [vladopot/Object-Oriented-programming at project](https://github.com/vladopot/Object-Oriented-programming)
