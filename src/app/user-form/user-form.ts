import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-user-form',
  standalone: false,  
  styleUrls: ['./user-form.css'],
  templateUrl: './user-form.html'

})
export class UserForm {

  // Steuert, ob eine Einzelbuchung oder eine Serienbuchung gewählt wurde
  serienbuchung: "true" | "false" | "" = "";

  user = {
    serienbuchung: null as boolean | null,
    date: "",
    timeFrom: "",
    timeTo: "",
    startDate: "",
    endDate: "",
    serienTimeFrom: "",
    serienTimeTo: "",
    standort: "",
    tag: "",
    grund: "",
    
  };
  constructor(private http: HttpClient) {}

  // Wird beim Absenden des Formulars ausgeführt
  onSubmit() {

    if ( typeof window === "undefined") return;
    const currentUser = JSON.parse(localStorage.getItem("user") || 'null')

    if (!currentUser?.idUser){
      alert("Benutzer nicht gefunden!");
      return;
    }

    const convertTime = (time: string | null): string | null => {
      if (!time) return null;
      const [h, m] = time.split(":");
      return `${h}:${m}:00`;
    };

    // Zumannenstellen der Formulardaten für das Backend
    const payload = {
      IdUser: currentUser.idUser,
      IdManager: 0,
      IdCar: 0,
      IdOrt: parseInt(this.user.standort),

      // Datum abhöngig von Einzel- oder Serienbuchung
      Startdatum: this.user.serienbuchung ? this.user.startDate : this.user.date,
      Enddatum: this.user.serienbuchung ? this.user.endDate : this.user.date,

      // Uhrzeit als String übergaben
      StartZeit: this.user.serienbuchung ? convertTime(this.user.serienTimeFrom) : convertTime(this.user.timeFrom),
      EndZeit: this.user.serienbuchung ? convertTime(this.user.serienTimeTo) : convertTime(this.user.timeTo),
      Status: "pending",
      GrundDerBuchung: this.user.grund
    };

    // Anfrage an das Backend senden
    this.http.post('/api/formular', payload).subscribe({
      next: () => {alert("Buchung erfolgreich ");
      this.resetForm();
      window.location.href = '/manager-form';
      },
      error: () => alert("Fheler beim Speichern der Buchung")
    });
  }

  // Formular zurücksetzen
  resetForm() {
    this.user = {
      serienbuchung: null,
      date: "",
      timeFrom: "",
      timeTo: "",
      startDate: "", 
      endDate: "",
      serienTimeFrom: "",
      serienTimeTo: "",
      standort: "",
      tag: "",
      grund: "",
    };
    this.serienbuchung = "";
  }
}
