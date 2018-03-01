-- phpMyAdmin SQL Dump
-- version 4.6.5.2
-- https://www.phpmyadmin.net/
--
-- Host: localhost:8889
-- Generation Time: Mar 01, 2018 at 01:45 AM
-- Server version: 5.6.35
-- PHP Version: 7.0.15

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";

--
-- Database: `library`
--
CREATE DATABASE IF NOT EXISTS `library` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;
USE `library`;

-- --------------------------------------------------------

--
-- Table structure for table `authors`
--

CREATE TABLE `authors` (
  `id` int(11) NOT NULL,
  `firstName` varchar(255) NOT NULL,
  `lastName` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

--
-- Dumping data for table `authors`
--

INSERT INTO `authors` (`id`, `firstName`, `lastName`) VALUES
(1, 'Ross', 'Bob'),
(2, 'Johnson', 'Dwayne'),
(3, 'Ondracek', 'Kayla'),
(4, 'Anderson', 'Cameron');

-- --------------------------------------------------------

--
-- Table structure for table `books`
--

CREATE TABLE `books` (
  `id` int(11) NOT NULL,
  `name` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `books`
--

INSERT INTO `books` (`id`, `name`) VALUES
(1, 'A Good Book'),
(3, 'How To Paint'),
(4, 'How To Be A Rock'),
(5, 'Punching Cam'),
(7, 'Blocking Kayla\'s Punch');

-- --------------------------------------------------------

--
-- Table structure for table `books_authors`
--

CREATE TABLE `books_authors` (
  `id` int(11) NOT NULL,
  `book_id` int(11) NOT NULL,
  `author_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `books_authors`
--

INSERT INTO `books_authors` (`id`, `book_id`, `author_id`) VALUES
(1, 1, 0),
(3, 3, 1),
(4, 4, 2),
(5, 5, 3),
(7, 7, 4);

-- --------------------------------------------------------

--
-- Table structure for table `checkouts`
--

CREATE TABLE `checkouts` (
  `id` int(11) NOT NULL,
  `copy_id` int(11) NOT NULL,
  `patron_id` int(11) NOT NULL,
  `checkout_date` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `copies`
--

CREATE TABLE `copies` (
  `id` int(11) NOT NULL,
  `book_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `copies`
--

INSERT INTO `copies` (`id`, `book_id`) VALUES
(1, 5),
(2, 5),
(3, 3),
(4, 3),
(5, 4),
(6, 1),
(7, 1);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `authors`
--
ALTER TABLE `authors`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `books`
--
ALTER TABLE `books`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `books_authors`
--
ALTER TABLE `books_authors`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `checkouts`
--
ALTER TABLE `checkouts`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `copies`
--
ALTER TABLE `copies`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `authors`
--
ALTER TABLE `authors`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;
--
-- AUTO_INCREMENT for table `books`
--
ALTER TABLE `books`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;
--
-- AUTO_INCREMENT for table `books_authors`
--
ALTER TABLE `books_authors`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;
--
-- AUTO_INCREMENT for table `checkouts`
--
ALTER TABLE `checkouts`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `copies`
--
ALTER TABLE `copies`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;
