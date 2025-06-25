-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Cze 25, 2025 at 04:55 PM
-- Wersja serwera: 10.4.32-MariaDB
-- Wersja PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `escaperoom`
--
CREATE DATABASE IF NOT EXISTS `escaperoom` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `escaperoom`;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `platnosci`
--

DROP TABLE IF EXISTS `platnosci`;
CREATE TABLE IF NOT EXISTS `platnosci` (
  `platnosc_id` int(11) NOT NULL AUTO_INCREMENT,
  `rezerwacja_id` int(11) NOT NULL,
  `uzytkownik_id` int(11) NOT NULL,
  `pokoj_id` int(11) NOT NULL,
  `metoda_platnosci` enum('karta','gotowka') NOT NULL,
  `numer_transakcji` int(11) NOT NULL,
  PRIMARY KEY (`platnosc_id`),
  KEY `rezerwacja_id` (`rezerwacja_id`),
  KEY `uzytkownik_id` (`uzytkownik_id`),
  KEY `pokoj_id` (`pokoj_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Tabela Truncate przed wstawieniem `platnosci`
--

TRUNCATE TABLE `platnosci`;
-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `pokoje`
--

DROP TABLE IF EXISTS `pokoje`;
CREATE TABLE IF NOT EXISTS `pokoje` (
  `pokoj_id` int(11) NOT NULL AUTO_INCREMENT,
  `nazwa` varchar(150) NOT NULL,
  `opis` text DEFAULT NULL,
  `trudnosc` tinyint(4) NOT NULL,
  `cena` decimal(6,2) NOT NULL,
  `max_graczy` tinyint(4) NOT NULL,
  `czas_minut` int(11) NOT NULL,
  `status_pokoj` enum('wolny','zarezerwowany') DEFAULT NULL,
  PRIMARY KEY (`pokoj_id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Tabela Truncate przed wstawieniem `pokoje`
--

TRUNCATE TABLE `pokoje`;
--
-- Dumping data for table `pokoje`
--

INSERT DELAYED IGNORE INTO `pokoje` (`pokoj_id`, `nazwa`, `opis`, `trudnosc`, `cena`, `max_graczy`, `czas_minut`, `status_pokoj`) VALUES
(1, '2 krasnale i 2 mutanty', 'Odkryj jaka tajemnice skrywaja krasnale przed mutatantami. Czy dawane sekrety wyjda na jaw? Co skrywa mroczna jaskinia tych istot', 3, 200.00, 4, 60, 'wolny'),
(2, 'Straszny szpital', 'Pudzisz w pouszczonym szpitalu. NIe pamietasz niczego. Nie ma masz przy sobie nic, nawet butów. Czy dolasz okdryc co sie stalo?', 4, 450.00, 6, 90, 'wolny'),
(4, 'Zaginiony Skarb', 'Przygoda w stylu Indiany Jonesa z zagadkami i pulapkami.', 3, 120.00, 5, 60, 'wolny'),
(5, 'Laboratorium Szalonego Naukowca', 'Eksperymenty, wybuchy i niebezpieczne substancje.', 4, 140.00, 4, 75, 'wolny'),
(6, 'Tajemnice Wiktorianskiego Dworu', 'Mroczna historia rodzinna pelna sekretow.', 2, 100.00, 6, 60, 'wolny'),
(7, 'Ucieczka z Wiezienia', 'Realistyczny pokoj z celami, kratami i straznikami.', 5, 150.00, 4, 90, 'wolny'),
(8, 'Kosmiczna Misja', 'Pokoj w stylu sci-fi z efektami swietlnymi i dzwiekowymi.', 3, 130.00, 5, 70, 'wolny');

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `recenzje`
--

DROP TABLE IF EXISTS `recenzje`;
CREATE TABLE IF NOT EXISTS `recenzje` (
  `recenzja_id` int(11) NOT NULL AUTO_INCREMENT,
  `uzytkownik_id` int(11) NOT NULL,
  `pokoj_id` int(11) NOT NULL,
  `opinia` text NOT NULL,
  `data_dodania` datetime NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`recenzja_id`),
  KEY `uzytkownik_id` (`uzytkownik_id`),
  KEY `pokoj_id` (`pokoj_id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Tabela Truncate przed wstawieniem `recenzje`
--

TRUNCATE TABLE `recenzje`;
-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `rezerwacje`
--

DROP TABLE IF EXISTS `rezerwacje`;
CREATE TABLE IF NOT EXISTS `rezerwacje` (
  `rezerwacja_id` int(11) NOT NULL AUTO_INCREMENT,
  `uzytkownik_id` int(11) NOT NULL,
  `pokoj_id` int(11) NOT NULL,
  `data_rozpoczecia` datetime NOT NULL,
  `liczba_osob` tinyint(4) NOT NULL,
  `status` enum('zarezerwowana','odwolana','zrealizowana','oplacona') NOT NULL,
  `data_utworzenia` datetime NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`rezerwacja_id`),
  KEY `uzytkownik_id` (`uzytkownik_id`),
  KEY `pokoj_id` (`pokoj_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Tabela Truncate przed wstawieniem `rezerwacje`
--

TRUNCATE TABLE `rezerwacje`;
--
-- Dumping data for table `rezerwacje`
--

INSERT DELAYED IGNORE INTO `rezerwacje` (`rezerwacja_id`, `uzytkownik_id`, `pokoj_id`, `data_rozpoczecia`, `liczba_osob`, `status`, `data_utworzenia`) VALUES
(1, 2, 2, '2025-05-29 21:00:00', 3, 'zrealizowana', '2025-05-29 18:00:00'),
(2, 2, 1, '2025-06-13 10:00:00', 4, 'zarezerwowana', '2025-06-07 17:00:00');

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `sesje`
--

DROP TABLE IF EXISTS `sesje`;
CREATE TABLE IF NOT EXISTS `sesje` (
  `sesja_id` int(11) NOT NULL AUTO_INCREMENT,
  `rezerwacja_id` int(11) NOT NULL,
  `data_start` datetime NOT NULL,
  `data_koniec` datetime DEFAULT NULL,
  `czas_zwiazania` int(11) DEFAULT NULL,
  `czy_ukonczone` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`sesja_id`),
  KEY `rezerwacja_id` (`rezerwacja_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Tabela Truncate przed wstawieniem `sesje`
--

TRUNCATE TABLE `sesje`;
--
-- Dumping data for table `sesje`
--

INSERT DELAYED IGNORE INTO `sesje` (`sesja_id`, `rezerwacja_id`, `data_start`, `data_koniec`, `czas_zwiazania`, `czy_ukonczone`) VALUES
(1, 1, '2025-05-29 21:00:00', '2025-05-29 22:30:00', 70, 1);

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `uzytkownicy`
--

DROP TABLE IF EXISTS `uzytkownicy`;
CREATE TABLE IF NOT EXISTS `uzytkownicy` (
  `uzytkownik_id` int(11) NOT NULL AUTO_INCREMENT,
  `email` varchar(255) NOT NULL,
  `haslo_hash` varchar(255) NOT NULL,
  `imie` varchar(100) NOT NULL,
  `nazwisko` varchar(100) NOT NULL,
  `telefon` varchar(20) DEFAULT NULL,
  `data_rejestracji` datetime NOT NULL DEFAULT current_timestamp(),
  `admin` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`uzytkownik_id`),
  UNIQUE KEY `email` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Tabela Truncate przed wstawieniem `uzytkownicy`
--

TRUNCATE TABLE `uzytkownicy`;
--
-- Dumping data for table `uzytkownicy`
--

INSERT DELAYED IGNORE INTO `uzytkownicy` (`uzytkownik_id`, `email`, `haslo_hash`, `imie`, `nazwisko`, `telefon`, `data_rejestracji`, `admin`) VALUES
(1, 'admin@er.pl', 'RG2ByvLxSgzE8jpxGvUwDA==:plRI5cXHchswtso04i5CBJmRTbRn4yJcv+5ODCYkLxo=', 'Tomasz', 'Witoldzin', '420692137', '2025-06-07 16:28:50', 1),
(2, 'user@hmail.pl', 'xrNGm4Kmi1wdBTLAThR4zQ==:5/d3G5WKWhAnlAaBKox0JKV2lz6+CBTvF98KJ/5EiTw=', 'Wodzislaw', 'Adamczyk', '213742069', '2025-06-07 16:30:47', 0),
(3, 'pawelzambrzycki03@gmail.com', '82Xp2nh4cCXt2xSG1J0WJw==:bMO/zumRVzJTu8PBlMatoNlKv7Sz0DSWLgXUPt6y/CE=', 'Pawel', 'Zambrzycki', '606978989', '2025-06-25 16:45:35', 0),
(4, 'mszandala2@gmail.com', 'OWWpclF4u1NcbWDUAZUnOA==:em6tHUxUMnB9xI33Eip0mfMRJg08R4EmdE1+DvNZRVc=', 'Martyna', 'Szandala', '500582796', '2025-06-25 16:47:53', 0),
(5, 'niskizgred@onet.pl', '2FEHtz44b3BFRdLR3kq5qg==:0lTkuD2nIw2y5qWUQxV2RN7licX9gNb3h/2nmClnOMs=', 'Jakub', 'Jaromin', '911328514', '2025-06-25 16:48:29', 0),
(6, 'r.brociek@polsl.pl', 'g7P4E+XqJSjRevc0gKTkqw==:9dTYp0lWORAF7mWFzasRs0mlrAIOfZ/ACRG8pTUT/2E=', 'Rafa?', 'Brociek', '713517841', '2025-06-25 16:50:27', 0),
(7, 'kozaboza@o2.pl', 'lRGOwTjAFD70ofaQh28sbA==:cXn/Be1lrTvyAheAzPyUXDCfBpcJpLM+MmXyLiCVbCY=', 'Natalia', 'Tomala', '831317581', '2025-06-25 16:51:02', 0);

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `wiadomosci`
--

DROP TABLE IF EXISTS `wiadomosci`;
CREATE TABLE IF NOT EXISTS `wiadomosci` (
  `id_wiadomosci` int(11) NOT NULL AUTO_INCREMENT,
  `wiadomosc` varchar(500) NOT NULL,
  `uzytkownik_id` int(11) NOT NULL,
  PRIMARY KEY (`id_wiadomosci`),
  KEY `uzytkownik_id` (`uzytkownik_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Tabela Truncate przed wstawieniem `wiadomosci`
--

TRUNCATE TABLE `wiadomosci`;
--
-- Constraints for dumped tables
--

--
-- Constraints for table `platnosci`
--
ALTER TABLE `platnosci`
  ADD CONSTRAINT `platnosci_ibfk_1` FOREIGN KEY (`rezerwacja_id`) REFERENCES `rezerwacje` (`rezerwacja_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `platnosci_ibfk_2` FOREIGN KEY (`uzytkownik_id`) REFERENCES `uzytkownicy` (`uzytkownik_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `platnosci_ibfk_3` FOREIGN KEY (`pokoj_id`) REFERENCES `pokoje` (`pokoj_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `recenzje`
--
ALTER TABLE `recenzje`
  ADD CONSTRAINT `Recenzje_ibfk_1` FOREIGN KEY (`uzytkownik_id`) REFERENCES `uzytkownicy` (`uzytkownik_id`),
  ADD CONSTRAINT `Recenzje_ibfk_2` FOREIGN KEY (`pokoj_id`) REFERENCES `pokoje` (`pokoj_id`);

--
-- Constraints for table `rezerwacje`
--
ALTER TABLE `rezerwacje`
  ADD CONSTRAINT `Rezerwacje_ibfk_1` FOREIGN KEY (`uzytkownik_id`) REFERENCES `uzytkownicy` (`uzytkownik_id`),
  ADD CONSTRAINT `Rezerwacje_ibfk_2` FOREIGN KEY (`pokoj_id`) REFERENCES `pokoje` (`pokoj_id`);

--
-- Constraints for table `sesje`
--
ALTER TABLE `sesje`
  ADD CONSTRAINT `Sesje_ibfk_1` FOREIGN KEY (`rezerwacja_id`) REFERENCES `rezerwacje` (`rezerwacja_id`);

--
-- Constraints for table `wiadomosci`
--
ALTER TABLE `wiadomosci`
  ADD CONSTRAINT `wiadomosci_ibfk_1` FOREIGN KEY (`uzytkownik_id`) REFERENCES `uzytkownicy` (`uzytkownik_id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
