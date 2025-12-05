import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NgForm } from '@angular/forms';

interface User{
  IdUser?: number;
  vorname: string;
  nachname: string;
  rolle: {Name: string};
}

interface Car{
  IdCar?: number;
  marke: string;
  kennzeichen: string;
  standort: {Ort:string};
  farbe: string;
  typ: string;
  datumVonKauf: string;
}

@Component({
  selector: 'app-admin-form',
  standalone: false,
  templateUrl: './admin-form.html',
  styleUrl: './admin-form.css'
})
export class AdminForm implements OnInit{
  
  newUser: User = { vorname: '', nachname: '', rolle:{Name: ''}};
  newCar: Car = { marke: '', kennzeichen: '', standort:{Ort: ''}, farbe: '', typ: '', datumVonKauf: ''};

  users: User[] = [];
  cars: Car[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.loadUsers();
    this.loadCars();
  }

  loadUsers(){
    this.http.get<User[]>('/api/User').subscribe(data => this.users = data);
  }

  addUser(form: NgForm){
    this.http.post<User>('/api/User', this.newUser).subscribe({
      next: (user) => {
        this.users.push(user);
        form.resetForm({rolle: ''})
      },
      error: () => alert("Fehler beim Hinzufügen des Benutzers")
    });
  }

  deleteUser(id?: number) {
    if (!id) return;
    this.http.delete(`/api/User/${id}`).subscribe({
      next: () => this.users = this.users.filter(u => u.IdUser !== id),
      error: () => alert("Fehler beim Löschen des Benutzers")
    });
  }

  loadCars(){
    this.http.get<Car[]>('/api/Fahrzeuge').subscribe(data => this.cars = data);
  }

  addCar(form: NgForm){
    this.http.post<Car>('/api/Fahrzeuge', this.newCar).subscribe({
      next: (car) => {
        this.cars.push(car);
        form.resetForm();
      },
      error: () => alert("Fehler beim Hinzufügen des Cars")
      });
  }

  deleteCar(id?: number) {
    if (!id) return;
    this.http.delete(`/api/Fahrzeuge/${id}`).subscribe({
      next: () => this.cars = this.cars.filter(c => c.IdCar !== id),
      error: () => alert("Fehleer beim Löschen des Fahrzegs") 
    });
  }
}
