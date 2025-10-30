import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-start-seite',
  standalone: false,
  templateUrl: './start-seite.html',
  styleUrls: ['./start-seite.css']
})
export class StartSeite {
  vorname: string = "";
  nachname: string = "";
  
  constructor(private router: Router){}

  onSubmit(){
    if(this.vorname && this.nachname){
      console.log("Login success:" , this.vorname, this.nachname);
      alert(`Willkommen, ${this.vorname} ${this.nachname}!`);
      this.router.navigate(['/user-form']);
    }
  }
}
