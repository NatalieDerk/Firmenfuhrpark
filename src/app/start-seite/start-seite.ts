import { Component } from '@angular/core';

@Component({
  selector: 'app-start-seite',
  standalone: false,
  templateUrl: './start-seite.html',
  styleUrl: './start-seite.css'
})
export class StartSeite {
  vorname: string = "";
  nachname: string = "";

  onSubmit(){
    if(this.vorname && this.nachname){
      console.log("Login success:" , this.vorname, this.nachname);
      alert("Willkommen, ${this.vorname} ${this.nachname}!");
    }
  }
}
