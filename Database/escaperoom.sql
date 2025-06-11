-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Cze 07, 2025 at 05:30 PM
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
-- Struktura tabeli dla tabeli `pokoje`
--

DROP TABLE IF EXISTS `pokoje`;
CREATE TABLE IF NOT EXISTS `pokoje` (
  `pokoj_id` int(11) NOT NULL AUTO_INCREMENT,
  `nazwa` varchar(150) NOT NULL,
  `opis` text DEFAULT NULL,
  `trudnosc` tinyint(4) NOT NULL,
  `cena_za_godzine` decimal(6,2) NOT NULL,
  `max_graczy` tinyint(4) NOT NULL,
  `czas_minut` int(11) NOT NULL,
  `status_pokoj` enum('wolny','zarezerwowany') DEFAULT NULL,
  PRIMARY KEY (`pokoj_id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `pokoje`
--

INSERT INTO `pokoje` (`pokoj_id`, `nazwa`, `opis`, `trudnosc`, `cena_za_godzine`, `max_graczy`, `czas_minut`, `status_pokoj`) VALUES
(1, '2 krasnale i 2 mutanty', 'Odkryj jaka tajemnice skrywaja krasnale przed mutatantami. Czy dawane sekrety wyjda na jaw? Co skrywa mroczna jaskinia tych istot', 3, 200.00, 4, 60, 'wolny'),
(2, 'Straszny szpital', 'Pudzisz w pouszczonym szpitalu. NIe pamietasz niczego. Nie ma masz przy sobie nic, nawet butów. Czy dolasz okdryc co sie stalo?', 4, 450.00, 6, 90, 'wolny');

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
  `status` enum('zarezerwowana','odwolana','zrealizowana') NOT NULL,
  `data_utworzenia` datetime NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`rezerwacja_id`),
  KEY `uzytkownik_id` (`uzytkownik_id`),
  KEY `pokoj_id` (`pokoj_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `rezerwacje`
--

INSERT INTO `rezerwacje` (`rezerwacja_id`, `uzytkownik_id`, `pokoj_id`, `data_rozpoczecia`, `liczba_osob`, `status`, `data_utworzenia`) VALUES
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
-- Dumping data for table `sesje`
--

INSERT INTO `sesje` (`sesja_id`, `rezerwacja_id`, `data_start`, `data_koniec`, `czas_zwiazania`, `czy_ukonczone`) VALUES
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
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `uzytkownicy`
--

INSERT INTO `uzytkownicy` (`uzytkownik_id`, `email`, `haslo_hash`, `imie`, `nazwisko`, `telefon`, `data_rejestracji`, `admin`) VALUES
(1, 'admin@er.pl', '*4ACFE3202A5FF5CF467898FC58AAB1D615029441', 'Tomasz', 'Witoldzin', '420692137', '2025-06-07 16:28:50', 1),
(2, 'user@hmail.pl', '*D5D9F81F5542DE067FFF5FF7A4CA4BDD322C578F', 'Wodzislaw', 'Adamczyk', '213742069', '2025-06-07 16:30:47', 0);

--
-- Constraints for dumped tables
--

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
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
