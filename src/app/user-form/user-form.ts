import { Component } from '@angular/core';
import { tick } from '@angular/core/testing';

@Component({
  selector: 'app-user-form',
  standalone: false,
  styleUrls: ['./user-form.css'],
  templateUrl: '/user-form.html'

})
export class UserForm {

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

  onSubmit() {
    console.log(this.user);
  }
}
