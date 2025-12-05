import { Component, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { isPlatformBrowser } from '@angular/common';

interface Booking {
  idForm: number;
  idUser: number;
  startdatum: string;
  enddatum: string;
  startZeit: string;
  endZeit: string;
  standort: string;
  grundDerBuchung: string;
  status: string;
  user?: {
    vorname: string;
    nachname: string;
  };
}

@Component({
  selector: 'app-manager-form',
  standalone: false,
  templateUrl: './manager-form.html',
  styleUrl: './manager-form.css'
})
export class ManagerForm implements OnInit {

  // Liste aller offenen Buchungsanfragen
  bookings: Booking[] = [];

  // Verfügbare Fahrzeuge für die Auswahl
  cars: any[] = [];

  // Id des aktuell eingeloggten Managers
  currentManagerId: number | null = null;

  // Ausgewähltes Fahrzeug pro Formular
  selectedFahrzeuge: {[key: number]: number[]} = {};

constructor(private http: HttpClient,
            @Inject(PLATFORM_ID) private platformId: Object
) {}

// Wird beim Laden der Seite aufgerufen
ngOnInit() {

  if (isPlatformBrowser(this.platformId)){
    const user = JSON.parse(localStorage.getItem('user') || 'null');
    this.currentManagerId = user?.idUser ?? null;
  }

  this.loadBookings();
  this.loadCars();
}

// Lädt alle ausstehenden Buchungsanfragen
loadBookings(){
  this.http.get<Booking[]>('/api/formular/pending').subscribe(data => {
    this.bookings = data;
  })
}

// Lädt die Fahrzeugliste aus der Datenbank
loadCars(){
  this.http.get<any[]>('/api/fahrzeuge').subscribe(data => this.cars = data);
}

// Bestätigt eine Buchungsanfrage
approveBooking(idForm: number) {
  const carId = this.selectedFahrzeuge[idForm];

  if(!carId || carId.length == 0) {
    alert("Bitte ein Fahrzeug auswählen!");
    return;
  }
  const url = `/api/formular/approve/${idForm}?idCar=${carId}&idManager=${this.currentManagerId}&status`;

  this.http.put(url, {})
    .subscribe({
      next: () => {
        alert("Buchung bastätigt.");
        this.loadBookings();
      },
      error: () => alert("Fehler beim Bestätigen der Buchung")
    });

}

rejectBooking(idForm: number) {
  const url = `/api/formular/approve/${idForm}?idManager=${this.currentManagerId}`;

  this.http.put(url, {})
    .subscribe({
      next: () => {
        alert("Buchung storniert.");
        this.loadBookings();
      },
      error: () => alert("Fehler beim Stornieren der Buchung")
    });
}
}
