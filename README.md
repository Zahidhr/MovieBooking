# 🎬 Movie Booking Application

A clean-architecture, domain-driven **Movie Booking Application** built with **.NET**. The project is designed both as a **production-grade reference** and a **learning-focused system** that demonstrates how to model a real-world booking domain with correctness, atomicity, and testability in mind.

The system is intentionally structured so the core **Booking** logic can later be reused for other domains such as events, transportation, or venue reservations.

---

## 📌 High-Level Goals

* Model a **real-world seat booking domain** with strong invariants
* Ensure **atomic seat reservation** (no double booking)
* Enforce **time-bound holds** before booking
* Follow **Clean Architecture + DDD-inspired design**
* Be **test-driven**, especially for business rules
* Keep infrastructure concerns isolated

---

## 🧱 Architecture Overview

The solution follows Clean Architecture principles:

```
MovieBooking.Api            → HTTP layer (Controllers)
MovieBooking.Application    → Use cases, services, DTOs
MovieBooking.Domain         → Core domain models & rules
MovieBooking.Infrastructure → EF Core, repositories, persistence
MovieBooking.Tests          → Unit tests (domain + application)
MovieBooking.IntegrationTests → Integration & EF tests
```

### Dependency Rule

* Domain has **no dependencies**
* Application depends on Domain
* Infrastructure depends on Application + Domain
* API depends on Application

---

## 🧠 Domain Model Overview (ERD-Aligned)

### Configuration / Setup (Admin-defined)

* **Cinema** – Physical cinema location
* **Layout** – Seating blueprint (rows × seats)
* **LayoutSeatTypes** – Seat type per seat position (Standard, VIP, etc.)
* **Room** – A screen within a cinema, linked to a layout
* **Movie** – Movie metadata

### Runtime / Booking Flow

* **Screening** – Movie shown in a room at a specific time
* **SeatReservation** – Single source of truth for seat availability
* **Hold** – Temporary reservation of seats
* **Booking** – Finalized purchase
* **BookingSeat** – Seats included in a booking with price paid

---

## 🔄 Core Booking Flow

1. Admin creates Cinema, Layout, Room, Movie
2. Admin schedules a Screening
3. User views seat map for a screening
4. User creates a **Hold** on selected seats (time-limited)
5. Seats become temporarily unavailable
6. User converts Hold into **Booking**
7. Seats become permanently booked

---

## ✅ Functional Requirements

### 1. Cinema & Room Management

* Create and manage cinemas
* Define seating layouts (row count, seats per row)
* Assign seat types to individual seat positions
* Create rooms linked to layouts

---

### 2. Movie & Screening Management

* Create movies
* Schedule screenings in rooms
* Prevent overlapping screenings in the same room
* Each screening is tied to one room and one movie

---

### 3. Seat Map & Availability

* Retrieve full seat map for a screening
* Seat map must include:

  * Row number
  * Seat number
  * Seat type
  * Seat status: `Available | Held | Booked`
* Seat availability must reflect:

  * Active holds
  * Completed bookings
  * Expired holds (released seats)

---

### 4. Hold Seats (Atomic Operation)

* Users can place a hold on one or more seats
* Hold must:

  * Be atomic (all seats or none)
  * Have an expiration time
  * Prevent other users from holding the same seats
* Validation rules:

  * At least one seat is required
  * Seats must exist in the layout
  * Seats must be available at time of hold

---

### 5. Hold Expiration

* Holds automatically expire after a configured duration
* Expired holds:

  * Cannot be booked
  * Release seats back to availability
* Expiration is enforced on access (no background job required initially)

---

### 6. Booking

* Users can convert an active hold into a booking
* Booking rules:

  * Hold must exist
  * Hold must not be expired
  * Hold must not already be booked
  * All seats in hold must still be valid
* Booking is atomic:

  * Booking record is created
  * BookingSeats are created
  * SeatReservations move from `Held` → `Booked`

---

### 7. Seat Reservation Integrity

* A seat is uniquely identified by:

  * ScreeningId + RowNumber + SeatNumber
* A seat can never be:

  * Held by two holds
  * Booked twice
* SeatReservations act as the **single source of truth**

---

### 8. Error Handling & Validation

* Consistent error codes for client handling
* Clear distinction between:

  * Validation errors (bad input)
  * Conflict errors (business rule violations)
* Examples:

  * EMPTY_SEAT_SELECTION
  * SEATS_UNAVAILABLE
  * HOLD_EXPIRED
  * HOLD_ALREADY_BOOKED

---

## 🧪 Testing Strategy

* **Unit Tests**

  * Domain invariants
  * Hold creation rules
  * Booking conversion rules

* **Integration Tests**

  * EF Core persistence
  * Transactional behavior
  * Concurrency scenarios

* Tests prioritize **business correctness over infrastructure details**

---

## ⏱ Time & Atomicity

* All time-based logic uses an abstracted **Clock**
* Hold creation and booking operations are **transactional**
* Seat availability checks are always performed inside transactions

---

## 🚀 Future Extensions

* Payment integration
* Pricing rules & discounts
* Multiple currencies
* Event / transport booking reuse
* Background jobs for cleanup
* Read-model optimization (CQRS-style)

---

## 📄 License

This project is intended for educational and experimental purposes. Feel free to fork, adapt, and extend.

---

## ✨ Philosophy

> Correctness first.
> Make illegal states unrepresentable.
> If a seat is booked twice, the system is wrong.

---
